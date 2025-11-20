import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserPageComponent } from '../userMenu/user-page/user-page.component';
import { UserListPageComponent } from './user-list-page/user-list-page.component';
import { UserCreatePageComponent } from './user-create-page/user-create-page.component';

const routes: Routes = [
    {
        path: '',
        component: UserPageComponent,
        children: [
            { path: '', redirectTo: 'list', pathMatch: 'full' },
            { path: 'list', component: UserListPageComponent },
            { path: 'create', component: UserCreatePageComponent },
        ],
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class UserMenuRoutingModule { }
