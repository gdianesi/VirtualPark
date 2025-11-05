import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { finalize } from 'rxjs';
import { ClockService } from '../../../backend/services/clock/clock.service';
import { ButtonsComponent } from '../../components/buttons/buttons.component';

@Component({
    selector: 'app-clock-register-page',
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule, ButtonsComponent],
    templateUrl: './clock-register-page.component.html',
    styleUrls: ['./clock-register-page.component.css']
})
export class ClockRegisterPageComponent implements OnInit {
    private fb = inject(FormBuilder);
    private clockService = inject(ClockService);

    form = this.fb.group({
        dateSystem: ['', [Validators.required]]
    });

    isLoading = false;
    isSaving = false;
    errorMessage: string | null = null;
    originalValue: string | null = null;

    ngOnInit(): void {
        this.loadClock();
    }

    get saveDisabled(): boolean {
        if (this.isLoading || this.isSaving || this.form.invalid) return true;
        const current = this.currentNormalizedValue();
        if (!current) return true;
        return current === this.originalValue;
    }

    get currentDisplayValue(): string {
        const current = this.currentNormalizedValue();
        return current ?? '';
    }

    loadClock(): void {
        this.isLoading = true;
        this.errorMessage = null;
        this.clockService.get()
            .pipe(finalize(() => { this.isLoading = false; }))
            .subscribe({
                next: clock => {
                    const normalized = this.normalize(clock?.dateSystem ?? '');
                    this.originalValue = normalized;
                    const inputValue = this.toInputValue(normalized);
                    this.form.setValue({ dateSystem: inputValue });
                    this.form.markAsPristine();
                },
                error: err => {
                    console.error('Error fetching clock', err);
                    this.errorMessage = 'No se pudo obtener la hora del sistema.';
                }
            });
    }

    save(): void {
        if (this.saveDisabled) return;

        const normalized = this.currentNormalizedValue();
        if (!normalized) return;

        this.isSaving = true;
        this.errorMessage = null;
        this.clockService.update({ dateSystem: normalized })
            .pipe(finalize(() => { this.isSaving = false; }))
            .subscribe({
                next: () => {
                    this.originalValue = normalized;
                    alert('Hora del sistema actualizada.');
                    const inputValue = this.toInputValue(normalized);
                    this.form.setValue({ dateSystem: inputValue });
                    this.form.markAsPristine();
                },
                error: err => {
                    console.error('Error updating clock', err);
                    this.errorMessage = 'No se pudo guardar la hora.';
                }
            });
    }

    private currentNormalizedValue(): string | null {
        const raw = this.form.get('dateSystem')?.value ?? '';
        const normalized = this.normalize(raw);
        return normalized || null;
    }

    private normalize(value: string | null | undefined): string {
        if (!value) return '';
        const trimmed = value.trim();
        if (!trimmed) return '';

        const secondsPattern = /^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}$/;
        const minutesPattern = /^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}$/;
        const datePattern = /^\d{4}-\d{2}-\d{2}$/;

        if (secondsPattern.test(trimmed)) return trimmed;
        if (minutesPattern.test(trimmed)) return `${trimmed}:00`;
        if (datePattern.test(trimmed)) return `${trimmed}T00:00:00`;

        const parsed = new Date(trimmed);
        if (!isNaN(parsed.getTime())) {
            const year = parsed.getFullYear();
            const month = `${parsed.getMonth() + 1}`.padStart(2, '0');
            const day = `${parsed.getDate()}`.padStart(2, '0');
            const hours = `${parsed.getHours()}`.padStart(2, '0');
            const minutes = `${parsed.getMinutes()}`.padStart(2, '0');
            const seconds = `${parsed.getSeconds()}`.padStart(2, '0');
            return `${year}-${month}-${day}T${hours}:${minutes}:${seconds}`;
        }

        return trimmed;
    }

    private toInputValue(value: string | null | undefined): string {
        if (!value) return '';
        const trimmed = value.trim();
        if (!trimmed) return '';

        if (/^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}/.test(trimmed)) {
            return trimmed.slice(0, 16);
        }

        if (/^\d{4}-\d{2}-\d{2}$/.test(trimmed)) {
            return `${trimmed}T00:00`;
        }

        return trimmed;
    }
}
