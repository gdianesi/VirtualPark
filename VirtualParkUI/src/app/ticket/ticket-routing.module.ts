import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TicketRegisterPageComponent } from './ticket-register-page/ticket-register-page.component';

const routes: Routes = [
  { path: 'register', component: TicketRegisterPageComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class TicketRoutingModule {}
