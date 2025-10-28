import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import GenericApiRepository from './generic-api-repository';
import { EventModel } from '../services/event/models/EventModel';
import { CreateEventResponse } from '../services/event/models/CreateEventResponse';
import { CreateEventRequest } from '../services/event/models/CreateEventRequest';

@Injectable({ providedIn: 'root' })
export class EventRepository extends GenericApiRepository {
  constructor(http: HttpClient) {
    super('events',  http);
  }

  public getAllEvents(): Observable<EventModel[]> {
    return this.getAll<EventModel[]>();
  }

  public getEventById(id: string): Observable<EventModel> {
    return this.getById<EventModel>(id);
  }

public createEvent(body: CreateEventRequest): Observable<CreateEventResponse> {
    return this.create<CreateEventResponse>(body);
  }

  public updateEvent(id: string, body: CreateEventRequest): Observable<void> {
    return this.updateById<void>(id, body);
  }

  public deleteEvent(id: string): Observable<void> {
    return this.deleteById<void>(id);
  }
}
