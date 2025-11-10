import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserRegisterPageComponent } from './user-register-page/user-register-page.component';
import { UserLoginPageComponent } from './user-login-page/user-login-page.component';
import { MainPageComponent } from '../main-page/main-page.component'
import { UserEditPageComponent } from './user-edit-page/user-edit-page.component';
import { UserListPageComponent } from '../userMenu/user-list-page/user-list-page.component';
import { UserCreatePageComponent } from '../userMenu/user-create-page/user-create-page.component';
import { UserPageComponent } from '../userMenu/user-page/user-page.component';


const routes: Routes = [
    { path: 'register', component: UserRegisterPageComponent },
    { path: 'create', component: UserCreatePageComponent },
    { path: 'login', component: UserLoginPageComponent },
    { path: 'home', component: MainPageComponent},
    { path: 'edit/:id', component: UserEditPageComponent},
    { path: 'profile', component: UserEditPageComponent}
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class UserRoutingModule { }
