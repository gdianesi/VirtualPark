import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { ButtonsComponent } from '../../components/buttons/buttons.component';
import { TicketService } from '../../../backend/services/ticket/ticket.service';

@Component({
  selector: 'app-ticket-register-page',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink, ButtonsComponent],
  templateUrl: './ticket-register-page.component.html',
  styleUrls: ['./ticket-register-page.component.css']
})
export class TicketRegisterPageComponent {
  private fb = inject(FormBuilder);
  private service = inject(TicketService);

  form = this.fb.group({
    visitorId: ['', [Validators.required, Validators.maxLength(64)]],
    type: ['', [Validators.required]],
    date: ['', [Validators.required]],
    eventId: ['']
  });

  get f() { return this.form.controls; }

  submit() {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }

    const v = this.form.value as { visitorId: string; type: string; date: string; eventId?: string };

    const date = /^\d{4}-\d{2}-\d{2}$/.test(v.date)
      ? v.date
      : new Date(v.date).toISOString().slice(0, 10);

    const payload = {
      visitorId: v.visitorId.trim(),
      Type: v.type,
      Date: date,
      eventId: (v.eventId?.trim() || null)
    };

    this.service.create(payload).subscribe({
      next: (res) => alert(`Ticket creado (id: ${res.id})`),
      error: (e) => alert('Error creando ticket: ' + (e?.message ?? e))
    });
  }
}
