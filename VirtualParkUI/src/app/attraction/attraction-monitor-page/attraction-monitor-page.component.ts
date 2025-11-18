import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { TableColumn, TableComponent } from '../../components/table/generic-table.component';
import { ButtonsComponent } from '../../components/buttons/buttons.component';
import { AttractionService } from '../../../backend/services/attraction/attraction.service';
import { MessageComponent } from '../../components/messages/message.component';
import { MessageService } from '../../../backend/services/message/message.service'
import { AuthRoleService } from '../../../backend/services/auth/auth-role.service';
import { VisitRegistrationService } from '../../../backend/services/visitRegistration/visit-registration.service';

type Row = {
    id: string;
    name: string;
    type: string;
    capacity: string;
    currentVisitors: string;
};

@Component({
    selector: 'app-attraction-monitor-page',
    standalone: true,
    imports: [CommonModule, TableComponent, ButtonsComponent, MessageComponent],
    templateUrl: './attraction-monitor-page.component.html',
    styleUrls: ['./attraction-monitor-page.component.css']
})
export class AttractionMonitorPageComponent implements OnInit {
    private attractionService = inject(AttractionService);
    private router = inject(Router);
    private messageService = inject(MessageService);
    private authRole = inject(AuthRoleService);
    private visitRegistrationService = inject(VisitRegistrationService);

    loading = false;
    emptyMessage = 'No attractions available.';

    columns: TableColumn<Row>[] = [
        { key: 'name', label: 'Name' },
        { key: 'type', label: 'Type', width: '160px', align: 'center' },
        { key: 'capacity', label: 'Capacity', width: '120px', align: 'center' },
        { key: 'currentVisitors', label: 'Current Visitors', width: '160px', align: 'center' },
        { key: 'actions', label: 'Actions', width: '140px', align: 'center' },
    ];

    data: Row[] = [];

    ngOnInit(): void {
        if (!this.canMonitor()) {
            this.messageService.show('You do not have permission to view this section.', 'error');
            return;
        }
        this.loadAttractions();
    }

    canMonitor(): boolean {
        return this.authRole.hasAnyRole(['Operator']);
    }

    private loadAttractions(): void {
        this.loading = true;
        this.attractionService.getAll().subscribe({
            next: items => {
                this.data = (items ?? []).map(it => ({
                    id: it.id ?? (it as any).Id ?? '',
                    name: it.name ?? (it as any).Name ?? '',
                    type: this.formatType(it.type ?? (it as any).Type),
                    capacity: String(it.capacity ?? (it as any).Capacity ?? '—'),
                    currentVisitors: String((it as any).currentVisitors ?? (it as any).CurrentVisitors ?? '0'),
                }));
                if (!this.data.length) {
                    this.emptyMessage = 'No attractions were found.';
                } else {
                    this.refreshVisitorCounts();
                }
                this.loading = false;
            },
            error: () => {
                this.loading = false;
                this.emptyMessage = 'Unable to load attractions.';
                this.messageService.show('Unable to load attractions for monitoring.', 'error');
            }
        });
    }

    onDetails(row: Row): void {
        this.router.navigate(['/attraction/monitor', row.id]);
    }

    private formatType(type: any): string {
        const typeMap: Record<string, string> = {
            RollerCoaster: 'Roller Coaster',
            Simulator: 'Simulator',
            Show: 'Show',
        };

        const normalized = String(type ?? '').trim();
        const resolved = typeMap[normalized] ?? normalized;
        return resolved || '—';
    }

    private refreshVisitorCounts(): void {
        this.data.forEach(row => {
            if (!row.id) return;
            this.visitRegistrationService.getVisitorsInAttraction(row.id).subscribe({
                next: visitors => {
                    const value = String((visitors ?? []).length);
                    this.data = this.data.map(current =>
                        current.id === row.id ? { ...current, currentVisitors: value } : current
                    );
                },
                error: () => { /* ignore count errors */ }
            });
        });
    }
}
