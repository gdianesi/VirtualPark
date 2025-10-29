import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RewardPageComponent } from './reward-list-page/reward-page.component';
import { RewardFormComponent } from '../business-components/reward/create- reward-form/reward-form.component';
import { RewardRedemptionHistoryComponent } from './reward-redemption-history/reward-redemption-history.component';

const routes: Routes = [
  { path: '', component: RewardPageComponent },
  { path: 'create', component: RewardFormComponent },
  { path: 'history', component: RewardRedemptionHistoryComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RewardRoutingModule { }
