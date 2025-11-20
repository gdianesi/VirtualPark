import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { EventService } from '../../../backend/services/event/event.service';
import { EventModel } from '../../../backend/services/event/models/EventModel';
import { CreateEventRequest } from '../../../backend/services/event/models/CreateEventRequest';
import { AttractionModel } from '../../../backend/services/attraction/models/AttractionModel';
import { AttractionService } from '../../../backend/services/attraction/attraction.service';
import { MessageService } from '../../../backend/services/message/message.service';
import { MessageComponent } from '../../components/messages/message.component';
import { ButtonsComponent } from '../../components/buttons/buttons.component';

@Component({
  selector: 'app-event-detail',
  standalone: true,
  imports: [CommonModule, MessageComponent, ButtonsComponent],
  templateUrl: './event-detail.component.html',
  styleUrls: ['./event-detail.component.css']
})
export class EventDetailComponent implements OnInit {
  event!: EventModel;
  attractions: AttractionModel[] = [];
  loading = false;
  error = '';

  constructor(
    private readonly eventSvc: EventService,
    private readonly attractionSvc: AttractionService,
    private readonly route: ActivatedRoute,
    private readonly messageSvc: MessageService
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (!id) {
      this.error = 'Invalid event ID';
      return;
    }

    this.loadAttractions();
    this.loadEvent(id);
  }

  private loadEvent(id: string): void {
    this.loading = true;
    this.eventSvc.getById(id).subscribe({
      next: ev => {
        this.event = ev;
        this.loading = false;
      },
      error: err => {
        this.error = `Error loading event: ${err.message}`;
        this.loading = false;
      }
    });
  }

  private loadAttractions(): void {
    this.attractionSvc.getAll().subscribe({
      next: list => (this.attractions = list),
      error: err => (this.error = `Error loading attractions: ${err.message}`)
    });
  }

  get linkedAttractions(): AttractionModel[] {
    return this.attractions.filter(a => this.event?.attractions?.includes(a.id));
  }

  get availableAttractions(): AttractionModel[] {
    return this.attractions.filter(a => !this.event?.attractions?.includes(a.id));
  }

  add(attractionId: string): void {
    if (!this.event.attractions?.includes(attractionId)) {
      this.event.attractions = [...(this.event.attractions || []), attractionId];
      this.saveChanges('Attraction added successfully!');
    } else {
      this.messageSvc.show('Attraction already added', 'info');
    }
  }

  remove(attractionId: string): void {
    this.event.attractions = this.event.attractions?.filter(id => id !== attractionId) ?? [];
    this.saveChanges('Attraction removed successfully!');
  }

  private saveChanges(successMsg: string): void {
    const request: CreateEventRequest = {
      name: this.event.name,
      date: this.event.date,
      capacity: String(this.event.capacity),
      cost: String(this.event.cost),
      attractionsIds: this.event.attractions ?? []
    };

    this.eventSvc.update(this.event.id, request).subscribe({
      next: () => {
        this.messageSvc.show(successMsg, 'success');
        console.log('Event updated successfully');
      },
      error: err => {
        this.error = `Error updating event: ${err.message}`;
        this.messageSvc.show('Error updating event', 'error');
      }
    });
  }
}
