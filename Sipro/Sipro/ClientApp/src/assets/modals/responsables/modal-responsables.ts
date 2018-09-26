import { Component, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { HttpClient } from '@angular/common/http';
import { LocalDataSource } from 'ng2-smart-table';

@Component({
  templateUrl: './modal-dialog.html'
})
export class DialogOverviewResponsables {
  constructor(public dialog: MatDialog) {}
}

@Component({
  selector: 'modal-responsables.ts',
  templateUrl: './modal-dialog.html'
})
export class DialogResponsables {
  totalElementos : number;
  source: LocalDataSource;
  paginaActual : number;
  elementosPorPagina : number;
  id : number;
  nombreCompleto: string;
  color = 'primary';
  mode = 'indeterminate';
  value = 50;
  diameter = 45;
  strokewidth = 3;
  esColapsado: boolean;
  busquedaGlobal: string;
  rolesSelected: number;

  roles = [
    {value:'r',nombre:'Responsable'},
    {value:'a',nombre:'Cuentadante'},
    {value:'c',nombre:'Consultor'},
    {value:'i',nombre:'Quien informa'}
  ];

  constructor(public dialog: MatDialog,
    public dialogRef: MatDialogRef<DialogResponsables>,
    @Inject(MAT_DIALOG_DATA) public data: any, private http: HttpClient) {
      this.elementosPorPagina = 10;
      this.busquedaGlobal = null;
      this.source = new LocalDataSource();
      let rolesids = data.idRoles;

      for (let x in rolesids){
        for (let y =0; y < this.roles.length; y++){
          if (rolesids[x] == this.roles[y].value){
            this.roles.splice(y, 1);
            break;
          }
        }
      }
    }

  ngOnInit() { 
    this.obtenerTotalColaboradores();
  }

  Ok(): void {    
    this.data = { nombre : this.nombreCompleto, id : this.id, rol : this.rolesSelected };
    this.dialogRef.close(this.data);
  }

  obtenerTotalColaboradores(){
    this.esColapsado = false;
    var filtro = {
      filtro_busqueda: this.busquedaGlobal
    };
    this.http.post('http://localhost:60011/api/Colaborador/TotalElementos',filtro, {withCredentials: true}).subscribe(response => {
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
    this.http.post('http://localhost:60011/api/Colaborador/ColaboradoresPorPagina', filtro, { withCredentials: true }).subscribe(response => {
      if (response['success'] == true) {
        var data = response["colaboradores"];        
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
    this.nombreCompleto = event.data.nombreCompleto;
    this.id = event.data.id;
  }

  onDblClickRow(event){
    this.data = { nombre : this.nombreCompleto, id : this.id, rol : this.rolesSelected };
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
      nombreCompleto: {
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
    this.obtenerTotalColaboradores();
  }
}