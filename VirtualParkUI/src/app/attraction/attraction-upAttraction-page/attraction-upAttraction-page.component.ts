import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TableColumn, TableComponent } from '../../components/table/generic-table.component';
import { ButtonsComponent } from '../../components/buttons/buttons.component';
import { VisitRegistrationService } from '../../../backend/services/visitRegistration/visit-registration.service';
import { AttractionModel } from '../../../backend/services/attraction/models/AttractionModel';
import { SessionService } from '../../../backend/services/session/session.service';
import { AttractionService } from '../../../backend/services/attraction/attraction.service';

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
    imports: [CommonModule, TableComponent, ButtonsComponent],
    templateUrl: './attraction-upAttraction-page.component.html',
    styleUrls: ['./attraction-upAttraction-page.component.css']
})
export class AttractionUpAttractionPageComponent implements OnInit {
    loading = false;
    processingId: string | null = null;
    downLoading = false;
    errorMessage = '';
    successMessage = '';

    visitorId: string | null = null;
    columns: TableColumn<Row>[] = [
        { key: 'name', label: 'Name' },
        { key: 'type', label: 'Type', width: '140px' },
        { key: 'minAge', label: 'Min. Age', width: '120px', align: 'center' },
        { key: 'capacity', label: 'Capacity', width: '120px', align: 'center' },
        { key: 'actions', label: 'Up / Down', width: '200px', align: 'right' },
    ];

    data: Row[] = [];

    constructor(
        private visitRegistrationService: VisitRegistrationService,
        private sessionService: SessionService,
        private attractionService: AttractionService
    ) { }

    ngOnInit(): void {
        this.ensureVisitorId();
    }

    onRideNfc(row: Row): void {
        if (!this.visitorId) {
            this.errorMessage = 'No se encontró una visita activa para hoy.';
            return;
        }
        this.processingId = row.id;
        this.errorMessage = '';
        this.successMessage = '';
        this.attractionService.validateEntryByNfc(row.id, this.visitorId).subscribe({
            next: res => {
                if (!res?.isValid) {
                    this.processingId = null;
                    this.errorMessage = 'El acceso por NFC no es válido.';
                    return;
                }
                this.registerRide(row);
            },
            error: () => {
                this.processingId = null;
                this.errorMessage = 'No se pudo validar el acceso por NFC.';
            }
        });
    }

    onRideQr(row: Row): void {
        if (!this.visitorId) {
            this.errorMessage = 'No se encontró una visita activa para hoy.';
            return;
        }
        const qrId = prompt('Escanea o ingresa el código del QR:')?.trim();
        if (!qrId) {
            this.errorMessage = 'Debes ingresar un código QR válido.';
            return;
        }
        this.processingId = row.id;
        this.errorMessage = '';
        this.successMessage = '';
        this.attractionService.validateEntryByQr(row.id, qrId).subscribe({
            next: res => {
                if (!res?.isValid) {
                    this.processingId = null;
                    this.errorMessage = 'El QR presentado no es válido.';
                    return;
                }
                this.registerRide(row);
            },
            error: () => {
                this.processingId = null;
                this.errorMessage = 'No se pudo validar el acceso por QR.';
            }
        });
    }

    private registerRide(row: Row): void {
        if (!this.visitorId) {
            this.errorMessage = 'No se encontró una visita activa para hoy.';
            return;
        }
        this.visitRegistrationService.upToAttraction(this.visitorId, row.id).subscribe({
            next: () => {
                this.processingId = null;
                this.successMessage = `Te subiste a ${row.name}.`;
            },
            error: () => {
                this.processingId = null;
                this.errorMessage = 'No se pudo registrar la subida. Intenta nuevamente.';
            }
        });
    }

    onDown(): void {
        if (!this.visitorId) {
            this.errorMessage = 'No se encontró una visita activa para hoy.';
            return;
        }
        this.downLoading = true;
        this.errorMessage = '';
        this.successMessage = '';
        this.visitRegistrationService.downToAttraction(this.visitorId).subscribe({
            next: () => {
                this.downLoading = false;
                this.successMessage = 'Se registró la bajada correctamente.';
            },
            error: () => {
                this.downLoading = false;
                this.errorMessage = 'No se pudo registrar la bajada. Intenta nuevamente.';
            }
        });
    }

    private ensureVisitorId(): void {
        const storedVisitor = localStorage.getItem('visitorId');
        if (storedVisitor) {
            this.visitorId = storedVisitor;
            this.loadAttractions();
            return;
        }

        const token = localStorage.getItem('token');
        if (!token) {
            this.errorMessage = 'Inicia sesión para ver tus atracciones disponibles.';
            return;
        }

        this.loading = true;
        this.sessionService.getSession(token).subscribe({
            next: res => {
                this.visitorId = res?.visitorId ?? null;
                this.loading = false;
                if (!this.visitorId) {
                    this.errorMessage = 'No se encontró un visitante asociado.';
                    return;
                }
                localStorage.setItem('visitorId', this.visitorId);
                this.loadAttractions();
            },
            error: () => {
                this.loading = false;
                this.errorMessage = 'No se pudo obtener la sesión del usuario.';
            }
        });
    }

    private loadAttractions(): void {
        if (!this.visitorId) return;
        this.loading = true;
        this.errorMessage = '';
        this.successMessage = '';
        this.visitRegistrationService.getAvailableAttractions(this.visitorId).subscribe({
            next: (items: AttractionModel[]) => {
                this.data = (items ?? []).map(this.mapRow);
                this.loading = false;
                if (!this.data.length) {
                    this.successMessage = '';
                    this.errorMessage = 'No hay atracciones disponibles para hoy.';
                }
            },
            error: () => {
                this.loading = false;
                this.errorMessage = 'No se pudieron cargar las atracciones disponibles.';
            }
        });
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
        if (typeof type === 'string') return type;
        return String(type ?? '—');
    }
}
