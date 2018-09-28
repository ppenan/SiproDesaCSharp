import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import * as moment from 'moment';
import { LocalDataSource } from 'ng2-smart-table';
import { Actividad } from './model/Actividad';
import { ActivatedRoute } from "@angular/router";
import { AuthService } from '../../auth.service';
import { UtilsService } from '../../utils.service';
import { HttpClient } from '@angular/common/http';
import { MatDialog } from '@angular/material';
import { Router } from '@angular/router';
import { DialogActividadTipo, DialogOverviewActividadTipo } from '../../../assets/modals/actividadtipo/actividad-tipo';
import { FormControl } from '@angular/forms';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { ButtonDeleteComponent } from '../../../assets/customs/ButtonDeleteComponent';
import { ButtonDownloadComponent } from '../../../assets/customs/ButtonDownloadComponent';
import { DialogDownloadDocument, DialogOverviewDownloadDocument } from '../../../assets/modals/documentosadjuntos/documento-adjunto';
import { DialogResponsables, DialogOverviewResponsables } from '../../../assets/modals/responsables/modal-responsables';

export interface AcumulacionCosto {
  id: number;
  nombre: string;
  usuarioActualizo: string;
  usuarioCreo: string;
  fechaActualizacion : Date;
  fechaCreacion: Date;
  estado: number;
}

@Component({
  selector: 'app-actividad',
  templateUrl: './actividad.component.html',
  styleUrls: ['./actividad.component.css']
})
export class ActividadComponent implements OnInit {
  mostrarcargando : boolean;
  color = 'primary';
  mode = 'indeterminate';
  value = 50;
  diameter = 45;
  strokewidth = 3;

  colorGuardar = 'primary';
  modeGuardar = 'indeterminate';
  valueGuardar = 50;
  diameterGuardar = 20;
  strokewidthGuardar = 3;

  isLoggedIn : boolean;
  isMasterPage : boolean;
  esTreeview: boolean;
  esColapsado: boolean;
  congelado: number;
  source : LocalDataSource;
  totalActividades: number;
  busquedaGlobal: string;
  actividad: Actividad;
  objetoTipo: number;
  objetoId: number;
  paginaActual : number;
  tabActive: number;
  elementosPorPagina: number;
  numeroMaximoPaginas: number;
  esNuevo: boolean;
  sobrepaso: boolean;
  modalActividadTipo: DialogOverviewActividadTipo;
  camposdinamicos = [];
  dimensionSelected: number;
  asignado: number;
  bloquearCosto: boolean;
  objetoTipoNombre: string;
  objetoNombre: string;
  minFechaPadre: Date;
  acumulacion_costo = [];
  sourceArchivosAdjuntos: LocalDataSource;
  modalAdjuntarDocumento: DialogOverviewDownloadDocument;
  responsables = [];
  sourceResponsables: LocalDataSource;
  modalResponsalbes: DialogOverviewResponsables;
  mostrarguardado: boolean;

  dimensiones = [
    {value:1,nombre:'Dias',sigla:'d'}
  ];

  @ViewChild('search') divSearch: ElementRef;
  myControl = new FormControl();
  filteredAcumulacionCosto: Observable<AcumulacionCosto[]>;

  constructor(private route: ActivatedRoute, private auth: AuthService, private utils: UtilsService, private http: HttpClient, private dialog: MatDialog, private router: Router) { 
    this.isMasterPage = this.auth.isLoggedIn();
    this.utils.setIsMasterPage(this.isMasterPage);
    this.elementosPorPagina = utils._elementosPorPagina;
    this.numeroMaximoPaginas = utils._numeroMaximoPaginas;
    this.totalActividades = 0;

    this.route.params.subscribe(param => {
        this.objetoTipo = Number(param.objeto_tipo);
        this.objetoId = Number(param.objeto_id);
    })

    this.busquedaGlobal = null;
    this.tabActive = 0;
    this.congelado = 0;
    this.obtenerObjeto();
    this.actividad = new Actividad();
    this.modalActividadTipo = new DialogOverviewActividadTipo(dialog);

    this.filteredAcumulacionCosto = this.myControl.valueChanges
      .pipe(
        startWith(''),
        map(value => value ? this._filterAcumulacionCosto(value) : this.acumulacion_costo.slice())
      );

    this.modalAdjuntarDocumento = new DialogOverviewDownloadDocument(dialog);
    this.modalResponsalbes = new DialogOverviewResponsables(dialog);
    this.sourceResponsables = new LocalDataSource();
  }

