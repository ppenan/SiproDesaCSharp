import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { ButtonDeleteComponent } from '../../../assets/customs/ButtonDeleteComponent';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from '../../auth.service';
import { UtilsService } from '../../utils.service';
import { HttpClient } from '@angular/common/http';
import { MatDialog } from '@angular/material';
import { LocalDataSource } from 'ng2-smart-table';
import * as moment from 'moment';
import { ActividadTipo } from './model/ActividadTipo';
import { DialogActividadPropiedad, DialogOverviewActividadPropiedad } from '../../../assets/modals/actividadpropiedad/modal-actividad-propiedad';
import { DialogDelete, DialogOverviewDelete } from '../../../assets/modals/deleteconfirmation/confirmation-delete';
// import { Actividad } from '../actividad/model/Actividad';

@Component({
  selector: 'app-actividadtipo',
  templateUrl: './actividadtipo.component.html',
  styleUrls: ['./actividadtipo.component.css']
})

export class ActividadtipoComponent implements OnInit {

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
  sourcePropiedades: LocalDataSource;
  source: LocalDataSource;
  totalActividadTipos: number;
  busquedaGlobal: string;
  actividadTipo: ActividadTipo;
  paginaActual: number;
  esNuevo: boolean;
  modalActividadPropiedad: DialogOverviewActividadPropiedad;
  propiedades = [];
  modalDelete: DialogOverviewDelete;

  @ViewChild('search') divSearch: ElementRef;

  settings = {
    columns: {
      id: {
        title: 'ID',
        width: '6%',
        filter: false,
        type: 'html',
        valuePrepareFunction : (cell) => {
          return '<div class="datos-numericos">' + cell + '</div>';
        }
      },
      nombre: {
        title: 'Nombre',
        filter: false,
      },
      descripcion: {
        title: 'Descripción',
        filter: false,
      },
      usuarioCreo: {
        title: 'Usuario Creación',
        filter: false
      },
      fechaCreacion: {
        title: 'Fecha Creación',
        type: 'html',
        filter: false,
        valuePrepareFunction : (cell) => {
          return '<div class="datos-numericos">' + moment(cell, 'DD/MM/YYYY HH:mm:ss').format('DD/MM/YYYY HH:mm:ss') + '</div>';
        }
      }
    },
    actions: false,
    noDataMessage: 'No se obtuvo información...',
    attr: {
      class: 'table table-bordered grid'
    },
    hideSubHeader: true
  };

