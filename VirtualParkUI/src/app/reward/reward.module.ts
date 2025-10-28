import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { RewardRoutingModule } from './reward-routing.module';
import { RewardPageComponent } from '../business-components/reward-page/reward-page.component';
import { RewardFormComponent } from '../business-components/reward-form/reward-form.component';
import { ButtonsComponent } from '../components/buttons/buttons.component';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    RewardPageComponent,
  ],
  imports: [
    CommonModule,
    RewardRoutingModule,
    ButtonsComponent,
    FormsModule
  ]
})
export class RewardModule { }
