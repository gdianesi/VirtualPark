import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EventDetailComponent } from './event-detail-page/event-detail.component';
import { EventPageComponent } from './event-list-page/event-page.component';

const routes: Routes = [
  { path: '', component: EventPageComponent },
  { path: ':id', component: EventDetailComponent },

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EventRoutingModule {}

