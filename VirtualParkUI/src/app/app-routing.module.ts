import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';

const routes: Routes = [
    { path: '', component: HomeComponent },
    { path: 'user', loadChildren: () => import('./user/user.module').then(m => m.UserModule) },
    { path: 'user-home', loadChildren: () => import('./userMenu/userMenu.module').then(m => m.UserMenuModule) },
    { path: 'attraction', loadChildren: () => import('./attraction/attraction.module').then(m => m.AttractionModule) },
    { path: 'rewards', loadChildren: () => import('./reward/reward.module').then(m => m.RewardModule) },
    { path: 'reedem', loadChildren: () => import('./reward-redemption/reward-redemption.module').then(m => m.RewardRedemptionModule) },
    { path: 'ranking', loadChildren: () => import('./ranking/ranking.module').then(m => m.RankingModule) },
    { path: 'events', loadChildren: () => import('./event/event.module').then(m => m.EventModule) },
    { path: 'incidences', loadChildren: () => import('./incidence/incidence.module').then(m => m.IncidenceModule) },
    { path: 'typeincidences', loadChildren: () => import('./type-incidence/type-incidence.module').then(m => m.TypeIncidenceModule) },
    { path: 'ticket', loadChildren: () => import('./ticket/ticket.module').then(m => m.TicketModule) },
    { path: 'clock', loadChildren: () => import('./clock/clock.module').then(m => m.ClockModule) },
    { path: 'reports/attractions', loadChildren: () => import('./report/report.module').then(m => m.ReportsModule) },
    { path: 'strategy', loadChildren: () => import('./strategy/strategy.module').then(m => m.StrategyModule) },
    { path: 'role', loadChildren: () => import('./role/role.module').then(m => m.RoleModule) },
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule {}