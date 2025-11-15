import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IncidenceRepository } from '../../repositories/incident-api-repository';
import { CreateIncidenceRequest } from './models/CreateIncidenceRequest';
import { GetIncidenceResponse } from './models/GetIncidenceResponse';
import { IncidenceModel } from './models/IncidenceModel';

@Injectable({ providedIn: 'root' })
export class IncidenceService {
  constructor(private readonly _repo: IncidenceRepository) {}

  getAll(): Observable<IncidenceModel[]> {
    return this._repo.getAll();
  }

  getById(id: string): Observable<GetIncidenceResponse> {
    return this._repo.getById(id);
  }

  create(incidence: CreateIncidenceRequest): Observable<void> {
    return this._repo.create(incidence);
  }

  update(id: string, incidence: CreateIncidenceRequest): Observable<void> {
    return this._repo.updateIncidence(id, incidence);
  }

  remove(id: string): Observable<void> {
    return this._repo.deleteIncidence(id);
  }
}
