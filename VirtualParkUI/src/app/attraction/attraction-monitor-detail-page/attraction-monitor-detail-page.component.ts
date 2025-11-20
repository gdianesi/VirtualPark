import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { TableColumn, TableComponent } from '../../components/table/generic-table.component';
import { ButtonsComponent } from '../../components/buttons/buttons.component';
import { VisitRegistrationService } from '../../../backend/services/visitRegistration/visit-registration.service';
import { VisitorInAttractionModel } from '../../../backend/services/visitRegistration/models/VisitorInAttractionModel';
import { VisitScoreRequest } from '../../../backend/services/visitRegistration/models/VisitScoreRequest';
import { MessageComponent } from '../../components/messages/message.component';
import { MessageService } from '../../../backend/services/message/message.service';
import { AttractionService } from '../../../backend/services/attraction/attraction.service';

type Row = {
    visitorProfileId: string;
    visitRegistrationId?: string;
    ticketType?: string;
    name: string;
    membership: string;
    score: string;
    nfcId: string;
};

@Component({
    selector: 'app-attraction-monitor-detail-page',
    standalone: true,
    imports: [CommonModule, RouterLink, TableComponent, ButtonsComponent, MessageComponent],
    templateUrl: './attraction-monitor-detail-page.component.html',
    styleUrls: ['./attraction-monitor-detail-page.component.css']
})
export class AttractionMonitorDetailPageComponent implements OnInit {
    private route = inject(ActivatedRoute);
    private visitRegistrationService = inject(VisitRegistrationService);
    private messageService = inject(MessageService);
    private attractionService = inject(AttractionService);

    attractionId: string | null = null;
    attractionName = '';
    loading = false;
    emptyMessage = 'No visitors are currently registered for this attraction.';
    capacity = 0;
    visitorCount = 0;
    availableSlots = 0;

    columns: TableColumn<Row>[] = [
        { key: 'name', label: 'Visitor' },
        { key: 'membership', label: 'Membership', width: '160px', align: 'center' },
        { key: 'score', label: 'Score', width: '120px', align: 'center' },
        { key: 'nfcId', label: 'NFC', width: '200px' },
        { key: 'actions', label: 'Actions', width: '140px', align: 'center' },
    ];

    data: Row[] = [];
    processingId: string | null = null;

    ngOnInit(): void {
        this.route.paramMap.subscribe(params => {
            this.attractionId = params.get('id');
            if (!this.attractionId) {
                this.messageService.show('Attraction identifier was not provided.', 'error');
                return;
            }
            this.loadAttractionInfo();
            this.loadVisitors();
        });
    }

    private loadAttractionInfo(): void {
        if (!this.attractionId) return;
        this.attractionService.getById(this.attractionId).subscribe({
            next: res => {
                this.attractionName = (res as any)?.name ?? (res as any)?.Name ?? '';
                const capacityValue = Number((res as any)?.capacity ?? (res as any)?.Capacity ?? 0);
                this.capacity = Number.isFinite(capacityValue) && capacityValue >= 0 ? capacityValue : 0;
                this.updateAvailability();
            },
            error: () => {
                this.attractionName = '';
                this.capacity = 0;
                this.updateAvailability();
            }
        });
    }

    private loadVisitors(): void {
        if (!this.attractionId) return;
        this.loading = true;
        this.visitRegistrationService.getVisitorsInAttraction(this.attractionId).subscribe({
            next: (visitors: VisitorInAttractionModel[]) => {
                this.data = (visitors ?? []).map(v => ({
                    visitorProfileId: v.visitorProfileId ?? (v as any).VisitorProfileId ?? '',
                    visitRegistrationId: v.visitRegistrationId ?? (v as any).VisitRegistrationId ?? '',
                    ticketType: String(v.ticketType ?? (v as any).TicketType ?? ''),
                    name: `${v.name ?? (v as any).Name ?? ''} ${v.lastName ?? (v as any).LastName ?? ''}`.trim(),
                    membership: this.formatMembership(v.membership ?? (v as any).Membership),
                    score: String(v.score ?? (v as any).Score ?? '0'),
                    nfcId: v.nfcId ?? (v as any).NfcId ?? '—',
                }));
                if (!this.data.length) {
                    this.emptyMessage = 'No visitors are currently registered for this attraction.';
                }
                this.updateAvailability();
                this.loading = false;
            },
            error: () => {
                this.loading = false;
                this.emptyMessage = 'Unable to load visitors for this attraction.';
                this.messageService.show('Unable to load visitors for this attraction.', 'error');
            }
        });
    }

    onDownVisitor(row: Row): void {
        if (!row.visitorProfileId) return;
        this.processingId = row.visitorProfileId;
        if (!row.visitRegistrationId) {
            this.executeDown(row.visitorProfileId);
            return;
        }

        const origin = this.resolveOrigin(row.ticketType);
        const payload: VisitScoreRequest = {
            visitRegistrationId: row.visitRegistrationId,
            origin,
            points: null,
        };

        this.visitRegistrationService.recordScoreEvent(payload).subscribe({
            next: () => {
                this.executeDown(row.visitorProfileId);
            },
            error: err => {
                this.processingId = null;
                const backendMsg =
                    err?.error?.message ??
                    err?.message ??
                    'Unable to register the score event.';
                this.messageService.show(backendMsg, 'error');
            }
        });
    }

    private executeDown(visitorProfileId: string): void {
        this.visitRegistrationService.downToAttraction(visitorProfileId).subscribe({
            next: () => {
                this.processingId = null;
                this.messageService.show('Visitor was removed from the attraction.', 'success');
                this.loadVisitors();
            },
            error: () => {
                this.processingId = null;
                this.messageService.show('Unable to remove the visitor from the attraction.', 'error');
            }
        });
    }

    private formatMembership(value: any): string {
        if (typeof value === 'string' && value.trim()) {
            return value;
        }

        const membershipMap: Record<number, string> = {
            0: 'Standard',
            1: 'Premium',
            2: 'VIP',
        };

        if (typeof value === 'number' && membershipMap[value] !== undefined) {
            return membershipMap[value];
        }

        const normalized = String(value ?? '').trim();
        return normalized || '—';
    }

    private updateAvailability(): void {
        this.visitorCount = this.data.length;
        this.availableSlots = this.capacity > 0
            ? Math.max(this.capacity - this.visitorCount, 0)
            : 0;
    }

    private resolveOrigin(ticketType?: string): string {
        const normalized = (ticketType ?? '').toString().trim().toLowerCase();
        if (normalized === 'event' || normalized === '1') {
            return 'Event';
        }
        return 'Attraction';
    }
}
