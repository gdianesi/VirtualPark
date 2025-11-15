import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { EventService } from '../../../backend/services/event/event.service';
import { EventModel } from '../../../backend/services/event/models/EventModel';
import { ButtonsComponent } from '../../components/buttons/buttons.component';
import { AuthRoleService } from '../../auth-role/auth-role.service';
import { MessageService } from '../../components/messages/service/message.service';

@Component({
  selector: 'app-event-page',
  standalone: true,
  imports: [CommonModule, ButtonsComponent],
  templateUrl: './event-list-page.component.html',
  styleUrls: ['./event-list-page.component.css']
})
export class EventListPageComponent implements OnInit {
  events: EventModel[] = [];
  loading = false;
  error = '';

  constructor(
    private eventSvc: EventService, 
    private router: Router, 
    private authRole: AuthRoleService,
    private messageSvc: MessageService
  ) {}

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
    this.router.navigate(['/events/', id]);
  }

  remove(id: string): void {
    this.error = '';

    if (!confirm('Are you sure you want to delete this event?')) return;

    this.eventSvc.remove(id).subscribe({
      next: () => {
        this.messageSvc.show('Event deleted successfully!', 'success');
        this.loadEvents();
      },
      error: (err) => {
        const backendMsg =
          err?.error?.message || err?.message || 'Error deleting event.';
        this.messageSvc.show('Error deleting event: ' + backendMsg, 'error');
      }
    });
  }
    canManageEvents(): boolean {
    return this.authRole.hasAnyRole(['Administrator']);
  }
}
