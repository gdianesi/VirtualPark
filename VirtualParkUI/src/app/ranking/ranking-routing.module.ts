import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RankingPageComponent } from '../business-components/ranking-page/ranking-page.component';
import { RankingDetailComponent } from '../business-components/ranking/ranking-detail/ranking-detail.component';

const routes: Routes = [
    { path: '', component: RankingPageComponent },
  { path: ':id', component: RankingDetailComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RankingRoutingModule { }
