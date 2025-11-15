import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ClockPageComponent } from './clock-page/clock-page.component';
import { ClockRegisterPageComponent } from './clock-register-page/clock-register-page.component';

const routes: Routes = [
  {
    path: '',
    component: ClockPageComponent,
    children: [
      { path: '', component: ClockRegisterPageComponent }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ClockRoutingModule {}
