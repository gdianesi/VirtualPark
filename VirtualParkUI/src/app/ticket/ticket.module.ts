import { NgModule } from '@angular/core';
import { TicketRoutingModule } from './ticket-routing.module';
import { TicketPageComponent } from './ticket-page/ticket-page.component';

@NgModule({
  imports: [
    TicketRoutingModule,
    TicketPageComponent
  ],
})
export class TicketModule {}
