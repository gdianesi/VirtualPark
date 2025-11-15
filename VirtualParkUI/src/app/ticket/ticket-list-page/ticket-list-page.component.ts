import { Component, OnInit, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { TableColumn, TableComponent } from '../../components/table/generic-table.component';
import { ButtonsComponent } from '../../components/buttons/buttons.component';
import { TicketService } from '../../../backend/services/ticket/ticket.service';
import { EventService } from '../../../backend/services/event/event.service';

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

    private ticketService = inject(TicketService);
    private eventService = inject(EventService);

    loading = true;
    errorMsg = '';
    data: Row[] = [];
    eventMap: Record<string, string> = {};

    columns: TableColumn<Row>[] = [];

    private shortenQr(qr: string): string {
        return qr.substring(0, 8) + '...';
    }

    ngOnInit(): void {
        this.loading = true;

        const roles = JSON.parse(localStorage.getItem('roles') ?? '[]');
        const visitorId = localStorage.getItem('visitorId');

        const isVisitor = roles.includes('Visitor');
        const isAdmin = roles.includes('Administrator');

        if (isVisitor) {
            this.columns = [
                { key: 'type', label: 'Type', width: '120px', align: 'center' },
                { key: 'date', label: 'Date', width: '130px', align: 'center' },
                { key: 'event', label: 'Event', width: '180px' },
                { key: 'qr', label: 'QR', width: '240px' },
            ];
        } else {
            this.columns = [
                { key: 'type', label: 'Type', width: '120px', align: 'center' },
                { key: 'date', label: 'Date', width: '130px', align: 'center' },
                { key: 'event', label: 'Event', width: '180px' },
                { key: 'visitor', label: 'Visitor', width: '220px' },
                { key: 'qr', label: 'QR', width: '240px' },
            ];
        }

        this.eventService.getAll().subscribe({
            next: events => {
                events.forEach(ev => this.eventMap[ev.id] = ev.name);

                const request$ = isVisitor
                    ? this.ticketService.getByVisitor(visitorId!)
                    : this.ticketService.getAll();

                request$.subscribe({
                    next: (items: any[]) => {
                        this.data = (items ?? []).map(it => {
                            
                            const eventId = it.eventId ?? it.EventId ?? null;
                            const eventName = eventId && this.eventMap[eventId]
                                ? this.eventMap[eventId]
                                : '—';

                            return {
                                id: it.id ?? it.Id,
                                type: it.type ?? it.Type ?? '',
                                date: it.date ?? it.Date ?? '',

                                visitor: isVisitor
                                    ? ''
                                    : (it.visitorId ?? it.VisitorId ?? ''),

                                event: isVisitor
                                    ? eventName
                                    : (eventId ?? '—'),

                                qr: isVisitor
                                    ? this.shortenQr(it.qrId ?? it.QrId ?? '')
                                    : (it.qrId ?? it.QrId ?? ''),
                            };
                        });

                        this.loading = false;
                    },
                    error: (e) => {
                        console.error(e);
                        this.errorMsg = 'The tickets could not be loaded.';
                        this.loading = false;
                    }
                });
            },

            error: e => {
                console.error(e);
                this.errorMsg = 'Could not load events.';
                this.loading = false;
            }
        });
    }
}
