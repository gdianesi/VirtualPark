import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { RankingPageComponent } from './ranking/ranking-list-page/ranking-page.component';
import { RewardRedemptionComponent } from './reward-redemption/reward-redemption-list/reward-redemption.component';


const routes: Routes = [
    { path: '', component: HomeComponent },
    {
        path: 'user',
        loadChildren: () => import('./user/user.module').then(m => m.UserModule)
    },
    {
        path: 'attraction',
        loadChildren: () => import('./attraction/attraction.module').then(m => m.AttractionModule)
    },
    { path: 'rewards',
        loadChildren: () => import('./reward/reward.module').then(m => m.RewardModule),
    },
    { path: 'reedem',
        loadChildren: () => import('./reward-redemption/reward-redemption.module').then(m => m.RewardRedemptionModule),
    },
    { path: 'ranking', component: RankingPageComponent},
    { path: 'events', loadChildren: () => import('./event/event.module').then(m => m.EventModule) },

];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule {}