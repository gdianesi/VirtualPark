import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { IncidenceRoutingModule } from './incidence-routing.module';
import { Routes } from '@angular/router';
import { IncidencePageListComponent } from './incidence-list-page/incidence-list-page.component';
import { IncidentFormPageComponent } from './incident-form-page/incident-form-page.component';

const routes: Routes = [
  { path: '', component: IncidencePageListComponent },
];

@NgModule({
  declarations: [
    IncidentFormPageComponent
  ],
  imports: [
    CommonModule,
    IncidenceRoutingModule
  ]
})
export class IncidenceModule { }
