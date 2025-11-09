import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TypeIncidenceRoutingModule } from './type-incidence-routing.module';
import { TypeIncidencePageComponent } from './type-incidence-page/type-incidence-page.component';

@NgModule({
  imports: [
    CommonModule,
    TypeIncidenceRoutingModule,
    TypeIncidencePageComponent
  ]
})
export class TypeIncidenceModule {}
