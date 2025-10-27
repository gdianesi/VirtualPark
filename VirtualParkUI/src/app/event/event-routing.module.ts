import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EventPageComponent } from '../business-components/event-page/event-page.component';
import { EventDetailComponent } from '../business-components/event-detail/event-detail.component';


const routes: Routes = [
  { path: '', component: EventPageComponent },
  { path: ':id', component: EventDetailComponent },

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EventRoutingModule {}

