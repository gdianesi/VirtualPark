import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RewardRedemptionPageComponent } from './reward-redemption-page/reward-redemption-page.component';
import { RewardRedemptionComponent } from './reward-redemption-list/reward-redemption.component';
import { RewardRedemptionHistoryComponent } from './reward-redemption-history/reward-redemption-history.component';

const routes: Routes = [
  {
    path: '',
    component: RewardRedemptionPageComponent,
    children: [
      { path: '', component: RewardRedemptionComponent },
      { path: 'history', component: RewardRedemptionHistoryComponent },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RewardRedemptionRoutingModule {}
