import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AttractionRegisterPageComponent } from './attraction-register-page/attraction-register-page.component';
import { AttractionEditPageComponent } from './attraction-edit-page/attraction-edit-page.component';
import { AttractionListPageComponent } from './attraction-list-page/attraction-list-page.component'
import { AttractionPageComponent } from './attraction-page/attraction-page.component';
import { AttractionDeletedComponent } from './attraction-deleted/attraction-deleted.component';

const routes: Routes = [
  {
    path: '',
    component: AttractionPageComponent,
    children: [
      { path: '', component: AttractionListPageComponent },
      { path: 'register', component: AttractionRegisterPageComponent },
      { path: ':id/edit', component: AttractionEditPageComponent },
      { path: 'deleted', component: AttractionDeletedComponent}
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AttractionRoutingModule { }