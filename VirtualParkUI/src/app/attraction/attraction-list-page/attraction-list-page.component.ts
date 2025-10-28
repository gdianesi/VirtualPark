import { Component, OnInit, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { TableColumn, TableComponent } from '../../components/table/table.component';
import { AttractionService } from '../../../backend/services/attraction/attraction.service';

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
    imports: [TableComponent, RouterLink],
    templateUrl: './attraction-list-page.component.html',
    styleUrls: ['./attraction-list-page.component.css'],
})
export class AttractionListPageComponent implements OnInit {
    private service = inject(AttractionService);

    loading = true;
    errorMsg = '';

    columns: TableColumn<Row>[] = [
        { key: 'name', label: 'Name' },
        { key: 'type', label: 'Type', width: '140px', align: 'center' },
        { key: 'miniumAge', label: 'Min. Age', width: '110px', align: 'center' },
        { key: 'capacity', label: 'Capacity', width: '120px', align: 'center' },
        { key: 'description', label: 'Description' },
        { key: 'available', label: 'Available', width: '120px', align: 'center' },
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
                this.errorMsg = 'No se pudieron cargar las atracciones.';
                console.error(e);
                this.loading = false;
            }
        });
    }
}
