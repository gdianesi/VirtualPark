import { Component, OnInit, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { TableColumn, TableComponent } from '../../components/table/generic-table.component';
import { ButtonsComponent } from '../../components/buttons/buttons.component';
import { TicketService } from '../../../backend/services/ticket/ticket.service';

type Row = {
    type: string;
    date: string;
    event: string;
    visitor: string; 
    qr: string;
    id?: string;
};

@Component({
    selector: 'app-ticket-list-page',
    standalone: true,
    imports: [CommonModule, TableComponent, RouterLink, ButtonsComponent],
    templateUrl: './ticket-list-page.component.html',
    styleUrls: ['./ticket-list-page.component.css'],
})
export class TicketListPageComponent implements OnInit {
    private service = inject(TicketService);

    loading = true;
    errorMsg = '';
    data: Row[] = [];

    columns: TableColumn<Row>[] = [
        { key: 'type', label: 'Type', width: '120px', align: 'center' },
        { key: 'date', label: 'Date', width: '130px', align: 'center' },
        { key: 'event', label: 'Event', width: '180px' },
        { key: 'visitor', label: 'Visitor', width: '220px' },
        { key: 'qr', label: 'QR', width: '240px' },
    ];

    ngOnInit(): void {
        this.loading = true;
        this.service.getAll().subscribe({
            next: (items: any[]) => {
                this.data = (items ?? []).map(it => ({
                    id: it.id ?? it.Id,
                    type: it.type ?? it.Type ?? '',
                    date: it.date ?? it.Date ?? '',
                    event: (it.eventId ?? it.EventId ?? '') || '—',
                    visitor: it.visitorId ?? it.VisitorId ?? '—',
                    qr: it.qrId ?? it.QrId ?? '—',
                }));
                this.loading = false;
            },
            error: (e) => {
                console.error(e);
                this.errorMsg = 'No se pudieron cargar los tickets.';
                this.loading = false;
            }
        });
    }
}
