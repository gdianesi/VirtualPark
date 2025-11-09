import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { IncidencePageComponent } from './incidence-page/incidence-page.component';
import { IncidencePageListComponent } from './incidence-list-page/incidence-list-page.component';
import { IncidenceFormComponent } from './incidence-form-page/incidence-form-page.component';

const routes: Routes = [
  {
    path: '',
    component: IncidencePageComponent,
    children: [
      { path: '', component: IncidencePageListComponent },
      { path: 'new', component: IncidenceFormComponent }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class IncidenceRoutingModule {}
