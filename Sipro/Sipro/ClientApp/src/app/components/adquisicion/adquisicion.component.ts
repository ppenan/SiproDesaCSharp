import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import * as moment from 'moment';
import { Adquisicion } from './model/Adquisicion';
import { ActivatedRoute } from "@angular/router";
import { AuthService } from '../../auth.service';
import { UtilsService } from '../../utils.service';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { FormControl } from '@angular/forms';
import { map, startWith } from 'rxjs/operators';

export interface Categoria{
  id: number;
  nombre: string;
  descripcion: string;
  usuarioCreo: string;
  usuarioActualizo: string;
  fechaCreacion: string;
  fechaActualizacion: string;
  estado: number;
}

export interface TipoAdquisicion{
  id: number;
  cooperantecodigo: number;
  nombre: string;
  estado: number;
  convenioCdirecta: boolean;
}


@Component({
  selector: 'app-adquisicion',
  templateUrl: './adquisicion.component.html',
  styleUrls: ['./adquisicion.component.css']
})
export class AdquisicionComponent implements OnInit {
  isLoggedIn : boolean;
  isMasterPage : boolean;
  adquisicion: Adquisicion;
  categorias = [];
  tipoadquisiciones = [];

  @Input() public obj_componentIn: any;
  @Output() child_adquisiciones = new EventEmitter<Adquisicion>();

  filteredCategoria: Observable<Categoria[]>;
  myControlCategoria = new FormControl();
  filteredTipos: Observable<TipoAdquisicion[]>;
  myControlTipo = new FormControl();

  constructor(private route: ActivatedRoute, private auth: AuthService, private utils: UtilsService, private http: HttpClient) { 
    this.isMasterPage = this.auth.isLoggedIn();
    this.utils.setIsMasterPage(this.isMasterPage);
    this.adquisicion = new Adquisicion();

    this.filteredCategoria = this.myControlCategoria.valueChanges
      .pipe(
        startWith(''),
        map(value => value ? this._filterCategoria(value) : this.categorias.slice())
      );

    this.filteredTipos = this.myControlTipo.valueChanges
      .pipe(
        startWith(''),
        map(value => value ? this._filterTipoAdquisicion(value): this.tipoadquisiciones.slice())
      );
  }

  private _filterCategoria(value: string): Categoria[] {
    const filterValue = value.toLowerCase();
    return this.categorias.filter(c => c.nombre.toLowerCase().indexOf(filterValue) === 0);
  }

  categoriaSeleccionado(value){
    this.adquisicion.categoriaAdquisicion = value.id;
  }

  private _filterTipoAdquisicion(value: string): TipoAdquisicion[] {
    const filterValue = value.toLowerCase();
    return this.tipoadquisiciones.filter(c => c.nombre.toLowerCase().indexOf(filterValue) === 0);
  }

  tipoAdquisicionSeleccionado(value){
    this.adquisicion.tipoAdquisicion = value.id;
  }

  ngOnInit() {
    this.obtenerCategoriasAdquisicion();
    this.obtenerTipoAdquisicion();
  }

  obtenerCategoriasAdquisicion(){
    this.http.get('http://localhost:60010/api/CategoriaAdquisicion/CategoriasAdquisicion', { withCredentials: true}).subscribe(response => {
      if (response['success'] == true) {
        this.categorias = response["categoriasAdquisicion"];        
      }
    })
  }

  obtenerTipoAdquisicion(){
    this.http.get('http://localhost:60087/api/TipoAdquisicion/TipoAdquisicionPorObjeto/'+ this.obj_componentIn['objeto_id'] + '/'+ this.obj_componentIn['objeto_tipo'], { withCredentials: true}).subscribe(response => {
      if(response['success'] == true){
        this.tipoadquisiciones = response['tipoAdquisiciones'];
      }
    });
  }

  settingsNog = {
    columns: {
      numeroContrato: {
        title: 'No. Contrato',
        width: '14%',
        filter: false,
        type: 'html',
        valuePrepareFunction : (cell) => {
          return "<div class=\"datos-numericos\">" + cell + "</div>";
        }
      },
      montoContrato: {
        title: 'Monto',
        width: '14%',
        filter: false,
        type: 'html',
        valuePrepareFunction : (cell) => {
          return "<div class=\"datos-numericos\">" + cell + "</div>";
        }
      },
      preparacionDocReal: {
        title: 'Prep. de doctos (Real)',
        width: '14%',
        filter: false,       
      },
      lanzamientoEventoReal: {
        title: 'Lanzamiento de evento (Real)',
        width: '14%',
        filter: false,       
      },
      recepcionOfertasReal: {
        title: 'Recepción de ofertas (Real)',
        width: '14%',
        filter: false,       
      },
      adjudicacionReal: {
        title: 'Adjudicación (Real)',
        width: '14%',
        filter: false,       
      },
      firmaContratoReal: {
        title: 'Firma contrato (Real)',
        width: '14%',
        filter: false,       
      }
    },
    actions: false,
    noDataMessage: 'No se obtuvo información...',
    attr: {
      class: 'table table-bordered grid estilo-letra'
    },
    hideSubHeader: true
  };
}
