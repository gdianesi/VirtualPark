import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RoleService } from '../../../backend/services/role/role.service';
import { RoleModel } from '../../../backend/services/role/models/RoleModel';
import { MessageService } from '../../../backend/services/message/message.service';
import { PermissionService } from '../../../backend/services/permission/permission.service';
import { PermissionModel } from '../../../backend/services/permission/models/PermissionModel';
import { CommonModule } from '@angular/common';
import { ButtonsComponent } from '../../components/buttons/buttons.component';
import { CreateRoleRequest } from '../../../backend/services/role/models/CreateRoleRequest';
import { MessageComponent } from '../../components/messages/message.component';

@Component({
    selector: 'app-role-permission-page',
    standalone: true,
    imports: [FormsModule, CommonModule, ButtonsComponent, MessageComponent],
    templateUrl: './role-permission-page.component.html',
    styleUrls: ['./role-permission-page.component.css']
})
export class RolePermissionPageComponent {
    errorMessage = '';
    roles: RoleModel[] = [];
    permissions: PermissionModel[] = [];

    selectedRoleId: string | null = null;
    selectedPermissionIds: string[] = [];

    constructor(
        private _roleService: RoleService,
        private _messageService: MessageService,
        private _permissionService: PermissionService
    ) {
        this.loadRoles();
        this.loadPermissions();
    }

    loadRoles() {
        this._roleService.getAll().subscribe({
            next: (data: RoleModel[]) => {
                this.roles = data;
            },
            error: (err) => {
                this._messageService.show(
                    `Error fetching roles: ${err.message || 'Please try again.'}`,'error');
            }
        });
    }

    loadPermissions() {
        this._permissionService.getAll().subscribe({
            next: (data: PermissionModel[]) => {
                this.permissions = data;
            },
            error: (err) => {
                this._messageService.show(`Error fetching permissions: ${err.message || 'Please try again.'}`,'error');
            }
        });
    }

    onRoleChange() {
        const roleSelected = this.roles.find(r => r.id === this.selectedRoleId);

        this.selectedPermissionIds = roleSelected?.permissionIds ?? [];
    }


    isPermissionSelected(permissionId: string): boolean {
        return this.selectedPermissionIds.includes(permissionId);
    }

    onPermissionToggle(permissionId: string, event: Event) {
    const input = event.target as HTMLInputElement;
    const checked = input.checked;

    if (checked) {
        if (!this.selectedPermissionIds.includes(permissionId)) {
        this.selectedPermissionIds.push(permissionId);
        }
    } else {
        this.selectedPermissionIds = this.selectedPermissionIds.filter(id => id !== permissionId);
    }
    }


    onSavePermissions() {
        if (!this.selectedRoleId) {
            this._messageService.show('Please select a role first.', 'error');
            return;
        }

        const roleToUpdate = this.roles.find(r => r.id === this.selectedRoleId);

        if (!roleToUpdate) {
            this._messageService.show('Selected role not found.', 'error');
            return;
        }

        const roleUpdate: CreateRoleRequest = {
            name: roleToUpdate.name,
            description: roleToUpdate.description,
            permissionsIds: this.selectedPermissionIds
        };

        this._roleService.update(this.selectedRoleId, roleUpdate).subscribe({
            next: () => {
            this._messageService.show('Permissions updated successfully.', 'success');
            },
            error: (err) => {
            this._messageService.show(
                `Error updating permissions: ${err.message || 'Please try again.'}`,
                'error'
            );
            }
        });
    }

}