  settingsPropiedades = {
    columns: {
      id: {
        title: 'ID',
        width: '5%',
        filter: false,
        type: 'html',
        valuePrepareFunction : (cell) => {
          return '<div class="datos-numericos">' + cell + '</div>';
        }
      },
      nombre: {
        title: 'Nombre',
        align: 'left',
        width: '28%',
        filter: false,
        class: 'align-left'
      },
      descripcion: {
        title: 'Descripción',
        align: 'left',
        width: '28%',
        filter: false,
        class: 'align-left'
      },
      datoTiponombre: {
        title: 'Tipo Dato',
        align: 'left',
        width: '28%',
        filter: false,
        class: 'align-left'
      },
      eliminar: {
        title: 'Eliminar',
        sort: false,
        type: 'custom',
        width: '10%',
        renderComponent: ButtonDeleteComponent,
        onComponentInitFunction: (instance) => {
          instance.actionEmitter.subscribe(row => {
            this.sourcePropiedades.remove(row);
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

  constructor(
    private route: ActivatedRoute,
    private auth: AuthService,
    private utils: UtilsService,
    private http: HttpClient,
    private dialog: MatDialog) {

      this.sourcePropiedades = new LocalDataSource();
      this.isMasterPage = this.auth.isLoggedIn();
      this.utils.setIsMasterPage(this.isMasterPage);
      this.elementosPorPagina = utils._elementosPorPagina;
      this.numeroMaximoPaginas = utils._numeroMaximoPaginas;
      this.totalActividadTipos = 0;
      this.busquedaGlobal = null;
      this.actividadTipo = new ActividadTipo();
      this.modalActividadPropiedad = new DialogOverviewActividadPropiedad(dialog);
      this.modalDelete = new DialogOverviewDelete(dialog);

    }

  ngOnInit() {
    this.mostrarcargando = true;
    this.obtenerTotalActividadTipos();
  }

  obtenerTotalActividadTipos() {
    const data = {
      filtro_busqueda: this.busquedaGlobal,
      t: new Date().getTime()
    };

    this.http
    .post(
      'http://localhost:60003/api/ActividadTipo/NumeroActividadTipos',
      data,
      { withCredentials: true })
    .subscribe(
        response => {
          if (response['success'] ===  true) {
            this.totalActividadTipos = response['totalactividadtipos'];
            this.paginaActual = 1;

            if (this.totalActividadTipos > 0) {
              this.cargarTabla(this.paginaActual);
            } else {
              this.source = new LocalDataSource();
              this.mostrarcargando = false;
            }
          }
        }
      );
  }

  cargarTabla(pagina?: number) {
      this.mostrarcargando = true;

      const filtro = {
        pagina: pagina,
        numero_actividades_tipo: this.elementosPorPagina,
        filtro_busqueda: this.busquedaGlobal,
        columna_ordenada: null,
        ordenDireccion: null,
        t: moment().unix()
      };

      this.http
      .post('http://localhost:60003/api/ActividadTipo/ActividadTiposPagina',
        filtro,
        { withCredentials: true})
      .subscribe(
        response => {
          if (response['success'] === true) {
            const data = response['actividadtipos'];

            this.source = new LocalDataSource(data);
            this.source.setSort([
              { field: 'id', direction: 'asc'}
            ]);

            this.busquedaGlobal = null;
          }
        }
      )
      ;
  }

  nuevo () {
    this.esColapsado = true;
    this.esNuevo = true;
    this.sourcePropiedades = new LocalDataSource();
  }

  editar () {
    if (this.actividadTipo.id > 0) {
      this.esColapsado = true;
      this.esNuevo = false;

      this.http.get(
        'http://localhost:60002/api/ActividadPropiedad/ActividadPropiedadPorTipo/' + this.actividadTipo.id,
        { withCredentials : true })
        .subscribe(response => {
          if (response['success'] === true) {
            this.propiedades = response['actividadpropiedades'];
            this.sourcePropiedades = new LocalDataSource(this.propiedades);
          }
        });
    } else {
      this.utils.mensaje('warning', 'Debe seleccionar un Tipo de actividad que desea editar');
    }
  }

  borrar() {

    if (this.actividadTipo.id > 0) {

      this.modalDelete.dialog.open(DialogDelete, {
        width: '600px',
        height: '200px',
        data: {
          id: this.actividadTipo.id,
          titulo: 'Confirmación de Borrado',
          textoCuerpo: '¿Desea borrar el tipo de actividad?',
          textoBotonOk: 'Borrar',
          textoBotonCancelar: 'Cancelar'
        }
      }).afterClosed().subscribe(result => {
        if (result === true) {
          this.http.delete(
            'http://localhost:60003/api/ActividadTipo/ActividadTipo/' + this.actividadTipo.id,
            { withCredentials : true })
          .subscribe(response => {
            if (response['success'] === true) {
              this.obtenerTotalActividadTipos();
            }
          });
        }
      });
    } else {
      this.utils.mensaje('warning', 'Selecione el tipo de actividad que desea borrar');
    }
  }

  filtrar(campo) {
    this.busquedaGlobal = campo;
    this.obtenerTotalActividadTipos();
  }

  onDblClickRow(event) {
    this.actividadTipo = event.data;
    this.editar();
  }

  onSelectRow(event) {
    this.actividadTipo = event.data;
  }

  handlePage(event) {
    this.cargarTabla(event.pageIndex + 1);
  }

  guardar() {

    if (this.actividadTipo != null) {

      this.actividadTipo.propiedades = '';
      this.propiedades = [];

      this.sourcePropiedades.getAll().then(value => {
        value.forEach(element => {
          this.propiedades.push(element);
        });

        for (let x = 0; x < this.propiedades.length; x++) {
          this.actividadTipo.propiedades = this.actividadTipo.propiedades + (x > 0 ? ',' : '') + this.propiedades[x].id;
        }

        let objetoHttp;

        if (this.actividadTipo.id > 0) {

          objetoHttp = this.http.put(
            'http://localhost:60003/api/ActividadTipo/ActividadTipo/' + this.actividadTipo.id,
            this.actividadTipo,
            { withCredentials: true }
            );

        } else {

          objetoHttp = this.http.post(
            'http://localhost:60003/api/ActividadTipo/ActividadTipo',
            this.actividadTipo,
            { withCredentials: true });
        }

        objetoHttp.subscribe(response => {

          if (response['success'] === true) {

            this.actividadTipo.usuarioCreo = response['usuarioCreo'];
            this.actividadTipo.fechaCreacion = response['fechaCreacion'];
            this.actividadTipo.fechaActualizacion = response['fechaActualizacion'];
            this.actividadTipo.usuarioActualizo = response['usuarioActualizo'];
            this.actividadTipo.id = response['id'];

            this.esNuevo = false;
            this.obtenerTotalActividadTipos();
            this.utils.mensaje('success', 'Tipo de componente guardado con éxito');
          }
        });
      });
    } else {
      this.utils.mensaje('warning', 'Debe seleccionar un Tipo de dato');
    }
  }

  IrATabla() {
    this.esColapsado = false;
    this.actividadTipo = new ActividadTipo();
    this.sourcePropiedades = new LocalDataSource();
  }

  buscarPropiedades() {
    this.modalActividadPropiedad.dialog.open(
      DialogActividadPropiedad, {
        width: '600px',
        height: '585px',
        data: { titulo: 'Propiedades' }
      })
      .afterClosed()
      .subscribe(result => {
        if (result != null) {
          let tablaPropiedades = [];

          this.sourcePropiedades.getAll().then(value => {
            value.forEach(element => {
              tablaPropiedades.push(element);
            });

            let existe = false;
            if (tablaPropiedades.length === 0) {
              tablaPropiedades.push({ id: result.id, nombre: result.nombre, datoTiponombre : result.datoTiponombre });
            } else {
              tablaPropiedades.forEach(element => {
                if (element.id == result.id) {
                  existe = true;
                }
              });

              if (!existe) {
                tablaPropiedades.push({ id: result.id, nombre: result.nombre, datoTiponombre : result.datoTiponombre });
              }
            }
          });

          this.sourcePropiedades = new LocalDataSource(tablaPropiedades);
          this.sourcePropiedades.setSort([
            { field: 'id', direction: 'asc' }  // primary sort
          ]);
        }
    });
  }
}
