import { Component, OnInit, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { TableColumn, TableComponent } from '../../components/table/generic-table.component';
import { AttractionService } from '../../../backend/services/attraction/attraction.service';
import { ButtonsComponent } from '../../components/buttons/buttons.component';
import { AuthRoleService } from '../../auth-role/auth-role.service';
import { MessageService } from '../../components/messages/service/message.service';
import { ConfirmDialogComponent } from '../../components/confirm-dialog/confirm-dialog.component';

type Row = {
    name: string;
    type: string;
    miniumAge: string;
    capacity: string;
    description: string;
    available: string;
    id?: string;
};

@Component({
    selector: 'app-attraction-list-page',
    standalone: true,
    imports: [CommonModule, TableComponent, RouterLink, ButtonsComponent,  ConfirmDialogComponent],
    templateUrl: './attraction-list-page.component.html',
    styleUrls: ['./attraction-list-page.component.css'],
})
export class AttractionListPageComponent implements OnInit {
    private service = inject(AttractionService);
    private authRole = inject(AuthRoleService);
    private messageService = inject(MessageService);

    showConfirm = false;
    selectedId: string | null = null;
    loading = true;
    errorMsg = '';
    deletingId: string | null = null;

    columns: TableColumn<Row>[] = [
        { key: 'name', label: 'Name' },
        { key: 'type', label: 'Type', width: '140px', align: 'center' },
        { key: 'miniumAge', label: 'Min. Age', width: '110px', align: 'center' },
        { key: 'capacity', label: 'Capacity', width: '120px', align: 'center' },
        { key: 'description', label: 'Description' },
        { key: 'available', label: 'Available', width: '120px', align: 'center' },
        { key: 'actions',    label: 'Configuration',     width: '140px', align: 'right' },
    ];

    data: Row[] = [];

    ngOnInit(): void {

        this.service.getAll().subscribe({
            next: (items: any[]) => {
                this.data = items.map(it => ({
                    id: it.id ?? it.Id,
                    name: it.name ?? it.Name ?? '',
                    type: it.type ?? it.Type ?? '',
                    miniumAge: String(it.miniumAge ?? it.MiniumAge ?? ''),
                    capacity: String(it.capacity ?? it.Capacity ?? ''),
                    description: it.description ?? it.Description ?? '',
                    available: ((it.available ?? it.Available ?? '') + '').toString().toLowerCase(),
                }));
                this.loading = false;
            },
            error: (e) => {
                console.error(e);
                this.errorMsg = 'No se pudieron cargar las atracciones.';
                this.loading = false;
            }
        });
    }
    
    canEdit(): boolean {
        return this.authRole.hasAnyRole(['Administrator', 'Operator']);
    }

    onDeleteClick(id: string) {
        this.selectedId = id;
        this.showConfirm = true;
    }

    onConfirmDelete(result: boolean) {
        this.showConfirm = false;

        if (!result || !this.selectedId) return;

        this.deletingId = this.selectedId;

        this.service.remove(this.selectedId).subscribe({
            next: () => {
            this.data = this.data.filter(a => a.id !== this.selectedId);

            this.messageService.show('Attraction successfully deleted!', 'success');

            this.deletingId = null;
            this.selectedId = null;
            },
            error: () => {
            this.messageService.show('Failed to delete attraction.', 'error');
            this.deletingId = null;
            }
        });
    }
}
