import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { TableColumn, TableComponent } from '../../components/table/generic-table.component';
import { UserService } from '../../../backend/services/user/user.service';
import { ButtonsComponent } from '../../components/buttons/buttons.component';
import { MessageService } from '../../../backend/services/message/message.service';
import { ConfirmDialogComponent } from '../../components/confirm-dialog/confirm-dialog.component';
import { MessageComponent } from "../../components/messages/message.component";

type Row = {
    id: string;
    name: string;
    lastName: string;
};

@Component({
    selector: 'app-user-list-page',
    standalone: true,
    imports: [CommonModule, TableComponent, RouterLink, ButtonsComponent, ConfirmDialogComponent, MessageComponent],
    templateUrl: './user-list-page.component.html',
    styleUrls: ['./user-list-page.component.css']
})
export class UserListPageComponent implements OnInit {
    private userService = inject(UserService);
    private messageService = inject(MessageService);

    showConfirm = false;
    selectedUserId: string | null = null;
    
    loading = true;
    deletingId: string | null = null;
    errorMsg = '';

    columns: TableColumn<Row>[] = [
        { key: 'name', label: 'Name' },
        { key: 'lastName', label: 'Last Name' },
        { key: 'actions', label: 'Actions', width: '180px', align: 'right' },
    ];

    data: Row[] = [];

    ngOnInit(): void {
        this.loadUsers();
    }

    private loadUsers(): void {
        this.loading = true;
        this.errorMsg = '';

        this.userService.getAll().subscribe({
            next: (users) => {
                this.data = (users ?? [])
                    .map((user: any) => ({
                        id: user.id ?? user.Id ?? '',
                        name: user.name ?? user.Name ?? '',
                        lastName: user.lastName ?? user.LastName ?? '',
                    }))
                    .filter(row => !!row.id);
                this.loading = false;
            },
            error: (error) => {
                console.error(error);
                this.errorMsg = 'No se pudieron cargar los usuarios.';
                this.loading = false;
            }
        });
    }

    onDelete(row: Row): void {
        this.selectedUserId = row.id;
        this.showConfirm = true;
    }

    onConfirmDelete(result: boolean) {
    this.showConfirm = false;

    if (!result || !this.selectedUserId) {
        return;
    }

    this.deletingId = this.selectedUserId;

    this.userService.remove(this.selectedUserId).subscribe({
        next: () => {
            this.data = this.data.filter(item => item.id !== this.selectedUserId);
            this.deletingId = null;
            this.selectedUserId = null;

            this.messageService.show('User successfully deleted!', 'success');
        },
        error: () => {
            this.deletingId = null;
            this.messageService.show('Could not delete the user.', 'error');
        }
    });
}

}
