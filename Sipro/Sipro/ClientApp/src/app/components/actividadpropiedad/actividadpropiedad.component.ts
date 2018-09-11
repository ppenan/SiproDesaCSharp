import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { AuthService } from '../../auth.service';
import { UtilsService } from '../../utils.service';
import { HttpClient } from '@angular/common/http';
import { LocalDataSource } from 'ng2-smart-table';
import * as moment from 'moment';
import { MatDialog } from '@angular/material';
import { ActividadPropiedad } from './model/ActividadPropiedad';
import { DialogDelete, DialogOverviewDelete } from '../../../assets/modals/deleteconfirmation/confirmation-delete';

@Component({
  selector: 'app-actividadpropiedad',
  templateUrl: './actividadpropiedad.component.html',
  styleUrls: ['./actividadpropiedad.component.css']
})
export class ActividadpropiedadComponent implements OnInit {
    mostrarcargando: boolean;
    color = 'primary';
    mode = 'indeterminate';
    value = 50;
    diameter = 45;
    strokewidth = 3;
    isLoggedIn: boolean;
    isMasterPage: boolean;
    esColapsado: boolean;
    elementosPorPagina: number;
    numeroMaximoPaginas: number;
    totalActividadPropiedades: number;
    actividadpropiedad: ActividadPropiedad;
    busquedaGlobal: string;
    paginaActual: number;
    source: LocalDataSource;
    esNuevo: boolean;
    datoTipoSelected: number;
    tipodatos = [];
    modalDelete: DialogOverviewDelete;

    @ViewChild('search') divSearch: ElementRef;

  constructor(private auth: AuthService, private utils: UtilsService, private http: HttpClient, private dialog: MatDialog) {
    this.isMasterPage = this.auth.isLoggedIn();
    this.utils.setIsMasterPage(this.isMasterPage);
    this.elementosPorPagina = utils._elementosPorPagina;
    this.numeroMaximoPaginas = utils._numeroMaximoPaginas;
    this.totalActividadPropiedades = 0;
    this.actividadpropiedad = new ActividadPropiedad();
    this.source = new LocalDataSource();
    this.modalDelete = new DialogOverviewDelete(dialog);
   }

  ngOnInit() {
    this.mostrarcargando = true;
    this.obtenerTotalActividadPropiedades();
    this.obtenerDatosTipo();
  }

  obtenerTotalActividadPropiedades(){
    var data = {
      filtro_busqueda: this.busquedaGlobal,
      t: moment().unix()
    };

    this.http.post(
      'http://localhost:60002/api/ActividadPropiedad/NumeroActividadPropiedades', data, { withCredentials : true })
      .subscribe(response => {
        if (response['success'] === true) {
          this.totalActividadPropiedades = response['totalactividadpropiedades'];
          this.paginaActual = 1;
          if (this.totalActividadPropiedades > 0) {
            this.cargarTabla(this.paginaActual);
          } else {
            this.source = new LocalDataSource();
            this.mostrarcargando = false;
          }
        } else {
          this.source = new LocalDataSource();
        }
    });
  }

  cargarTabla(pagina?: any) {
    this.mostrarcargando = true;
    var filtro = {
      pagina: pagina,
      numeroActividadPropiedades: this.elementosPorPagina,
      filtro_busqueda: this.busquedaGlobal,
      columna_ordenada: null,
      t: moment().unix()
    };

    this.http.post(
      'http://localhost:60002/api/ActividadPropiedad/ActividadPropiedadPagina',
      filtro,
      { withCredentials : true })
      .subscribe(response => {
      if (response['success'] === true) {
        const data = response['actividadpropiedades'];
        this.source = new LocalDataSource(data);
        this.source.setSort([
          { field: 'id', direction: 'asc' }  // primary sort
        ]);
        this.busquedaGlobal = null;
        this.mostrarcargando = false;
      }
    });
  }

  obtenerDatosTipo() {
    this.http.get('http://localhost:60017/api/DatoTipo/Listar', { withCredentials : true }).subscribe(response => {
      if (response['success'] === true) {
        this.tipodatos = response['datoTipos'];
      }
    });
  }

  cambioOpcionDatoTipo(opcion) {
    this.actividadpropiedad.datoTipoid = opcion;
  }

  handlePage(event){
    this.cargarTabla(event.pageIndex+1);
  }

  nuevo(){
    this.esColapsado = true;
    this.esNuevo = true;
    this.componentepropiedad = new ComponentePropiedad();
    this.datoTipoSelected = 0;
  }

  editar(){
    if(this.componentepropiedad.id > 0){
      this.esColapsado = true;
      this.esNuevo = false;
      this.datoTipoSelected = this.componentepropiedad.datoTipoid;
    }
    else{
      this.utils.mensaje('warning', 'Debe seleccionar la Propiedad de componente que desea editar');
    }
  }

  borrar(){
    if(this.componentepropiedad.id > 0){
      this.modalDelete.dialog.open(DialogDelete, {
        width: '600px',
        height: '200px',
        data: { 
          id: this.componentepropiedad.id,
          titulo: 'Confirmación de Borrado', 
          textoCuerpo: '¿Desea borrar la propiedad de componente?',
          textoBotonOk: 'Borrar',
          textoBotonCancelar: 'Cancelar'
        }
      }).afterClosed().subscribe(result => {
        if(result != null){
          if(result){
            this.http.delete('http://localhost:60013/api/ComponentePropiedad/ComponentePropiedad/'+ this.componentepropiedad.id, { withCredentials : true }).subscribe(response =>{
              if(response['success'] == true){
                this.obtenerTotalComponentePropiedades();
              }
            })
          } 
        }
      })
    }
    else{
      this.utils.mensaje('warning', 'Seleccione una propiedad de componente');
    }
  }

}
