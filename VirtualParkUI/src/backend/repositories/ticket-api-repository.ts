import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import GenericApiRepository from './generic-api-repository';
import { Observable } from 'rxjs';
import { TicketModel } from '../services/ticket/models/TicketModel';
import { CreateTicketResponse } from '../services/ticket/models/CreateTicketResponse';
import { CreateTicketRequest } from '../services/ticket/models/CreateTicketRequest';

@Injectable({ providedIn: 'root' })
export class TicketRepository extends GenericApiRepository {
  constructor(http: HttpClient) {
    super('tickets', http);
  }

  getAllTickets(): Observable<TicketModel[]> {
    return super.getAll<TicketModel[]>();
  }

  getTicketById(id: string): Observable<TicketModel> {
    return super.getById<TicketModel>(id);
  }

  createTicket(body: CreateTicketRequest): Observable<CreateTicketResponse> {
    return super.create<CreateTicketResponse>(body);
  }

  deleteTicket(id: string): Observable<void> {
    return super.deleteById<void>(id);
  }
  getTicketsByVisitor(visitorId: string): Observable<TicketModel[]> {
    return this.getAll<TicketModel[]>(`visitor/${visitorId}`);
  }
}