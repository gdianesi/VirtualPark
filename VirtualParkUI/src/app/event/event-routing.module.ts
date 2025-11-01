import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EventDetailComponent } from './event-detail-page/event-detail.component';
import { EventPageComponent } from './event-list-page/event-page.component';
import { EventCreateComponent } from './event-create-form/event-create-form.component';

const routes: Routes = [
  { path: '', component: EventPageComponent },
  { path: 'new', component: EventCreateComponent },
  { path: ':id', component: EventDetailComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EventRoutingModule {}

