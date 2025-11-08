import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EventRoutingModule } from './event-routing.module';
import { EventListPageComponent } from './event-list-page/event-list-page.component';
import { ButtonsComponent } from '../components/buttons/buttons.component';
import { EventCreateComponent } from './event-create-form/event-create-form.component';
import { EventPageComponent } from './event-page/event-page.component';

@NgModule({
  declarations: [EventPageComponent
  ],
  imports: [
    CommonModule,
    EventRoutingModule,
    ButtonsComponent,
    EventCreateComponent,
    EventListPageComponent,
  ]
})
export class EventModule {}
