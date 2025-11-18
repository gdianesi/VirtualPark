import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RoleService } from '../../../backend/services/role/role.service';
import { MessageService } from '../../../backend/services/message/message.service';
import { RoleModel } from '../../../backend/services/role/models/RoleModel';
import { CommonModule } from '@angular/common';
import { ButtonsComponent } from '../../components/buttons/buttons.component';
import { MessageComponent } from '../../components/messages/message.component';
import { CreateRoleRequest } from '../../../backend/services/role/models/CreateRoleRequest';

@Component({
  selector: 'app-role-edit-page',
  standalone: true,
  imports: [FormsModule, CommonModule, ButtonsComponent, MessageComponent],
  templateUrl: './role-edit-page.component.html',
  styleUrls: ['./role-edit-page.component.css']
})
export class RoleEditPageComponent {
  errorMessage = '';
  roles: RoleModel[] = [];

  selectedRoleId: string | null = null;
  editDescription: string = '';

  constructor(
    private _roleService: RoleService,
    private _messageService: MessageService,
  ) {
    this.loadRoles();
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

  onRoleChange() {
    const roleSelected = this.roles.find(r => r.id === this.selectedRoleId);

    if (!roleSelected) {
      this.editDescription = '';
      return;
    }
    this.editDescription = roleSelected.description ?? '';
  }

  onSave() {
    if (!this.selectedRoleId) {
      this._messageService.show('Please select a role first.', 'error');
      return;
    }

    const roleSelected = this.roles.find(r => r.id === this.selectedRoleId);

    if (!roleSelected) {
      this._messageService.show('Selected role not found.', 'error');
      return;
    }

    const newDescription = this.editDescription.trim();
    if (!newDescription) {
      this._messageService.show('Description is required.', 'error');
      return;
    }

    const roleUpdate: CreateRoleRequest = {
      name: roleSelected.name,
      description: newDescription,
      permissionsIds: roleSelected.permissionIds ?? []
    };

    this._roleService.update(this.selectedRoleId, roleUpdate).subscribe({
      next: () => {
        roleSelected.description = newDescription;
        this._messageService.show('Role description updated successfully.', 'success');
      },
      error: (err) => {
        this._messageService.show(
          `Error updating role: ${err.message || 'Please try again.'}`,
          'error'
        );
      }
    });
  }
}
