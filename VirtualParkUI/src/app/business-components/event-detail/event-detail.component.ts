import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { EventService } from '../../../backend/services/event/event.service';
import { AttractionService } from '../../../backend/services/attractions/attraction.service';
import { EventModel } from '../../../backend/services/event/models/EventModel';
import { AttractionModel } from '../../../backend/services/attractions/models/AttractionModel';
import { ButtonsComponent } from '../../components/buttons/buttons.component';
import { CreateEventRequest } from '../../../backend/services/event/models/CreateEventRequest';


@Component({
  selector: 'app-event-detail',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './event-detail.component.html',
  styleUrls: ['./event-detail.component.css']
})
export class EventDetailComponent implements OnInit {
  event!: EventModel;
  attractions: AttractionModel[] = [];
  attractionNames: string[] = [];
  loading = false;
  error = '';

  constructor(
    private readonly eventSvc: EventService,
    private readonly attractionSvc: AttractionService,
    private readonly route: ActivatedRoute
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

        this.attractionNames = ev.attractions?.map(aid => {
          const found = this.attractions.find(at => at.id === aid);
          return found ? found.name : '(Unknown attraction)';
        }) ?? [];

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

    add(attractionId: string): void {
    if (!this.event.attractions?.includes(attractionId)) {
      this.event.attractions = [...(this.event.attractions || []), attractionId];
      this.saveChanges();
    }
  }

  remove(attractionId: string): void {
    this.event.attractions = this.event.attractions?.filter(id => id !== attractionId) ?? [];
    this.saveChanges();
  }

  private updateAttractionNames(): void {
  this.attractionNames =
    this.event.attractions?.map(aid => {
      const found = this.attractions.find(at => at.id === aid);
      return found ? found.name : '(Unknown attraction)';
    }) ?? [];
}

  private saveChanges(): void {
    const request: CreateEventRequest = {
      name: this.event.name,
      date: this.event.date,
      capacity: String(this.event.capacity),
      cost: String(this.event.cost),
      attractionIds: this.event.attractions ?? []
    };

    this.eventSvc.update(this.event.id, request).subscribe({
      next: () => {
        console.log('Event updated successfully');
        this.updateAttractionNames();
      },
      error: err => {
        this.error = `Error updating event: ${err.message}`;
        console.error(err);
      }
    });
  }

  getAttractionName(id: string): string {
  const found = this.attractions.find(a => a.id === id);
  return found ? found.name : '(Unknown attraction)';
}
}
