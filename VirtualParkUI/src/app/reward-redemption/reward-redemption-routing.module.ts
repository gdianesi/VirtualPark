import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RewardRedemptionHistoryComponent } from './reward-redemption-history/reward-redemption-history.component';
import { RewardRedemptionComponent } from './reward-redemption-list/reward-redemption.component';

const routes: Routes = [
  { path: '', component: RewardRedemptionComponent },
  { path: 'history', component: RewardRedemptionHistoryComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RewardRedemptionRoutingModule { }
