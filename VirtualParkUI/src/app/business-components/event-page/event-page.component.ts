import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { ButtonsComponent } from '../../components/buttons/buttons.component';
import { EventModel } from '../../../backend/services/event/models/EventModel';
import { EventService } from '../../../backend/services/event/event.service';

@Component({
  selector: 'app-event-page',
  standalone: true,
  imports: [CommonModule, ButtonsComponent],
  templateUrl: './event-page.component.html',
  styleUrls: ['./event-page.component.css']
})
export class EventPageComponent implements OnInit {
  events: EventModel[] = [];
  loading = false;
  error = '';

  constructor(private eventSvc: EventService, private router: Router) {}

  ngOnInit(): void {
    this.loadEvents();
  }

  loadEvents(): void {
    this.loading = true;
    this.eventSvc.getAll().subscribe({
      next: list => {
        this.events = list;
        this.loading = false;
      },
      error: err => {
        this.error = err.message;
        this.loading = false;
      }
    });
  }

  create(): void {
    this.router.navigate(['/events/new']);
  }

  edit(id: string): void {
    this.router.navigate(['/events/edit', id]);
  }

/*   remove(id: string): void {
    if (!confirm('¿Eliminar este evento?')) return;
    this.eventSvc.delete(id).subscribe({
      next: () => this.loadEvents(),
      error: err => alert(`Error eliminando: ${err.message}`)
    });
  } */
}
