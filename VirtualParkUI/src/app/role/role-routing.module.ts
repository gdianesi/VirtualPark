import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AddPermissionRoleFormComponent } from '../business-components/role/add-permission-role-form/add-permission-role-form.component'

const routes: Routes = [
    { path: 'assign-permissions', component: AddPermissionRoleFormComponent },
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class RoleRoutingModule {}