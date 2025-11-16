import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { Router } from '@angular/router';
import { TableColumn, TableComponent } from '../../components/table/generic-table.component';
import { RoleService } from '../../../backend/services/role/role.service';
import { ButtonsComponent } from '../../components/buttons/buttons.component';
import { MessageService } from '../../components/messages/service/message.service';
import { ConfirmDialogComponent } from '../../components/confirm-dialog/confirm-dialog.component';
import { MessageComponent } from '../../components/messages/message.component';

type Row = {
  id: string;
  name: string;
  description: string;
};

@Component({
  selector: 'app-role-list-page',
  standalone: true,
  imports: [
    CommonModule,
    TableComponent,
    ButtonsComponent,
    ConfirmDialogComponent,
    MessageComponent
  ],
  templateUrl: './role-list-page.component.html',
  styleUrls: ['./role-list-page.component.css']
})
export class RoleListPageComponent implements OnInit {

  private roleService = inject(RoleService);
  private messageService = inject(MessageService);

  private router = inject(Router);

  showConfirm = false;
  selectedRoleId: string | null = null;

  loading = true;
  deletingId: string | null = null;
  errorMsg = '';

  columns: TableColumn<Row>[] = [
    { key: 'name',        label: 'Name' },
    { key: 'description', label: 'Description' },
    { key: 'actions',     label: 'Actions', width: '180px', align: 'right' },
  ];

  data: Row[] = [];
  protectedRoles = ['Visitor', 'Administrator', 'Operator'];

  ngOnInit(): void {
    this.loadRoles();
  }

  isProtectedRole(name: string): boolean {
    return this.protectedRoles.includes(name);
  }

  private loadRoles(): void {
    this.loading = true;
    this.errorMsg = '';

    this.roleService.getAll().subscribe({
      next: (roles) => {
        this.data = (roles ?? [])
          .map((role: any) => ({
            id: role.id ?? role.Id ?? '',
            name: role.name ?? role.Name ?? '',
            description: role.description ?? role.Description ?? '',
          }))
          .filter(row => !!row.id);

        this.loading = false;
      },
      error: (error) => {
        console.error(error);
        this.errorMsg = 'The roles could not be loaded.';
        this.loading = false;
      }
    });
  }


  onDelete(row: Row): void {
    this.selectedRoleId = row.id;
    this.showConfirm = true;
  }

  onConfirmDelete(result: boolean): void {
    this.showConfirm = false;

    if (!result || !this.selectedRoleId) {
      return;
    }

    this.deletingId = this.selectedRoleId;

    this.roleService.remove(this.selectedRoleId).subscribe({
      next: () => {
        this.data = this.data.filter(item => item.id !== this.selectedRoleId);
        this.deletingId = null;
        this.selectedRoleId = null;

        this.messageService.show('Role successfully deleted!', 'success');
      },
      error: () => {
        this.deletingId = null;
        this.messageService.show('Could not delete the role.', 'error');
      }
    });
  }
}
