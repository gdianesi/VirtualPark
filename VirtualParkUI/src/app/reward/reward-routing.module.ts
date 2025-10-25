import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RewardPageComponent } from './reward-page/reward-page.component';
import { RewardFormComponent } from './reward-form/reward-form.component';

const routes: Routes = [
  { path: '', component: RewardPageComponent },
  { path: 'rewards/create', component: RewardFormComponent }

];


@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RewardRoutingModule { }