  private _filterAcumulacionCosto(value: string): AcumulacionCosto[] {
    const filterValue = value.toLowerCase();
    return this.acumulacion_costo.filter(c => c.nombre.toLowerCase().indexOf(filterValue) === 0);
  }

  acumulacionCostoSeleccionado(value){
    this.actividad.acumulacionCostoid = value.id;
  }

  ngOnInit() {
    this.mostrarcargando=true;
    this.obtenerTotalActividades();
    this.obtenerAcumulacionCosto();
  }

  obtenerObjeto(){
    this.http.get('http://localhost:60045/api/Objecto/ObjetoPorId/' + this.objetoId + '/' + this.objetoTipo, { withCredentials : true}).subscribe(response =>{
      if(response['success'] == true){
        this.objetoTipoNombre = response['tiponombre'];
        this.objetoNombre = response['nombre'];

        let fechaInicioPadre = moment(response['fechaInicio'], 'DD/MM/YYYY').toDate();
				this.modificarFechaInicial(fechaInicioPadre);
      }
    })
  }

  modificarFechaInicial = function(fechaPadre){
    this.minFechaPadre = fechaPadre;
  }

  obtenerTotalActividades(){
    var data = {  
      filtro_busqueda: this.busquedaGlobal,
      objetoid: this.objetoId,
      tipo: this.objetoTipo,
      t: new Date().getTime()
    };

    this.http.post('http://localhost:60001/api/Actividad/NumeroActividadesPorObjeto', data, { withCredentials : true}).subscribe(response =>{
      if(response['success'] == true){
        this.totalActividades = response['total'];
        this.paginaActual = 1;
        if(this.totalActividades > 0){
          this.cargarTabla(this.paginaActual);
        }
        else{
          this.source = new LocalDataSource();
          this.mostrarcargando=false;
        }
      }
    })
  }

  cargarTabla(pagina? : number){
    this.mostrarcargando = true;
    var filtro = {
      objetoId: this.objetoId,
      tipo: this.objetoTipo,
      pagina: pagina,
      numeroActividades: this.elementosPorPagina,
      filtro_busqueda: this.busquedaGlobal,
      columna_ordenada: null,
      t:moment().unix()
    };

    this.http.post('http://localhost:60001/api/Actividad/ActividadesPaginaPorObjeto', filtro, { withCredentials : true}).subscribe(
      response =>{
        if(response['success'] == true){
          var data = response['actividades'];

          for(var i =0; i<data.length; i++){
            data[i].fechaInicio = data[i].fechaInicio != null ? moment(data[i].fechaInicio,'DD/MM/YYYY').toDate() : null;
            data[i].fechaFin = data[i].fechaFin != null ? moment(data[i].fechaFin,'DD/MM/YYYY').format('DD/MM/YYYY') : null;
          }

          this.source = new LocalDataSource(data);
          this.source.setSort([
              { field: 'id', direction: 'asc' }  // primary sort
          ]);
          this.busquedaGlobal = null;
      }
      this.mostrarcargando = false;
    })
  }

  nuevo(){
    this.esColapsado = true;
    this.esNuevo = true;
    this.tabActive = 0;
    this.dimensionSelected = 1;
    this.actividad.duracionDimension = this.dimensiones[this.dimensionSelected-1].sigla;
    this.sourceArchivosAdjuntos = new LocalDataSource();
    this.sourceResponsables = new LocalDataSource();
  }

  editar(){
    if(this.actividad.id != null){
      this.esColapsado = true;
      this.esNuevo = false;
      this.tabActive = 0;
      this.dimensionSelected = 1;
      this.actividad.duracionDimension = this.dimensiones[this.dimensionSelected-1].sigla;   

      if(this.actividad.acumulacionCostoid==2)
        this.bloquearCosto = true;
      else
        this.bloquearCosto = false;
      
      this.esNuevo = false;

      this.obtenerCamposDinamicos();
      this.obtenerAsignacionesRaci();
      this.getAsignado();
      this.getDocumentosAdjuntos(this.actividad.id, 5);
    }
  }

  borrar(){

  }

  filtrar(campo){
    this.busquedaGlobal = campo;
    this.obtenerTotalActividades();
  }

  onSelectRow(event) {
    this.actividad = event.data;
  }

