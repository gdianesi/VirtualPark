import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { Observable, map } from 'rxjs';
import { ButtonsComponent } from '../../components/buttons/buttons.component';
import { TicketService } from '../../../backend/services/ticket/ticket.service';
import { EventService } from '../../../backend/services/event/event.service';
import { MessageComponent } from '../../components/messages/message.component';
import { MessageService } from '../../components/messages/service/message.service';

type EventOption = {
  id: string;
  label: string;
  soldOut: boolean;
};

@Component({
  selector: 'app-ticket-register-page',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink, ButtonsComponent, MessageComponent],
  templateUrl: './ticket-register-page.component.html',
  styleUrls: ['./ticket-register-page.component.css']
})
export class TicketRegisterPageComponent {
  private fb = inject(FormBuilder);
  private ticketService = inject(TicketService);
  private eventService = inject(EventService);
  private router = inject(Router);
  private messageService = inject(MessageService);

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

  get f() { return this.form.controls; }

  trackById = (_: number, e: { id: string }) => e.id;

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
      next: (res) => {
        alert(`Ticket created`);
        this.router.navigate(['/ticket']);
      },
      error: (e) => {
        const backendMsg = e?.error?.message ?? 'Unknown error';
        this.messageService.show(backendMsg, 'error');
      }
    });
  }

}
