import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import * as moment from 'moment';
import { LocalDataSource } from 'ng2-smart-table';
import { Subproducto } from './model/Subproducto';
import { ActivatedRoute } from "@angular/router";
import { AuthService } from '../../auth.service';
import { UtilsService } from '../../utils.service';
import { HttpClient } from '@angular/common/http';
import { MatDialog } from '@angular/material';
import { Router } from '@angular/router';
import { DialogMapa, DialogOverviewMapa } from '../../../assets/modals/cargamapa/modal-carga-mapa';
import { FormControl } from '@angular/forms';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { DialogOverviewUnidadEjecutora, DialogUnidadEjecutora } from '../../../assets/modals/unidadejecutora/unidad-ejecutora';
import { DialogSubProductoTipo, DialogOverviewSubProductoTipo } from '../../../assets/modals/subproductotipo/modal-subproducto-tipo';
import { DialogDelete, DialogOverviewDelete } from '../../../assets/modals/deleteconfirmation/confirmation-delete';

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
  selector: 'app-subproducto',
  templateUrl: './subproducto.component.html',
  styleUrls: ['./subproducto.component.css']
})

export class SubproductoComponent implements OnInit {
  mostrarcargando : boolean;
  color = 'primary';
  mode = 'indeterminate';
  value = 50;
  diameter = 45;
  strokewidth = 3;

  isLoggedIn : boolean;
  isMasterPage : boolean;
  esTreeview: boolean;
  esColapsado: boolean;
  congelado: number;  
  source : LocalDataSource;
  totalSubProductos: number;
  elementosPorPagina: number;
  numeroMaximoPaginas: number;
  subproducto: Subproducto;
  productoid: number;
  busquedaGlobal : string;
  paginaActual : number;
  tabActive: number;
  prestamoid: number;
  unidadEjecutoraNombre: string;
  unidadEjecutora: number;
  entidad: number;
  entidadNombre: string;
  ejercicio: number;
  esNuevo: boolean;
  modalMapa: DialogOverviewMapa;
  coordenadas: string;
  bloquearCosto: boolean;
  asignado: number;
  sobrepaso: boolean;
  acumulacion_costo = [];
  dimensionSelected: number;
  modalUnidadEjecutora: DialogOverviewUnidadEjecutora;
  modalSubProductoTipo: DialogOverviewSubProductoTipo;
  unidadejecutoraid: number;
  unidadejecutoranombre: string;
  entidadnombre: string;
  mostraringreso: boolean;
  camposdinamicos = [];
  botones: boolean;
  productoNombre: string;
  objetoTipoNombre: string;
  modalDelete: DialogOverviewDelete;

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
    this.totalSubProductos = 0;

    this.route.params.subscribe(param => {
      this.productoid = Number(param.id);
    })

    this.busquedaGlobal = null;
    this.tabActive = 0;
    this.congelado = 0;
    this.obtenerProducto();
    this.subproducto = new Subproducto();
    this.modalMapa = new DialogOverviewMapa(dialog);

    this.filteredAcumulacionCosto = this.myControl.valueChanges
      .pipe(
        startWith(''),
        map(value => value ? this._filterAcumulacionCosto(value) : this.acumulacion_costo.slice())
      );