  onDblClickRow(event) {
    this.actividad = event.data;
    this.editar();
  }

  handlePage(event){
    this.cargarTabla(event.pageIndex+1);
  }

  settings = {
    columns: {
      id: {
        title: 'ID',
        width: '6%',
        filter: false,
        type: 'html',
        valuePrepareFunction : (cell) => {
          return "<div class=\"datos-numericos\">" + cell + "</div>";
        }
      },
      nombre: {
        title: 'Nombre',
        width: '35%',
        filter: false,
      },
      descripcion: {
        title: 'Descripción',
        filter: false
      },
      usuarioCreo: {
        title: 'Usuario Creación',
        filter: false
      },
      fechaCreacion:{
        title: 'Fecha Creación',
        type: 'html',
        filter: false,
        valuePrepareFunction : (cell) => {
          return "<div class=\"datos-numericos\">" + moment(cell,'DD/MM/YYYY HH:mm:ss').format('DD/MM/YYYY HH:mm:ss') + "</div>";
        }
      }
    },
    actions: false,
    noDataMessage: 'No se obtuvo información...',
    attr: {
      class: 'table table-bordered grid estilo-letra'
    },
    hideSubHeader: true,
    pager:{
      display: false
    }
  };

  irAActividad(actividadId){
    if(this.actividad!=null){
      this.router.navigateByUrl('/main/actividad/'+ actividadId+'/'+5);
    }
  }

  verHistoria(){

  }

  agregarPagos(){

  }

  guardar(){
    if(this.actividad != null){
      this.mostrarguardado = true;
      for(var i=0; i < this.camposdinamicos.length; i++){
        if(this.camposdinamicos[i].tipo === 'fecha'){
          this.camposdinamicos[i].valor_f = this.camposdinamicos[i].valor != null ? moment(this.camposdinamicos[i].valor).format('DD/MM/YYYY') : "";        
        }
      }

      this.actividad.camposDinamicos = JSON.stringify(this.camposdinamicos);

      let asignaciones="";
			for (let x in this.sourceResponsables['data']){
				asignaciones = asignaciones.concat(asignaciones.length > 0 ? "|" : "", this.sourceResponsables['data'][x].id + "~" + this.sourceResponsables['data'][x].r); 
      }
      
      this.actividad.asignacionroles = asignaciones;
      this.actividad.objetoId = this.objetoId;
      this.actividad.objetoTipo = this.objetoTipo;

      var objetoHttp;

      if(this.actividad.id > 0){
        objetoHttp = this.http.put("http://localhost:60001/api/Actividad/Actividad/" + this.actividad.id, this.actividad, { withCredentials: true });
      }
      else{
        this.actividad.id=0;
        objetoHttp = this.http.post("http://localhost:60001/api/Actividad/Actividad", this.actividad, { withCredentials: true });
      }

      objetoHttp.subscribe(response => {
        if(response['success'] == true){
          this.actividad.id = response['id'];
          this.actividad.usuarioCreo = response['usuarioCreo'];
          this.actividad.fechaCreacion = response['fechaCreacion'];
          this.actividad.usuarioActualizo = response['usuarioActualizo'];
          this.actividad.fechaActualizacion = response['fechaActualizacion'];

          if(this.esTreeview){

          }
          else
            this.obtenerTotalActividades();

            this.utils.mensaje('success', 'Actividad ' + (this.esNuevo ? 'creada' : 'guardada') + ' con Éxito');
            this.mostrarguardado = false;
        }
        else{
          this.utils.mensaje('warning', 'Ocurrió un error al guardar la actividad');
          this.mostrarguardado = false;
        }
      })
    }
  }

  IrATabla(){
    this.esColapsado = false;
    this.actividad = new Actividad();
    this.sourceArchivosAdjuntos = new LocalDataSource();
    this.sourceResponsables = new LocalDataSource();
    this.responsables = [];
  }

  buscarActividadTipo(){
    this.modalActividadTipo.dialog.open(DialogActividadTipo, {
      width: '600px',
      height: '585px',
      data: { titulo: 'Actividad Tipo' }
    }).afterClosed().subscribe(result => {
      if(result != null){
        this.actividad.actividadTipoid = result.id;
        this.actividad.actividadtiponombre = result.nombre;

        this.obtenerCamposDinamicos();
      }
    })
  }

