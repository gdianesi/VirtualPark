import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { Observable, map } from 'rxjs';
import { ButtonsComponent } from '../../components/buttons/buttons.component';
import { TicketService } from '../../../backend/services/ticket/ticket.service';
import { EventService } from '../../../backend/services/event/event.service';
import { MessageComponent } from '../../components/messages/message.component';
import { MessageService } from '../../../backend/services/message/message.service';
import { ClockService } from '../../../backend/services/clock/clock.service';
import { ConfirmDialogComponent } from '../../components/confirm-dialog/confirm-dialog.component';

type EventOption = {
  id: string;
  label: string;
  soldOut: boolean;
};

@Component({
  selector: 'app-ticket-register-page',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink, ButtonsComponent, MessageComponent, ConfirmDialogComponent],
  templateUrl: './ticket-register-page.component.html',
  styleUrls: ['./ticket-register-page.component.css']
})
export class TicketRegisterPageComponent implements OnInit {
  private fb = inject(FormBuilder);
  private ticketService = inject(TicketService);
  private eventService = inject(EventService);
  private router = inject(Router);
  private messageService = inject(MessageService);
  private clockService = inject(ClockService);

  events$: Observable<EventOption[]> = this.eventService.getAll().pipe(
    map(events => {
      const today = new Date().setHours(0,0,0,0);

      return events
        .filter(e => new Date(e.date).getTime() >= today)

        .sort((a, b) => new Date(a.date).getTime() - new Date(b.date).getTime())

        .map(e => ({
          id: e.id,
          label: `${e.name} (${e.date}) - ${e.ticketsSold}/${e.capacity}`,
          soldOut: Number(e.ticketsSold) >= Number(e.capacity)
        }));
    })
  );


  form = this.fb.group({
    date: ['', [Validators.required]],
    eventId: ['']
  });
  showConfirm = false;

  ngOnInit(): void {
    this.loadClockDate();
  }

  get f() { return this.form.controls; }

  trackById = (_: number, e: { id: string }) => e.id;

  confirmSubmit(): void {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    this.showConfirm = true;
  }

  handleConfirm(value: boolean): void {
    this.showConfirm = false;
    if (value) {
      this.submit();
    }
  }

  submit() {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }

    const visitorId = localStorage.getItem('visitorId')!;

    if (!visitorId) { console.warn('visitorId no encontrado en localStorage'); return; }

    const v = this.form.value as { date: string; eventId?: string };

    const date = /^\d{4}-\d{2}-\d{2}$/.test(v.date)
      ? v.date
      : new Date(v.date).toISOString().slice(0, 10);

    const isEvent = !!(v.eventId && v.eventId.trim().length);
    const Type = isEvent ? 'Event' : 'General';

    const payload = {
      visitorId,
      Type,
      Date: date,
      eventId: v.eventId && v.eventId.trim().length ? v.eventId.trim() : null
    };

    this.ticketService.create(payload).subscribe({
      next: () => {
        this.messageService.show('Ticket created successfully!', 'success');
        setTimeout(() => this.router.navigate(['/ticket']), 1500);
      },
      error: (e) => {
        const backendMsg = e?.error?.message ?? 'Unknown error';
        this.messageService.show(backendMsg, 'error');
      }
    });
  }

  private loadClockDate(): void {
    this.clockService.get().subscribe({
      next: clock => {
        const normalized = this.normalizeDate(clock?.dateSystem ?? null);
        if (normalized) {
          this.form.patchValue({ date: normalized });
        }
      },
      error: () => { /* ignore */ }
    });
  }

  private normalizeDate(value: string | null): string | null {
    if (!value) return null;
    const date = new Date(value);
    if (Number.isNaN(date.getTime())) return null;
    return date.toISOString().slice(0, 10);
  }
}
