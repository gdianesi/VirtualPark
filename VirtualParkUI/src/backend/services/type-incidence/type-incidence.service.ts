import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { TypeIncidenceRepository } from '../../repositories/typeIncidence-api-repository';
import { TypeIncidenceModel } from './models/TypeIncidenceModel';
import { CreateTypeIncidenceRequest } from './models/CreateTypeIncidenceRequest';
import { CreateTypeIncidenceResponse } from './models/CreateTypeIncidenceResponse';

@Injectable({ providedIn: 'root' })
export class TypeIncidenceService {
  constructor(private readonly _typeIncidenceRepository: TypeIncidenceRepository) {}

  getAll(): Observable<TypeIncidenceModel[]> {
    return this._typeIncidenceRepository.getAllTypeIncidences();
  }

  create(request: CreateTypeIncidenceRequest): Observable<CreateTypeIncidenceResponse> {
    return this._typeIncidenceRepository.createTypeIncidence(request);
  }

  update(id: string, typeIncidence: CreateTypeIncidenceRequest): Observable<void> {
    return this._typeIncidenceRepository.updateById(id, typeIncidence)
  }
  delete(id: string): Observable<void> {
    return this._typeIncidenceRepository.deleteTypeIncidence(id);
  }
}
