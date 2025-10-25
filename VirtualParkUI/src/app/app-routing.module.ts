import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { RewardFormComponent } from './reward/reward-form/reward-form.component';
import { RewardRedemptionComponent } from './reward/reward-redemption/reward-redemption.component';

const routes: Routes = [
    { path: '', component: HomeComponent },
    { path: 'rewards',
    loadChildren: () =>
      import('./reward/reward.module').then(m => m.RewardModule),
    },
    { path: 'rewards/create', component: RewardFormComponent },
    { path: 'reedem', component: RewardRedemptionComponent }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule {}
