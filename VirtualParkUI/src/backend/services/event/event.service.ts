import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { EventModel } from './models/EventModel';
import { CreateEventResponse } from './models/CreateEventResponse';
import { CreateEventRequest } from './models/CreateEventRequest';
import { EventRepository } from '../../repositories/event-api-repository'

@Injectable({
  providedIn: 'root'
})
export class EventService {
    constructor(private readonly _eventRepository: EventRepository) {
    }
  getAll(): Observable<EventModel[]> {
    return this._eventRepository.getAllEvents();
  }

  getById(id: string): Observable<EventModel> {
    return this._eventRepository.getEventById(id);
  }

  create(event: CreateEventRequest): Observable<CreateEventResponse> {
    return this._eventRepository.createEvent(event);
  }

  update(id: string, event: CreateEventRequest): Observable<void> {
    return this._eventRepository.updateById(id, event)
  }

  remove(id: string): Observable<void> {
    return this._eventRepository.deleteEvent(id);
  }
}
