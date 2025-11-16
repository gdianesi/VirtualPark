import { Component } from '@angular/core';
import { ButtonsComponent } from '../../components/buttons/buttons.component';
import { MessageService } from '../../components/messages/service/message.service'; 
import { RoleService } from '../../../backend/services/role/role.service';
import { CreateRoleRequest } from '../../../backend/services/role/models/CreateRoleRequest';
import { FormsModule } from '@angular/forms';
import { MessageComponent } from '../../components/messages/message.component';
import { CommonModule } from '@angular/common';
import { PermissionService } from '../../../backend/services/permission/permission.service';

@Component({
  selector: 'app-role-create-page',
  standalone: true,
  imports: [ButtonsComponent, FormsModule, MessageComponent, CommonModule],
  templateUrl: './role-create-page.component.html',
  styleUrls: ['./role-create-page.component.css']
})
export class RoleCreatePageComponent {

  newRole: CreateRoleRequest = { 
    name: '', 
    description: '', 
    permissionsIds: []   
  };

  permission: string = '99999999-1111-1111-1111-111111111113';

  constructor(
    private _messageService: MessageService,
    private _roleService: RoleService,
    private _permissionService: PermissionService
  ) {}

  onCreateRole() {
  if (this.newRole.name.trim() === '') {
    this._messageService.show('The role name is required.', 'error');
    return;
  }

  if (this.newRole.description.trim() === '') {   
    this._messageService.show('The role description is mandatory.', 'error');
    return;
  }

  if (!this.newRole.permissionsIds || this.newRole.permissionsIds.length === 0) {
    this.newRole.permissionsIds = [this.permission];
  }

  this._roleService.create(this.newRole).subscribe({
    next: () => {
      this._messageService.show('Role successfully created.', 'success');

      this.newRole = { 
        name: '', 
        description: '', 
        permissionsIds: [this.permission] 
      };
    },
    error: (error) => {
      this._messageService.show('Error creating role: ' + error.message, 'error');
    }
  }); 
}

}
