import { Component, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { HttpClient } from '@angular/common/http';
import { LocalDataSource } from 'ng2-smart-table';

@Component({
  templateUrl: './modal-dialog.html'
})
export class DialogOverviewProductoPropiedad {
  constructor(public dialog: MatDialog) {}
}

@Component({
  selector: 'modal-producto-propiedad.ts',
  templateUrl: './modal-dialog.html'
})
export class DialogProductoPropiedad {
  totalElementos : number;
  source: LocalDataSource;
  paginaActual : number;
  elementosPorPagina : number;
  id : number;
  nombre: string;
  tipoDatoNombre : string
  color = 'primary';
  mode = 'indeterminate';
  value = 50;
  diameter = 45;
  strokewidth = 3;
  esColapsado: boolean;
  busquedaGlobal: string;

  constructor(public dialog: MatDialog,
    public dialogRef: MatDialogRef<DialogProductoPropiedad>,
    @Inject(MAT_DIALOG_DATA) public data: any, private http: HttpClient) {
      this.elementosPorPagina = 10;
      this.busquedaGlobal = null;
      this.source = new LocalDataSource();
    }

  ngOnInit() { 
    this.obtenerTotalProductosPropiedades();
  }

  Ok(): void {    
    this.data = { nombre : this.nombre, id : this.id, datoTiponombre: this.tipoDatoNombre };
    this.dialogRef.close(this.data);
  }

  obtenerTotalProductosPropiedades(){
    this.esColapsado = false;
    var filtro = {
      filtro_busqueda: this.busquedaGlobal
    };
    this.http.post('http://localhost:60059/api/ProductoPropiedad/totalElementos',filtro, {withCredentials: true}).subscribe(response => {
      if (response['success'] == true) {   
        this.totalElementos = response["total"];
        this.paginaActual = 1;
        this.cargarTabla(this.paginaActual);
      } else {
        console.log('Error');
      }
    });
  }

  cargarTabla(pagina? : number){
    var filtro = {
      pagina: pagina,
      numeroSubComponentePropiedad: this.elementosPorPagina,
      filtro_busqueda: this.busquedaGlobal
    }
    this.http.post('http://localhost:60059/api/ProductoPropiedad/ProductoPropiedadPagina', filtro, { withCredentials: true }).subscribe(response => {
      if (response['success'] == true) {
        var data = response["productoPropiedades"];        
        this.source = new LocalDataSource(data);
        this.esColapsado = true;

        this.source.setSort([
          { field: 'id', direction: 'asc' }  // primary sort
        ]);
      } else {
        console.log('Error');
      }
    });
  }

  cancelar(){
    this.dialogRef.close();
  }

  onSelectRow(event){
    this.nombre = event.data.nombre;
    this.id = event.data.id;
    this.tipoDatoNombre = event.data.datoTiponombre;
  }

  onDblClickRow(event){
    this.data = { nombre : this.nombre, id : this.id, datoTiponombre : this.tipoDatoNombre };
    this.dialogRef.close(this.data);
  }

  settings = {
   columns: {
      id: {
        title: 'ID',
        width: '10%',
        filter: false,
        type: 'html',
        valuePrepareFunction : (cell) => {
          return "<div class=\"datos-numericos\">" + cell + "</div>";
        }
      },
      nombre: {
        title: 'Nombre',
        filter: false, 
        class: 'align-left'      
      }
    },
    actions: false,
    noDataMessage: 'No se encontró información.',
    attr: {
      class: 'table table-bordered grid estilo-letra'
    },
    hideSubHeader: true
  };

  handlePage(event){
    this.esColapsado = false;
    this.cargarTabla(event.pageIndex+1);
  }

  filtrar(campo){  
    this.busquedaGlobal = campo;
    this.obtenerTotalProductosPropiedades();
  }
} 