    this.modalUnidadEjecutora = new DialogOverviewUnidadEjecutora(dialog);
    this.modalSubProductoTipo = new DialogOverviewSubProductoTipo(dialog);
    this.unidadejecutoranombre = "";
    this.modalDelete = new DialogOverviewDelete(dialog);
   }

   private _filterAcumulacionCosto(value: string): AcumulacionCosto[] {
    const filterValue = value.toLowerCase();
    return this.acumulacion_costo.filter(c => c.nombre.toLowerCase().indexOf(filterValue) === 0);
  }

  acumulacionCostoSeleccionado(value){
    this.subproducto.acumulacionCostoid = value.id;
  }

  ngOnInit() {
    this.mostrarcargando=true;
    this.obtenerTotalSubproductos();
    this.obtenerAcumulacionCosto();
  }

  obtenerProducto(){
    this.http.get('http://localhost:60058/api/Producto/ProductoPorId/' + this.productoid, { withCredentials: true }).subscribe(response => {
      if (response['success'] == true) {
        this.productoNombre = response['nombre'];
        this.objetoTipoNombre = 'Producto';
        this.congelado = response['congelado'];  
        this.prestamoid = response['prestamoId'];
        this.unidadEjecutoraNombre = response['unidadEjecutoraNombre'];
        this.unidadEjecutora = response['unidadEjecutora'];
        this.entidad = response['entidad'];
        this.entidadNombre = response['entidadNombre'];
        this.ejercicio = response['ejercicio'];
      }
    })
  }

  obtenerTotalSubproductos(){
    var data = {  
      filtro_busqueda: this.busquedaGlobal,
      productoid: this.productoid,
      t: new Date().getTime()      
    };
    this.http.post('http://localhost:60083/api/Subproducto/NumeroSubProductosPorProducto', data, { withCredentials : true}).subscribe(response =>{
      if(response['success'] == true){
        this.totalSubProductos = response['totalsubproductos'];
        this.paginaActual = 1;
        if(this.totalSubProductos > 0){
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
      productoid: this.productoid,
      pagina: pagina,
      numerosubproductos: this.elementosPorPagina,
      filtro_busqueda: this.busquedaGlobal,
      columna_ordenada: null,
      t:moment().unix()
    };

    this.http.post('http://localhost:60083/api/Subproducto/SubProductosPaginaPorProducto', filtro, { withCredentials : true}).subscribe(
      response =>{
        if(response['success'] == true){
          var data = response['subproductos'];

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
    this.coordenadas = null;
    this.camposdinamicos = [];
    this.unidadejecutoraid= this.prestamoid != null ? this.unidadEjecutora :  null;
    this.unidadejecutoranombre= this.prestamoid != null ? this.unidadEjecutoraNombre : null;
    this.subproducto.duracionDimension = this.dimensiones[this.dimensionSelected-1].sigla;
  } 

  editar(){
    if(this.subproducto.id != null){
      this.esColapsado = true;
      this.esNuevo = false;
      this.tabActive = 0;
      this.dimensionSelected = 0;

      this.unidadejecutoraid= this.subproducto.ueunidadEjecutora; 
      this.unidadejecutoranombre= this.subproducto.nombreUnidadEjecutora;
      this.ejercicio = this.subproducto.ejercicio;
      this.entidad = this.subproducto.entidadentidad;
      this.entidadnombre = this.subproducto.entidadnombre;

      if(this.subproducto.acumulacionCostoid==2)
        this.bloquearCosto = true;
      else
        this.bloquearCosto = false;

      this.mostraringreso = true;
      this.esNuevo = false;

      this.coordenadas = (this.subproducto.latitud !=null ?  this.subproducto.latitud : '') +
        (this.subproducto.latitud!=null ? ', ' : '') + (this.subproducto.longitud!=null ? this.subproducto.longitud : '');
        
      this.obtenerCamposDinamicos();

      this.getAsignado();      
    }
    else
      this.utils.mensaje("warning", "Debe de seleccionar el subproducto que desea editar");
  }

  borrar(){
    if(this.subproducto.id > 0){
      this.modalDelete.dialog.open(DialogDelete, {
        width: '600px',
        height: '200px',
        data: { 
          id: this.subproducto.id,
          titulo: 'Confirmación de Borrado', 
          textoCuerpo: '¿Desea borrar el subproducto?',
          textoBotonOk: 'Borrar',
          textoBotonCancelar: 'Cancelar'
        }
      }).afterClosed().subscribe(result => {
        if(result != null){
          if(result){
            this.http.delete('http://localhost:60083/api/Subproducto/SubProducto/'+ this.subproducto.id, { withCredentials : true }).subscribe(response =>{
              if(response['success'] == true){
                this.obtenerTotalSubproductos();
                this.utils.mensaje('success', 'Subproducto borrado exitosamente');
              }
            })
          } 
        }
      })
    }
    else{
      this.utils.mensaje('warning', 'Seleccione una propiedad de subproducto');
    }
  }

  filtrar(campo){
    this.busquedaGlobal = campo;
    this.obtenerTotalSubproductos();
  }

  onSelectRow(event) {
    this.subproducto = event.data;
  }

  onDblClickRow(event) {
    this.subproducto = event.data;
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
    hideSubHeader: true
  };

  guardar(){
    if(this.subproducto != null){
      for(var i=0; i < this.camposdinamicos.length; i++){
        this.botones = false;
        if(this.camposdinamicos[i].tipo === 'fecha'){
          this.camposdinamicos[i].valor_f = this.camposdinamicos[i].valor != null ? moment(this.camposdinamicos[i].valor).format('DD/MM/YYYY') : "";        
        }
      }

      this.subproducto.inversionNueva = this.subproducto.inversionNueva == 1 ? 1 : this.subproducto.inversionNueva.toString() == "true" ? 1 : 0;
      this.subproducto.camposDinamicos = JSON.stringify(this.camposdinamicos);
      this.subproducto.productoid = this.productoid;

      var objetoHttp;

      if(this.subproducto.id > 0){
        objetoHttp = this.http.put("http://localhost:60083/api/Subproducto/SubProducto/" + this.subproducto.id, this.subproducto, { withCredentials: true });
      }
      else{        
        this.subproducto.id=0;
        this.subproducto.ejercicio = this.ejercicio;
        this.subproducto.entidadentidad = this.entidad;
        this.subproducto.ueunidadEjecutora = this.unidadEjecutora;
        objetoHttp = this.http.post("http://localhost:60083/api/Subproducto/SubProducto", this.subproducto, { withCredentials: true });
      }

      objetoHttp.subscribe(response => {
        if(response['success'] == true){
          this.subproducto.id = response['id'];
          this.subproducto.usuarioCreo = response['usuarioCreo'];
          this.subproducto.fechaCreacion = response['fechaCreacion'];
          this.subproducto.usuarioActualizo = response['usuarioActualizo'];
          this.subproducto.fechaActualizacion = response['fechaActualizacion'];

          if(!this.esTreeview)
							this.obtenerTotalSubproductos();
						/*else{
							//if(!this.esNuevo)
								//mi.t_cambiarNombreNodo();
							//else
								//mi.t_crearNodo(mi.componente.id,mi.componente.nombre,1,true);
						}
						if(this.child_riesgos!=null){
							//ret = mi.child_riesgos.guardar('Componente '+(mi.esNuevo ? 'creado' : 'guardado')+' con éxito',
							//		'Error al '+(mi.esNuevo ? 'creado' : 'guardado')+' el Componente');
						}
						else
              $utilidades.mensaje('success','Componente '+(this.esNuevo ? 'creado' : 'guardado')+' con éxito');*/
            this.utils.mensaje('success', 'Subproducto '+(this.esNuevo ? 'creado' : 'guardado')+' con éxito');
            this.esNuevo = false;
        }
        else
          this.utils.mensaje('warning', 'No se pudo guardar el Subproducto.');
      },
      error => {
          this.utils.mensaje('danger', 'Ocurrió un error al guardar el Subproducto');
      }
    )
    }
  }

  IrATabla(){
    this.esColapsado = false;
    this.subproducto = new Subproducto();
  }

  buscarUnidadEjecutora(){
    if(this.prestamoid == null){
      this.modalUnidadEjecutora.dialog.open(DialogUnidadEjecutora, {
        width: '600px',
        height: '585px',
        data: { titulo: 'Unidades Ejecutoras', ejercicio: this.subproducto.ejercicio, entidad: this.subproducto.entidadentidad }
      }).afterClosed().subscribe(result => {
        if(result != null){
          this.unidadejecutoraid = result.id;
          this.unidadejecutoranombre = result.nombre;
        }
      })
    }
  }

  buscarSubproductoTipo(){
    this.modalSubProductoTipo.dialog.open(DialogSubProductoTipo, {
      width: '600px',
      height: '585px',
      data: { titulo: 'Subproducto Tipo' }
    }).afterClosed().subscribe(result => {
      if(result != null){
        this.subproducto.subproductoTipoid = result.id;
        this.subproducto.subProductoTipo = result.nombre;

        this.obtenerCamposDinamicos();
      }
    })
  }

  abrirMapa(){
    this.modalMapa.dialog.open(DialogMapa, {
      width: '1000px',
      height: '500px',
      data: { titulo: 'Mapa' }
    }).afterClosed().subscribe(result=>{
      if(result != null && result.success){
        this.subproducto.latitud = result.latitud;
        this.subproducto.longitud = result.longitud;
        this.coordenadas = result.latitud + ", " + result.longitud;
      }else{
        this.coordenadas = '';
      }
    })
  }

  validarAsignado(){
    if(this.subproducto.costo != null){
      if(this.subproducto.programa != null){
        if(this.subproducto.costo <= this.asignado)
          this.sobrepaso = false;
        else
          this.sobrepaso = true;
      }
    }
  }

  getAsignado() {
    var params = {
      id: this.subproducto.id,
      programa: this.subproducto.programa,
      subprograma: this.subproducto.subprograma,
      proyecto: this.subproducto.proyecto,
      actividad: this.subproducto.actividad,
      obra: this.subproducto.obra,
      renglon: this.subproducto.renglon,
      geografico: this.subproducto.ubicacionGeografica,
      t: new Date().getDate()
    }
    this.http.post('http://localhost:60083/api/Subproducto/ValidacionAsignado', params, { withCredentials: true }).subscribe(response =>{
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

  cambioDuracion(dimension){
    this.subproducto.fechaFin = this.sumarDias(this.subproducto.fechaInicio, this.subproducto.duracion, dimension.sigla);
  }

  modelChangedFechaInicio(event, dimension){
    this.subproducto.fechaFin = this.sumarDias(event._d, this.subproducto.duracion, dimension.sigla);
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

  obtenerCamposDinamicos(){
    var parametros ={
      idProyecto : this.subproducto.id,
      idProductoTipo : this.subproducto.subproductoTipoid,
      t: new Date().getTime() 
    }
    this.http.get('http://localhost:60084/api/SubproductoPropiedad/SubProductoPropiedadPorTipo/'+this.subproducto.id+'/'+this.subproducto.subproductoTipoid,  { withCredentials: true }).subscribe(response => {
      if (response['success'] == true) {
        this.camposdinamicos = response['subproductopropiedades'];
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


  irAActividades(subproductoId){
    if(this.subproducto!=null){
      this.router.navigateByUrl('/main/actividad/'+ subproductoId);
    }
  }
}
