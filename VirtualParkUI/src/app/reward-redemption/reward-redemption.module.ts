import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RewardRedemptionRoutingModule } from './reward-redemption-routing.module';
import { RewardRedemptionPageComponent } from './reward-redemption-page/reward-redemption-page.component';

@NgModule({
  imports: [
    CommonModule,
    RewardRedemptionRoutingModule,
    RewardRedemptionPageComponent
  ]
})
export class RewardRedemptionModule {}
