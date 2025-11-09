import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserRegisterPageComponent } from './user-register-page/user-register-page.component';
import { UserLoginPageComponent } from './user-login-page/user-login-page.component';
import { MainPageComponent } from '../main-page/main-page.component';
import { UserEditPageComponent } from './user-edit-page/user-edit-page.component';
import { UserListPageComponent } from './user-list-page/user-list-page.component';

const routes: Routes = [
    { path: 'list', component: UserListPageComponent },
    { path: 'register', component: UserRegisterPageComponent },
    { path: 'login', component: UserLoginPageComponent },
    { path: 'home', component: MainPageComponent },
    { path: 'edit/:id', component: UserEditPageComponent },
    { path: 'profile', component: UserEditPageComponent }
];

@NgModule({
    imports: [RouterModule.forChild(routes)], 
})
export class UserModule {}
