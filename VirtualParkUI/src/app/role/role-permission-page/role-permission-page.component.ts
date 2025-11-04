import { Component } from '@angular/core';
import { AddPermissionRoleFormComponent } from '../../business-components/role/add-permission-role-form/add-permission-role-form.component';
import { RoleService } from '../../../backend/services/role/role.service';

@Component({
    selector: 'app-role-permission-page',
    standalone: true,
    imports: [AddPermissionRoleFormComponent], 
    templateUrl: './role-permission-page.component.html',
    styleUrls: ['./role-permission-page.component.css'] // <- ojo: plural
})
export class RolePermissionPageComponent {
    isLoading = false;
    errorMessage = '';

    constructor(private roleService: RoleService) {}

    handleAssign(roleForm: any) {
        roleForm.onSubmit();
    }

    onFormSubmit(payload: { roleId: string; permissionsIds: string[] }) {
        this.isLoading = true;
        this.errorMessage = '';
        this.roleService.update(payload.roleId, payload.permissionsIds)
            .subscribe({
                next: () => { this.isLoading = false;},
                error: () => { this.isLoading = false; this.errorMessage = 'No se pudo asignar permisos.'; }
            });
    }
}
