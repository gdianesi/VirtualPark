import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TableColumn, TableComponent } from '../../components/table/generic-table.component';
import { ButtonsComponent } from '../../components/buttons/buttons.component';
import { VisitRegistrationService } from '../../../backend/services/visitRegistration/visit-registration.service';
import { AttractionModel } from '../../../backend/services/attraction/models/AttractionModel';
import { SessionService } from '../../../backend/services/session/session.service';
import { AttractionService } from '../../../backend/services/attraction/attraction.service';
import { TicketService } from '../../../backend/services/ticket/ticket.service';
import { TicketModel } from '../../../backend/services/ticket/models/TicketModel';
import { ClockService } from '../../../backend/services/clock/clock.service';
import { MessageService } from '../../components/messages/service/message.service';
import { MessageComponent } from '../../components/messages/message.component';

type Row = {
    id: string;
    name: string;
    type: string;
    minAge: string;
    capacity: string;
    description: string;
};

@Component({
    selector: 'app-attraction-up-attraction-page',
    standalone: true,
    imports: [CommonModule, TableComponent, ButtonsComponent, MessageComponent],
    templateUrl: './attraction-upAttraction-page.component.html',
    styleUrls: ['./attraction-upAttraction-page.component.css']
})
export class AttractionUpAttractionPageComponent implements OnInit {
    loading = false;
    processingId: string | null = null;
    emptyMessage = 'No attractions available right now.';

    visitorId: string | null = null;
    private ticketQrId: string | null = null;
    private clockDateValue: number | null = null;
    columns: TableColumn<Row>[] = [
        { key: 'name', label: 'Name' },
        { key: 'type', label: 'Type', width: '140px' },
        { key: 'minAge', label: 'Min. Age', width: '120px', align: 'center' },
        { key: 'capacity', label: 'Capacity', width: '120px', align: 'center' },
        { key: 'actions', label: 'Up', width: '200px', align: 'center' },
    ];

    data: Row[] = [];

    constructor(
        private visitRegistrationService: VisitRegistrationService,
        private sessionService: SessionService,
        private attractionService: AttractionService,
        private ticketService: TicketService,
        private clockService: ClockService,
        private messageService: MessageService
    ) { }

    ngOnInit(): void {
        this.ensureVisitorId();
    }

    onRideNfc(row: Row): void {
        if (!this.visitorId) {
            this.messageService.show('No active visit found for today.', 'error');
            return;
        }
        this.processingId = row.id;
        this.attractionService.validateEntryByNfc(row.id, this.visitorId).subscribe({
            next: res => {
                if (!res?.isValid) {
                    this.processingId = null;
                    this.messageService.show('Entry could not be validated. Please verify requirements.', 'error');
                    return;
                }
                this.registerRide(row);
            },
            error: () => {
                this.processingId = null;
                this.messageService.show('Unable to validate NFC access.', 'error');
            }
        });
    }

    onRideQr(row: Row): void {
        if (!this.visitorId) {
            this.messageService.show('No active visit found for today.', 'error');
            return;
        }
        if (!this.ticketQrId) {
            this.messageService.show('No QR ticket was found for this visitor.', 'error');
            return;
        }
        this.processingId = row.id;
        this.attractionService.validateEntryByQr(row.id, this.ticketQrId).subscribe({
            next: res => {
                if (!res?.isValid) {
                    this.processingId = null;
                    this.messageService.show('Entry could not be validated. Please verify requirements.', 'error');
                    return;
                }
                this.registerRide(row);
            },
            error: () => {
                this.processingId = null;
                this.messageService.show('Unable to validate QR access.', 'error');
            }
        });
    }

    private registerRide(row: Row): void {
        if (!this.visitorId) {
            this.messageService.show('No active visit found for today.', 'error');
            return;
        }
        this.visitRegistrationService.upToAttraction(this.visitorId, row.id).subscribe({
            next: () => {
                this.processingId = null;
                this.messageService.show(`You boarded ${row.name}.`, 'success');
            },
            error: () => {
                this.processingId = null;
                this.messageService.show('Unable to register the boarding. Please try again.', 'error');
            }
        });
    }

    private ensureVisitorId(): void {
        const storedVisitor = localStorage.getItem('visitorId');
        if (storedVisitor) {
            this.visitorId = storedVisitor;
            this.initializeVisitorContext();
            return;
        }

        const token = localStorage.getItem('token');
        if (!token) {
            this.messageService.show('Sign in to view your available attractions.', 'error');
            return;
        }

        this.loading = true;
        this.sessionService.getSession(token).subscribe({
            next: res => {
                this.visitorId = res?.visitorId ?? null;
                this.loading = false;
                if (!this.visitorId) {
                    this.messageService.show('No visitor was associated with this session.', 'error');
                    return;
                }
                localStorage.setItem('visitorId', this.visitorId);
                this.initializeVisitorContext();
            },
            error: () => {
                this.loading = false;
                this.messageService.show('Unable to retrieve the user session.', 'error');
            }
        });
    }

    private loadAttractions(): void {
        if (!this.visitorId) return;
        this.loading = true;
        this.emptyMessage = 'No attractions available right now.';
        this.visitRegistrationService.getAvailableAttractions(this.visitorId).subscribe({
            next: (items: AttractionModel[]) => {
                this.data = (items ?? []).map(this.mapRow);
                this.loading = false;
                if (!this.data.length) {
                    this.emptyMessage = 'There are no attractions available today.';
                    this.messageService.show('There are no attractions available today.', 'info');
                }
            },
            error: () => {
                this.loading = false;
                this.emptyMessage = 'Unable to load the available attractions.';
                this.messageService.show('Unable to load the available attractions.', 'error');
            }
        });
    }

    private initializeVisitorContext(): void {
        if (!this.visitorId) return;
        this.loadClockDate(() => this.loadTicketQr());
        this.loadAttractions();
    }

    private loadTicketQr(): void {
        if (!this.visitorId) return;
        this.ticketService.getByVisitor(this.visitorId).subscribe({
            next: (tickets: TicketModel[]) => {
                const todayTicket = this.selectBestTicket(tickets ?? []);
                this.ticketQrId = todayTicket?.QrId ?? (todayTicket as any)?.qrId ?? null;
            },
            error: () => {
                this.ticketQrId = null;
            }
        });
    }

    private loadClockDate(onReady?: () => void): void {
        this.clockService.get().subscribe({
            next: clock => {
                this.clockDateValue = this.normalizeDateString(clock?.dateSystem ?? null);
                onReady?.();
            },
            error: () => {
                this.clockDateValue = null;
                onReady?.();
            }
        });
    }

    private selectBestTicket(tickets: TicketModel[]): TicketModel | null {
        if (!tickets.length) return null;

        const normalized = tickets
            .map(ticket => ({ ticket, value: this.normalizeTicketDate(ticket) }))
            .filter(item => item.value !== null)
            .sort((a, b) => (a.value! - b.value!));

        if (!normalized.length) {
            return tickets[0];
        }

        const targetValue = this.clockDateValue ?? this.startOfDayValue(new Date());
        const exact = normalized.find(item => item.value === targetValue);
        if (exact) return exact.ticket;

        const beforeTarget = [...normalized].reverse().find(item => item.value! <= targetValue);
        if (beforeTarget) return beforeTarget.ticket;

        return normalized[0].ticket;
    }

    private normalizeTicketDate(ticket: TicketModel): number | null {
        const raw = (ticket.Date ?? (ticket as any)?.date ?? '').toString();
        return this.normalizeDateString(raw);
    }

    private normalizeDateString(value: string | null): number | null {
        if (!value) return null;
        const datePart = value.split('T')[0];
        const segments = datePart.split('-');
        if (segments.length !== 3) return null;
        const [yearStr, monthStr, dayStr] = segments;
        const year = Number(yearStr);
        const month = Number(monthStr);
        const day = Number(dayStr);
        if (!Number.isInteger(year) || !Number.isInteger(month) || !Number.isInteger(day)) {
            return null;
        }
        return Date.UTC(year, month - 1, day);
    }

    private startOfDayValue(date: Date): number {
        return Date.UTC(date.getFullYear(), date.getMonth(), date.getDate());
    }

    private mapRow = (attraction: AttractionModel): Row => {
        const format = (value: any) => (value ?? '').toString();
        return {
            id: attraction.id ?? (attraction as any).Id ?? '',
            name: attraction.name ?? (attraction as any).Name ?? '',
            type: this.formatType(attraction.type ?? (attraction as any).Type),
            minAge: format(attraction.miniumAge ?? (attraction as any).MiniumAge ?? '—'),
            capacity: format(attraction.capacity ?? (attraction as any).Capacity ?? '—'),
            description: attraction.description ?? (attraction as any).Description ?? '—',
        };
    };

    private formatType(type: any): string {
        const numberLabels: Record<number, string> = {
            0: 'Roller Coaster',
            1: 'Simulator',
            2: 'Show',
        };

        if (typeof type === 'number' && numberLabels[type] !== undefined) {
            return numberLabels[type];
        }

        const normalized = String(type ?? '').trim();
        if (!normalized) return '—';

        const textLabels: Record<string, string> = {
            RollerCoaster: 'Roller Coaster',
            Simulator: 'Simulator',
            Show: 'Show',
        };

        return textLabels[normalized] ?? normalized;
    }
}
