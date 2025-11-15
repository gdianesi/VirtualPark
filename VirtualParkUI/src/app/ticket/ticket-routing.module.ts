import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TicketPageComponent } from './ticket-page/ticket-page.component';
import { TicketListPageComponent } from './ticket-list-page/ticket-list-page.component';
import { TicketRegisterPageComponent } from './ticket-register-page/ticket-register-page.component';

const routes: Routes = [
  {
    path: '',
    component: TicketPageComponent,
    children: [
      { path: '', component: TicketListPageComponent },
      { path: 'register', component: TicketRegisterPageComponent },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TicketRoutingModule {}
