import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { TicketRepository } from '../../repositories/ticket-api-repository';
import { TicketModel } from './models/TicketModel';
import { CreateTicketRequest } from './models/CreateTicketRequest';

@Injectable({ providedIn: 'root' })

export class TicketService {
  constructor(private readonly _repo: TicketRepository) {}

  getAll(): Observable<TicketModel[]> {
    return this._repo.getAll();
  }

  getById(id: string): Observable<TicketModel> {
    return this._repo.getById(id);
  }

  create(ticket: CreateTicketRequest): Observable<void> {
    return this._repo.create(ticket);
  }

  remove(id: string): Observable<void> {
    return this._repo.deleteTicket(id);
  }
}