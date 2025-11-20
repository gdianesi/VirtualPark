import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { TicketRepository } from '../../repositories/ticket-api-repository';
import { TicketModel } from './models/TicketModel';
import { CreateTicketRequest } from './models/CreateTicketRequest';
import { CreateTicketResponse } from './models/CreateTicketResponse';

@Injectable({ providedIn: 'root' })
export class TicketService {
  constructor(private readonly _repo: TicketRepository) {}

  getAll(): Observable<TicketModel[]> {
    return this._repo.getAllTickets();
  }

  getById(id: string): Observable<TicketModel> {
    return this._repo.getTicketById(id);
  }

  create(ticket: CreateTicketRequest): Observable<CreateTicketResponse> {
    return this._repo.createTicket(ticket);
  }

  delete(id: string): Observable<void> {
    return this._repo.deleteTicket(id);
  }
  getByVisitor(visitorId: string): Observable<TicketModel[]> {
    return this._repo.getTicketsByVisitor(visitorId);
  }
}