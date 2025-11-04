import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CloackRegisterPageComponent } from './cloack-register-page/cloack-register-page.component';


const routes: Routes = [
    { path: '', component: CloackRegisterPageComponent },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class CloackRoutingModule { }