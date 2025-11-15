import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IncidenceRoutingModule } from './incidence-routing.module';
import { IncidencePageComponent } from './incidence-page/incidence-page.component';

@NgModule({
  imports: [
    CommonModule,
    IncidenceRoutingModule,
    IncidencePageComponent
  ],
})
export class IncidenceModule {}
