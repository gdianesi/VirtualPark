import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RoleService } from '../../../backend/services/role/role.service';
import { RoleModel } from '../../../backend/services/role/models/RoleModel';
import { MessageService } from '../../components/messages/service/message.service';
import { PermissionService } from '../../../backend/services/permission/permission.service';
import { PermissionModel } from '../../../backend/services/permission/models/PermissionModel';

@Component({
    selector: 'app-role-permission-page',
    standalone: true,
    imports: [FormsModule],
    templateUrl: './role-permission-page.component.html',
    styleUrls: ['./role-permission-page.component.css']
})
export class RolePermissionPageComponent {
    errorMessage = '';
    roles: RoleModel[] = [];
    permissions: PermissionModel[] = [];

    selectedRoleId: number | null = null;
    selectedPermissionIds: number[] = [];

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
                    `Error fetching roles: ${err.message || 'Please try again.'}`,
                    'error'
                );
            }
        });
    }

    loadPermissions() {
        this._permissionService.getAll().subscribe({
            next: (data: PermissionModel[]) => {
                this.permissions = data;
            },
            error: (err) => {
                this._messageService.show(
                    `Error fetching permissions: ${err.message || 'Please try again.'}`,
                    'error'
                );
            }
        });
    }

    // 👇 se dispara cuando el usuario cambia el rol en el select
    onRoleChange() {
        // Acá podrías cargar los permisos actuales del rol desde el backend.
        // Por ahora, lo dejo simple: limpiamos la selección
        this.selectedPermissionIds = [];
    }

    // ayuda para saber si un permiso está seleccionado
    isPermissionSelected(permissionId: number): boolean {
        return this.selectedPermissionIds.includes(permissionId);
    }

    // se dispara cuando el usuario tilda/destilda un permiso
    onPermissionToggle(permissionId: number, checked: boolean) {
        if (checked) {
            if (!this.selectedPermissionIds.includes(permissionId)) {
                this.selectedPermissionIds.push(permissionId);
            }
        } else {
            this.selectedPermissionIds = this.selectedPermissionIds.filter(id => id !== permissionId);
        }
    }

    // guardar la asignación (ejemplo)
    onSavePermissions() {
        if (!this.selectedRoleId) {
            this._messageService.show('Please select a role first.', 'error');
            return;
        }
    }
}
