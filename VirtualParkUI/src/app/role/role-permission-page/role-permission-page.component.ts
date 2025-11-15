import { Component } from '@angular/core';
import { AddPermissionRoleFormComponent } from '../../business-components/role/add-permission-role-form/add-permission-role-form.component';
import { RoleService } from '../../../backend/services/role/role.service';
import { RoleModel } from '../../../backend/services/role/models/RoleModel';
import { MessageService } from '../../components/messages/service/message.service';

@Component({
    selector: 'app-role-permission-page',
    standalone: true,
    imports: [AddPermissionRoleFormComponent],
    templateUrl: './role-permission-page.component.html',
    styleUrls: ['./role-permission-page.component.css']
})
export class RolePermissionPageComponent {
    errorMessage = '';
    roles: RoleModel[] = [];

    constructor(private _roleService: RoleService, private _messageService: MessageService) {
        this.loadRoles();
        this.loadPermissions();
    }

    loadRoles() {
        this._roleService.getAll().subscribe({
            next: (data) => {
                this.roles = data;
            },
            error: (err) => {
                this._messageService.show( `Error fetching roles: ${err.message || 'Please try again.'}`,'error');
            }
        });
    }

    loadPermissions(){
        
    }
}
