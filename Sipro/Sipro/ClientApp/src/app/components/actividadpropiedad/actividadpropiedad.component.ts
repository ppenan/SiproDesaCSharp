import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { ButtonDeleteComponent } from '../../../assets/customs/ButtonDeleteComponent';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from '../../auth.service';
import { UtilsService } from '../../utils.service';
import { HttpClient } from '@angular/common/http';
import { MatDialog } from '@angular/material';
import { LocalDataSource } from 'ng2-smart-table';
import * as moment from 'moment';
import { ActividadPropiedad } from './model/ActividadPropiedad';
import { DialogComponentePropiedad, DialogOverviewComponentePropiedad } from '../../../assets/modals/componentepropiedad/modal-componente-propiedad';
import { DialogDelete, DialogOverviewDelete } from '../../../assets/modals/deleteconfirmation/confirmation-delete';


@Component({
  selector: 'app-actividadpropiedad',
  templateUrl: './actividadpropiedad.component.html',
  styleUrls: ['./actividadpropiedad.component.css']
})
export class ActividadpropiedadComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

}
