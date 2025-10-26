import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserRegisterPageComponent } from './user-register-page/user-register-page.component';

const routes: Routes = [
    { path: 'register', component: UserRegisterPageComponent }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class UserRoutingModule {}
