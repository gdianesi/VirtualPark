import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { RewardFormComponent } from './business-components/reward-form/reward-form.component';
import { RewardRedemptionComponent } from './business-components/reward-redemption/reward-redemption.component';

const routes: Routes = [
    { path: '', component: HomeComponent },
    {
        path: 'user',
        loadChildren: () => import('./user/user.module').then(m => m.UserModule)
    },
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