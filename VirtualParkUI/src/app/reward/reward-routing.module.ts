import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RewardPageComponent } from './reward-page/reward-page.component';
import { RewardListPageComponent } from './reward-list-page/reward-list-page.component';
import { RewardFormComponent } from '../business-components/reward/create-reward-form/reward-form.component';

const routes: Routes = [
  {
    path: '',
    component: RewardPageComponent,
    children: [
      { path: '', component: RewardListPageComponent },
      { path: 'create', component: RewardFormComponent }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RewardRoutingModule {}
