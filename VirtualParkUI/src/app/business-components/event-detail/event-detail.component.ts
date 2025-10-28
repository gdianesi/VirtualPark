import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { EventService } from '../../../backend/services/event.service';
import { AttractionService } from '../../../backend/services/attraction.service';
import { EventModel } from '../../event/models/event.model';
import { AttractionModel } from '../../../backend/repositories/attraction-api-repository';

@Component({
  selector: 'app-event-detail',
  standalone: true,
  imports : [CommonModule],
  templateUrl: './event-detail.component.html',
  styleUrls: ['./event-detail.component.css']
})
export class EventDetailComponent implements OnInit {
event!: EventModel & { attractions: AttractionModel[] };
  attractions: AttractionModel[] = [];
  loading = true;
  error?: string;

  constructor(
    private route: ActivatedRoute,
    private eventSvc: EventService,
    private attractionSvc: AttractionService
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (!id) {
      this.error = 'Invalid event ID';
      return;
    }

    this.loadEvent(id);
  }

private loadEvent(id: string): void {
  this.eventSvc.getById(id).subscribe({
    next: ev => {
      this.event = {
        ...ev,
        attractions: ev.attractions?.map(a => {
          if (typeof a === 'string') {
            const found = this.attractions.find(at => at.id === a);
            return found || { id: a, name: '(Unknown attraction)' };
          }
          return a;
        }) || []
      };
      this.loading = false;
      this.loadAttractions();
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
    if (!this.event) return;
    this.eventSvc.addAttraction(this.event.id, attractionId).subscribe({
      next: () => {
        alert('Attraction added!');
        this.loadEvent(this.event!.id);
      },
      error: err => alert(`Error adding attraction: ${err.message}`)
    });
  }

  remove(attractionId: string): void {
    if (!this.event) return;
    this.eventSvc.removeAttraction(this.event.id, attractionId).subscribe({
      next: () => {
        alert('Attraction removed!');
        this.loadEvent(this.event!.id);
      },
      error: err => alert(`Error removing attraction: ${err.message}`)
    });
  }
}

