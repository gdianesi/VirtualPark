import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EventDetailComponent } from './event-detail-page/event-detail.component';
import { EventListPageComponent } from './event-list-page/event-list-page.component';
import { EventCreateComponent } from './event-create-form/event-create-form.component';
import { EventPageComponent } from './event-page/event-page.component';

const routes: Routes = [
  {
    path: '',
    component: EventPageComponent,
    children: [
      { path: '', component: EventListPageComponent },
      { path: 'new', component: EventCreateComponent },
      { path: ':id', component: EventDetailComponent },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EventRoutingModule {}

