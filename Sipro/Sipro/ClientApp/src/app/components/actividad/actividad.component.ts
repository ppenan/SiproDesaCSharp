import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import * as moment from 'moment';
import { LocalDataSource } from 'ng2-smart-table';
import { Actividad } from './model/Actividad';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from '../../auth.service';
import { UtilsService } from '../../utils.service';
import { HttpClient } from '@angular/common/http';
import { MatDialog } from '@angular/material';
import { Router } from '@angular/router';
import { DialogMapa, DialogOverviewMapa } from '../../../assets/modals/cargamapa/modal-carga-mapa';
import { FormControl } from '@angular/forms';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { DialogActividadTipo, DialogOverviewActividadTipo } from '../../../assets/modals/actividadtipo/actividad-tipo';
import { DialogOverviewUnidadEjecutora, DialogUnidadEjecutora } from '../../../assets/modals/unidadejecutora/unidad-ejecutora';
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
  selector: 'app-actividad',
  templateUrl: './actividad.component.html',
  styleUrls: ['./actividad.component.css']
})

export class ActividadComponent implements OnInit {

  mostrarcargando: boolean;
  color = 'primary';
  mode = 'indeterminate';
  value = 50;
  diameter = 45;
  strokewidth = 3;

  isLoggedIn: boolean;
  isMasterPage: boolean;
  objetoTipoNombre: string;
  proyectoNombre: string;
  esTreeview: boolean;
  esColapsado: boolean;
  congelado: number;
  source: LocalDataSource;
  totalActividades: number;
  elementosPorPagina: number;
  numeroMaximoPaginas: number;
  actividad: Actividad;
  pepid: number;
  busquedaGlobal: string;
  paginaActual: number;
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
  modalActividadTipo: DialogOverviewActividadTipo;
  unidadejecutoraid: number;
  unidadejecutoranombre: string;
  entidadnombre: string;
  mostraringreso: boolean;
  camposdinamicos = [];
  modalUnidadEjecutora: DialogOverviewUnidadEjecutora;
  botones: boolean;
  modalDelete: DialogOverviewDelete;
  objeto_id: number;
  objeto_tipo: number;

  dimensiones = [
    {value: 1, nombre: 'Dias', sigla: 'd'}
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
      this.objeto_id = Number(param.objeto_id);
      this.objeto_tipo = Number(param.objeto_tipo);
    });

    this.busquedaGlobal = null;
    this.tabActive = 0;
    this.congelado = 0;
    this.actividad = new Actividad();
    this.modalMapa = new DialogOverviewMapa(dialog);

    this.filteredAcumulacionCosto = this.myControl.valueChanges
      .pipe(
        startWith(''),
        map(value => value ? this._filterAcumulacionCosto(value) : this.acumulacion_costo.slice())
      );

    this.modalActividadTipo = new DialogOverviewActividadTipo(dialog);
    this.modalUnidadEjecutora = new DialogOverviewUnidadEjecutora(dialog);
    this.modalDelete = new DialogOverviewDelete(dialog);
  }

  private _filterAcumulacionCosto(value: string): AcumulacionCosto[] {
    const filterValue = value.toLowerCase();
    return this.acumulacion_costo.filter(c => c.nombre.toLowerCase().indexOf(filterValue) === 0);
  }

  acumulacionCostoSeleccionado(value){
    this.actividad.acumulacionCostoId = value.id;
  }

  ngOnInit() {
    this.mostrarcargando = true;
    this.obtenerTotalActividades();
    this.obtenerAcumulacionCosto();
  }

  obtenerAcumulacionCosto(){
    this.http.get('http://localhost:60004/api/AcumulacionCosto/AcumulacionesCosto', { withCredentials: true}).subscribe(response => {
      if (response['success'] == true) {
        this.acumulacion_costo = response["acumulacionesTipos"];        
      }
    })
  }

  obtenerTotalActividades() {
    const data = {
      filtro_busqueda: this.busquedaGlobal,
      objetoid: this.objeto_id, 
      tipo: this.objeto_tipo,
      t: new Date().getTime(),
    };

    this.http.post(
      'http://localhost:60001/api/Actividad/NumeroActividadesPorObjeto',
      data,
      {
        withCredentials: true
      })
      .subscribe(
        response => {
          if (response['success'] === true) {
            this.totalActividades = response['totalActividades'];
            this.paginaActual = 1;

            if (this.totalActividades > 0) {
              this.cargarTabla(this.paginaActual);
            } else {
              this.source = new LocalDataSource();
              this.mostrarcargando=false;
            }
          }
        });
  }

  cargarTabla(pagina?: number){
    this.mostrarcargando = true;
    const filtro = {
      objetoId: this.objeto_id,
      tipo: this.objeto_tipo,
      pagina: pagina,
      numeroActividades: this.elementosPorPagina,
      filtro_busqueda: this.busquedaGlobal,
      columna_ordenada: null,
      t: moment().unix()
    };

    this.http.post('http://localhost:60001/api/Actividad/ActividadesPaginaPorObjeto', filtro, { withCredentials : true}).subscribe(response => {
      if (response['success'] === true) {
        let data = response['actividades'];

        for(let i = 0; i < data.length; i++) {
          data[i].fechaInicio = data[i].fechaInicio != null ? moment(data[i].fechaInicio, 'DD/MM/YYYY').toDate() : null;
          data[i].fechaFin = data[i].fechaFin != null ? moment(data[i].fechaFin, 'DD/MM/YYYY').format('DD/MM/YYYY') : null;
        }

        this.source = new LocalDataSource(data);
        this.source.setSort([
            { field: 'id', direction: 'asc' }  // primary sort
        ]);
        this.busquedaGlobal = null;
      }
      this.mostrarcargando = false;
    });
  }

  nuevo() {
    this.esColapsado = true;
    this.esNuevo = true;
    this.tabActive = 0;
    this.dimensionSelected = 1;
    this.coordenadas = null;
    this.camposdinamicos = [];
    this.unidadejecutoraid= this.prestamoid != null ? this.unidadEjecutora :  null;
    this.unidadejecutoranombre= this.prestamoid != null ? this.unidadEjecutoraNombre : null;
    this.actividad.duracionDimension = this.dimensiones[this.dimensionSelected - 1].sigla;
  }

  editar() {
    if (this.actividad.id != null) {
      this.esColapsado = true;
      this.esNuevo = false;
      this.tabActive = 0;
      this.dimensionSelected = 0;

      /*this.unidadejecutoraid = this.actividad.ueunidadEjecutora;
      this.unidadejecutoranombre = this.actividad.unidadejecutoranombre;
      this.ejercicio = this.actividad.ejercicio;
      this.entidad = this.actividad.entidad;
      this.entidadnombre = this.actividad.entidadnombre;*/

      if (this.actividad.acumulacionCostoId == 2){
        this.bloquearCosto = true;
      } else {
        this.bloquearCosto = false;
      }


      this.mostraringreso = true;
      this.esNuevo = false;

      /*this.coordenadas = (this.componente.latitud !=null ?  this.componente.latitud : '') +
        (this.componente.latitud!=null ? ', ' : '') + (this.componente.longitud!=null ? this.componente.longitud : '');
        
      this.obtenerCamposDinamicos();

      this.getAsignado();    */  
    }
    else
      this.utils.mensaje("warning", "Debe de seleccionar el componente que desea editar");
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

  IrATabla(){
    this.esColapsado = false;
    this.actividad = new Actividad();
  }
}
