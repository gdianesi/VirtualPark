import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { RewardRoutingModule } from './reward-routing.module';
import { RewardPageComponent } from './reward-page/reward-page.component';
import { RewardFormComponent } from './reward-form/reward-form.component';
import { ButtonsComponent } from '../components/buttons/buttons.component';
import { FormsModule } from '@angular/forms';



@NgModule({
  declarations: [
    RewardPageComponent,
    RewardFormComponent,
  ],
  imports: [
    CommonModule,
    RewardRoutingModule,
    ButtonsComponent,
    FormsModule
  ]
})
export class RewardModule { }