  obtenerCamposDinamicos(){
    var parametros ={
      t: new Date().getTime() 
    }
    this.http.get('http://localhost:60002/api/ActividadPropiedad/ActividadPropiedadPorTipo/'+this.actividad.id+'/'+this.actividad.actividadTipoid,  { withCredentials: true }).subscribe(response => {
      if (response['success'] == true) {
        this.camposdinamicos = response['actividadpropiedades'];
        for(var i=0; i<this.camposdinamicos.length; i++){
          switch(this.camposdinamicos[i].tipo){
            case "fecha":
              this.camposdinamicos[i].valor = this.camposdinamicos[i].valor != null ? moment(this.camposdinamicos[i].valor, 'DD/MM/YYYY').toDate() : null;
            break;
            case "entero":
              this.camposdinamicos[i].valor = this.camposdinamicos[i].valor != null ? Number(this.camposdinamicos[i].valor) : null;
            break;
            case "decimal":
              this.camposdinamicos[i].valor = this.camposdinamicos[i].valor != null ? Number(this.camposdinamicos[i].valor) : null;
            break;
            case "booleano":
              this.camposdinamicos[i].valor = this.camposdinamicos[i].valor == 'true' ? true : false;
            break;
          }
        }
      }
    })
  }

  cambioDuracion(dimension){
    this.actividad.fechaFin = this.sumarDias(this.actividad.fechaInicio, this.actividad.duracion, dimension.sigla);
  }

  modelChangedFechaInicio(event, dimension){
    this.actividad.fechaFin = this.sumarDias(event._d, this.actividad.duracion, dimension.sigla);
  }

  sumarDias(fecha, dias, dimension){
    if(dimension != undefined && dias != undefined && fecha != ""){
      var cnt = 0;
      var tmpDate = moment(fecha,'DD/MM/YYYY');
        while (cnt < (dias -1 )) {
          if(dimension=='d'){
            tmpDate = tmpDate.add(1,'days');	
          }
            if (tmpDate.weekday() != moment().day("Sunday").weekday() && tmpDate.weekday() != moment().day("Saturday").weekday()) {
                cnt = cnt + 1;
            }
        }
        tmpDate = moment(tmpDate,'DD/MM/YYYY');
        return tmpDate.format('DD/MM/YYYY');
    }
  }

  validarAsignado(){
    if(this.actividad.costo != null){
      if(this.actividad.programa != null){
        if(this.actividad.costo <= this.asignado)
          this.sobrepaso = false;
        else
          this.sobrepaso = true;
      }
    }
  }

  getAsignado() {
    var params = {
      id: this.actividad.id,
      programa: this.actividad.programa,
      subprograma: this.actividad.subprograma,
      proyecto: this.actividad.proyecto,
      actividad: this.actividad.actividad,
      obra: this.actividad.obra,
      renglon: this.actividad.renglon,
      geografico: this.actividad.ubicacionGeografica,
      t: new Date().getDate()
    }
    this.http.post('http://localhost:60001/api/Actividad/ValidacionAsignado', params, { withCredentials: true }).subscribe(response =>{
      if(response['success']==true){
        this.asignado = response['asignado'];
        this.sobrepaso = response['sobrepaso'];
      }
    })
  }

  obtenerAcumulacionCosto(){
    this.http.get('http://localhost:60004/api/AcumulacionCosto/AcumulacionesCosto', { withCredentials: true}).subscribe(response => {
      if (response['success'] == true) {
        this.acumulacion_costo = response["acumulacionesTipos"];        
      }
    })
  }

  settingsArchivosAdjuntos = {
    columns: {
      id: {
        title: 'ID',
        width: '5%',
        filter: false,
        type: 'html',
        valuePrepareFunction : (cell) => {
          return "<div class=\"datos-numericos\">" + cell + "</div>";
        }
      },
      nombre: {
        title: 'Nombre',
        width: '42.5%',
        filter: false
      },
      extension: {
        title: 'Extensión',
        width: '42.5%',
        filter: false
      },
      descargar: {
        title: 'Descargar',
        sort: false,
        type: 'custom',
        renderComponent: ButtonDownloadComponent,
        onComponentInitFunction: (instance) =>{
          instance.actionEmitter.subscribe(row => {
            window.location.href='http://localhost:60021/api/DocumentoAdjunto/Descarga/' + row.id;
          });
        }
      },
      eliminar:{
        title: 'Eliminar',
        sort: false,
        type: 'custom',
        renderComponent: ButtonDeleteComponent,
        onComponentInitFunction: (instance) =>{
          instance.actionEmitter.subscribe(row => {
            this.http.delete('http://localhost:60021/api/DocumentoAdjunto/Documento/' + row.id, { withCredentials: true }).subscribe(response => {
              if (response['success'] == true){
                this.sourceArchivosAdjuntos.remove(row);
              }
              else{
                this.utils.mensaje("danger", "Error al borrar el documento");
              } 
            })
            
          });
        }
      }
    },
    actions: false,
    attr: {
      class: 'table table-bordered grid estilo-letra'
    },
    hideSubHeader: true,
    noDataMessage: ''
  };

