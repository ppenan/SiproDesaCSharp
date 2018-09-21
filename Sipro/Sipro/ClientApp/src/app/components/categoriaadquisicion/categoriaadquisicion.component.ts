import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { AuthService } from '../../auth.service';
import { UtilsService } from '../../utils.service';
import { HttpClient } from '@angular/common/http';
import { LocalDataSource } from 'ng2-smart-table';
import * as moment from 'moment';
import { MatDialog } from '@angular/material';
import { CategoriaAdquisicion } from './model/CategoriaAdquisicion';
import { DialogDelete, DialogOverviewDelete } from '../../../assets/modals/deleteconfirmation/confirmation-delete';

@Component({
  selector: 'app-categoriaadquisicion',
  templateUrl: './categoriaadquisicion.component.html',
  styleUrls: ['./categoriaadquisicion.component.css']
})
export class CategoriaadquisicionComponent implements OnInit {
  mostrarcargando : boolean;
  color = 'primary';
  mode = 'indeterminate';
  value = 50;
  diameter = 45;
  strokewidth = 3;

  isLoggedIn : boolean;
  isMasterPage : boolean;
  esColapsado : boolean;
  elementosPorPagina : number;
  numeroMaximoPaginas : number;
  totalCategoriaAdquisicion : number;
  categoriaadquisicion : CategoriaAdquisicion;
  busquedaGlobal: string;
  paginaActual : number;
  source: LocalDataSource;
  esNuevo : boolean;
  modalDelete: DialogOverviewDelete;

  @ViewChild('search') divSearch: ElementRef;

  constructor(private auth: AuthService, private utils: UtilsService, private http: HttpClient, private dialog: MatDialog) { 
    this.isMasterPage = this.auth.isLoggedIn();
    this.utils.setIsMasterPage(this.isMasterPage);
    this.elementosPorPagina = utils._elementosPorPagina;
    this.numeroMaximoPaginas = utils._numeroMaximoPaginas;
    this.totalCategoriaAdquisicion = 0;
    this.categoriaadquisicion = new CategoriaAdquisicion();
    this.source = new LocalDataSource();
    this.modalDelete = new DialogOverviewDelete(dialog);
  }

  ngOnInit() {
    this.mostrarcargando = true;
    this.obtenerTotalCategoriasAdquisicion();
  }

  obtenerTotalCategoriasAdquisicion(){
    var data = {  
      filtro_busqueda: this.busquedaGlobal,
      t:moment().unix()
    };

    this.http.post('http://localhost:60010/api/CategoriaAdquisicion/NumeroCategoriaPorObjeto', data, { withCredentials : true }).subscribe(response =>{
      if(response['success'] == true){
        this.totalCategoriaAdquisicion = response['totalCategoriaAdquisiciones'];
        this.paginaActual = 1;
        if(this.totalCategoriaAdquisicion > 0)
          this.cargarTabla(this.paginaActual);
        else{
          this.source = new LocalDataSource();
          this.mostrarcargando = false;
        }
      }
      else{
        this.source = new LocalDataSource();
      }
    })
  }

  cargarTabla(pagina? : any){
    this.mostrarcargando = true;
    var filtro = {
      pagina: pagina,
      numeroCategoriaAdquisicion: this.elementosPorPagina,
      filtro_busqueda: this.busquedaGlobal,
      columna_ordenada: null,
      t:moment().unix()
    };

    this.http.post('http://localhost:60010/api/CategoriaAdquisicion/CategoriaAdquisicionPagina', filtro, { withCredentials : true }).subscribe(response =>{
      if(response['success'] == true){
        var data = response['categoriaAdquisiciones'];
        this.source = new LocalDataSource(data);
        this.source.setSort([
          { field: 'id', direction: 'asc' }  // primary sort
        ]);
        this.busquedaGlobal = null;

        this.mostrarcargando = false;
      }
    })
  }

  nuevo(){
    this.esColapsado = true;
    this.esNuevo = true;
    this.categoriaadquisicion = new CategoriaAdquisicion();
  }

  editar(){
    if(this.categoriaadquisicion.id > 0){
      this.esColapsado = true;
      this.esNuevo = false;
    }
    else{
      this.utils.mensaje('warning', 'Debe seleccionar la categoria de adquisición que desea editar');
    }
  }

  borrar(){
    if(this.categoriaadquisicion.id > 0){
      this.modalDelete.dialog.open(DialogDelete, {
        width: '600px',
        height: '200px',
        data: { 
          id: this.categoriaadquisicion.id,
          titulo: 'Confirmación de Borrado', 
          textoCuerpo: '¿Desea borrar la categoría de adquisición?',
          textoBotonOk: 'Borrar',
          textoBotonCancelar: 'Cancelar'
        }
      }).afterClosed().subscribe(result => {
        if(result != null){
          if(result){
            this.http.delete('http://localhost:60010/api/CategoriaAdquisicion/CategoriaAdquisicion/'+ this.categoriaadquisicion.id, { withCredentials : true }).subscribe(response =>{
              if(response['success'] == true){
                this.obtenerTotalCategoriasAdquisicion();
              }
            })
          } 
        }
      })
    }
    else{
      this.utils.mensaje('warning', 'Seleccione una categoria de adquisición');
    }
  }

  filtrar(campo){
    this.busquedaGlobal = campo;
    this.obtenerTotalCategoriasAdquisicion();
  }

  onDblClickRow(event){
    this.categoriaadquisicion = event.data;
    this.editar();
  }

  onSelectRow(event){
    this.categoriaadquisicion = event.data;
  }

  guardar(){
    var objetoHttp;
    if(this.categoriaadquisicion.id > 0){
      objetoHttp = this.http.put("http://localhost:60010/api/CategoriaAdquisicion/CategoriaAdquisicion/" + this.categoriaadquisicion.id, this.categoriaadquisicion, { withCredentials: true });
    }
    else{
      objetoHttp = this.http.post("http://localhost:60010/api/CategoriaAdquisicion/CategoriaAdquisicion", this.categoriaadquisicion, { withCredentials: true });
    }

    objetoHttp.subscribe(response =>{
      if(response['success'] == true){
        this.categoriaadquisicion.usuarioCreo = response['usuarioCreo'];
        this.categoriaadquisicion.fechaCreacion = response['fechaCreacion'];
        this.categoriaadquisicion.fechaActualizacion = response['fechaActualizacion'];
        this.categoriaadquisicion.usuarioActualizo = response['usuarioActualizo'];
        this.categoriaadquisicion.id = response['id'];

        this.esNuevo = false;
        this.obtenerTotalCategoriasAdquisicion();
        this.utils.mensaje('success', 'Categoría de adquisición guardada exitosamente');
      }
    })
  }

  IrATabla(){
    this.esColapsado = false;
    this.categoriaadquisicion = new CategoriaAdquisicion();
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
      class: 'table table-bordered grid'
    },
    hideSubHeader: true,
    pager:{
      display: false
    }
  };
}
