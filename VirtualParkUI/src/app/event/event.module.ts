import { NgModule } from '@angular/core';
import { EventRoutingModule } from './event-routing.module';
import { EventPageComponent } from './event-page/event-page.component';

@NgModule({
  imports: [EventRoutingModule, EventPageComponent],
  declarations: [
  ],
})
export class EventModule {}
