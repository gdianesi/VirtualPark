import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RankingPageComponent } from './ranking-page/ranking-page.component';
import { RankingListPageComponent } from './ranking-list-page/ranking-list-page.component';

const routes: Routes = [
  {
    path: '',
    component: RankingPageComponent,
    children: [
      { path: '', component: RankingListPageComponent }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RankingRoutingModule {}
