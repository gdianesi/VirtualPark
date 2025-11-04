import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AttractionModel } from './models/AttractionModel';
import { CreateAttractionRequest } from './models/CreateAttractionRequest';
import { AttractionRepository } from '../../repositories/attraction-api-repository';
import { GetAttractionResponse } from './models/GetAttractionRequest';

@Injectable({ providedIn: 'root' })
export class AttractionService {
  constructor(private readonly _repo: AttractionRepository) {}

  getAll(): Observable<AttractionModel[]> {
    return this._repo.getAll();
  }

  getById(id: string): Observable<GetAttractionResponse> {
    return this._repo.getById(id);
  }

  create(attraction: CreateAttractionRequest): Observable<void> {
    return this._repo.create(attraction);
  }

  remove(id: string): Observable<void> {
    return this._repo.deleteAttraction(id);
  }

  update(id: string, attraction: CreateAttractionRequest): Observable<void> {
    return this._repo.updateAttraction(id, attraction);
  }
}
