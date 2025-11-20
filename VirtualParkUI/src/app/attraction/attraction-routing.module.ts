import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AttractionRegisterPageComponent } from './attraction-register-page/attraction-register-page.component';
import { AttractionEditPageComponent } from './attraction-edit-page/attraction-edit-page.component';
import { AttractionListPageComponent } from './attraction-list-page/attraction-list-page.component'
import { AttractionPageComponent } from './attraction-page/attraction-page.component';
import { AttractionUpAttractionPageComponent } from './attraction-upAttraction-page/attraction-upAttraction-page.component';
import { AttractionMonitorPageComponent } from './attraction-monitor-page/attraction-monitor-page.component';
import { AttractionMonitorDetailPageComponent } from './attraction-monitor-detail-page/attraction-monitor-detail-page.component';
import { AttractionDeletedComponent } from './attraction-deleted/attraction-deleted.component';
import { VisitScoreHistoryComponent } from './visit-score-history/visit-score-history.component';

const routes: Routes = [
  {
    path: '',
    component: AttractionPageComponent,
    children: [
      { path: '', component: AttractionListPageComponent },
      { path: 'register', component: AttractionRegisterPageComponent },
      { path: 'ride', component: AttractionUpAttractionPageComponent },
      { path: 'monitor', component: AttractionMonitorPageComponent },
      { path: 'monitor/:id', component: AttractionMonitorDetailPageComponent },
      { path: 'scores', component: VisitScoreHistoryComponent },
      { path: 'deleted', component: AttractionDeletedComponent },
      { path: ':id/edit', component: AttractionEditPageComponent },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AttractionRoutingModule { }
