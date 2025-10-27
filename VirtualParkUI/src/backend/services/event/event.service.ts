import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CreateEventRequest, EventModel } from '../../../app/event/models/event.model';
import { EventRepository } from '../../repositories/event-api-repository'

@Injectable({
  providedIn: 'root'
})
export class EventService {
    constructor(private readonly _eventRepository: EventRepository) {
    }
  getAll(): Observable<EventModel[]> {
    return this._eventRepository.getAll();
  }

  getById(id: string): Observable<EventModel> {
    return this._eventRepository.getById();
  }

  create(req: CreateEventRequest): Observable<void> {
    return this.http.post<void>(this.apiUrl, req);
  }

  update(id: string, req: CreateEventRequest): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, req);
  }

  remove(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  addAttraction(eventId: string, attractionId: string): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/${eventId}/attractions/${attractionId}`, {});
  }

  removeAttraction(eventId: string, attractionId: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${eventId}/attractions/${attractionId}`);
  }
}