  adjuntarDocumentos(){
    this.modalAdjuntarDocumento.dialog.open(DialogDownloadDocument, {
      width: '600px',
      height: '200px',
      data: { titulo: 'Documentos Adjuntos', idObjeto: this.actividad.id, idTipoObjeto: 5 }
    }).afterClosed().subscribe(result => {
      if(result != null){
        this.sourceArchivosAdjuntos = new LocalDataSource(result);
      }
    });
  }

  getDocumentosAdjuntos = function(objetoId, tipoObjetoId){
    var formatData = {
      idObjeto: objetoId,
      idTipoObjeto: tipoObjetoId,
      t: new Date().getTime()
    }
    
    this.http.post('http://localhost:60021/api/DocumentoAdjunto/Documentos', formatData, { withCredentials: true }).subscribe(response => {
      if (response['success'] == true) {
        this.sourceArchivosAdjuntos = new LocalDataSource(response['documentos']);
      }
    })
  }

  obtenerAsignacionesRaci(){
    this.http.get('http://localhost:60039/api/MatrizRaci/AsignacionPorObjeto/'+ this.actividad.id + '/'+ 5, { withCredentials: true }).subscribe(response => {
      if(response['success'] == true){
        let asignaciones = response['asignaciones'];
        for(let i = 0; i<asignaciones.length; i++){
          let responsable = [];
					responsable['id'] = asignaciones[i].colaboradorId;
					responsable['nombre'] = asignaciones[i].colaboradorNombre;
					responsable['rol'] = this.obtenerNombreRol(asignaciones[i].rolId);
					responsable['r'] = asignaciones[i].rolId;
				  this.responsables.push(responsable);
        }
      }

      this.sourceResponsables = new LocalDataSource(this.responsables);
    })
  }

  obtenerNombreRol(valor: string){
    switch (valor){
      case 'r': return "Responsable";
      case 'a': return "Cuentadante";
      case 'c': return "Consultor";
      case 'i': return "Quien informa";
    }
    return "";
  }

  buscarActividadResponsable(){
    let idRoles = [];
    if(this.sourceResponsables['data']!= undefined && this.sourceResponsables['data'].length > 0){
      for (let x in this.sourceResponsables['data']){
        idRoles.push(this.sourceResponsables['data'][x].r);
      }
    }

    this.modalResponsalbes.dialog.open(DialogResponsables, {
      width: '600px',
      height: '600px',
      data: { titulo: 'Responsables', idRoles: idRoles }
    }).afterClosed().subscribe(result => {
      if(result != null){
        this.sourceResponsables['data'].push({ id: result.id, nombre: result.nombre, rol : this.obtenerNombreRol(result.rol), r: result.rol });

        this.sourceResponsables.setSort([
          { field: 'id', direction: 'asc' }  // primary sort
        ]);
      }
    });
  }

  settingsResponsables = {
    columns: {
      id: {
        title: 'ID',
        width: '5%',
        filter: false,
        type: 'html',
        valuePrepareFunction : (cell) => {
          return "<div class=\"datos-numericos\">" + cell + "</div>";
        }
      },
      nombre: {
        title: 'Nombre',
        width: '42.5%',
        filter: false
      },
      rol: {
        title: 'Rol',
        width: '42.5%',
        filter: false
      },
      eliminar:{
        title: 'Eliminar',
        sort: false,
        type: 'custom',
        renderComponent: ButtonDeleteComponent,
        onComponentInitFunction: (instance) =>{
          instance.actionEmitter.subscribe(row => {
              this.sourceResponsables.remove(row);
          });
        }
      }
    },
    actions: false,
    attr: {
      class: 'table table-bordered grid estilo-letra'
    },
    hideSubHeader: true,
    noDataMessage: ''
  }
}
