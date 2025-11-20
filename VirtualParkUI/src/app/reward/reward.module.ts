import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RewardRoutingModule } from './reward-routing.module';
import { RewardPageComponent } from './reward-page/reward-page.component';
import { RewardFormComponent } from './create-reward-form/reward-form.component';
import { ButtonsComponent } from '../components/buttons/buttons.component';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    RewardRoutingModule,
    RewardPageComponent,
    RewardFormComponent,
    ButtonsComponent,
    FormsModule
  ]
})
export class RewardModule {}
