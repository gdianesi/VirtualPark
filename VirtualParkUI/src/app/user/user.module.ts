import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserRegisterPageComponent } from './user-register-page/user-register-page.component';
import { UserLoginPageComponent } from './user-login-page/user-login-page.component';

const routes: Routes = [
    { path: 'register', component: UserRegisterPageComponent },
    { path: 'login', component: UserLoginPageComponent }
];

@NgModule({
    imports: [RouterModule.forChild(routes)], 
})
export class UserModule {}
