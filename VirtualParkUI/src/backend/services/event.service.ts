import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CreateEventRequest, EventModel } from '../../app/event/models/event.model';

@Injectable({
  providedIn: 'root'
})
export class EventService {
  private readonly apiUrl = 'http://localhost:5104/events';

  constructor(private http: HttpClient) {}

  getAll(): Observable<EventModel[]> {
    return this.http.get<EventModel[]>(this.apiUrl);
  }

  getById(id: string): Observable<EventModel> {
    return this.http.get<EventModel>(`${this.apiUrl}/${id}`);
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
