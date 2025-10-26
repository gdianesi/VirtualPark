import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AttractionRegisterPageComponent } from './attraction-register-page/attraction-register-page.component';

const routes: Routes = [
  { path: 'register', component: AttractionRegisterPageComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AttractionRoutingModule {}
