import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { TableColumn, TableComponent } from '../../components/table/generic-table.component';
import { UserService } from '../../../backend/services/user/user.service';
import { ButtonsComponent } from '../../components/buttons/buttons.component';

type Row = {
    id: string;
    name: string;
    lastName: string;
};

@Component({
    selector: 'app-user-list-page',
    standalone: true,
    imports: [CommonModule, TableComponent, RouterLink, ButtonsComponent],
    templateUrl: './user-list-page.component.html',
    styleUrls: ['./user-list-page.component.css']
})
export class UserListPageComponent implements OnInit {
    private userService = inject(UserService);

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
        if (this.deletingId || !row?.id) {
            return;
        }

        this.deletingId = row.id;
        this.userService.remove(row.id).subscribe({
            next: () => {
                this.data = this.data.filter(item => item.id !== row.id);
                this.deletingId = null;
            },
            error: (error) => {
                console.error(error);
                this.errorMsg = 'No se pudo eliminar el usuario.';
                this.deletingId = null;
            }
        });
    }
}
