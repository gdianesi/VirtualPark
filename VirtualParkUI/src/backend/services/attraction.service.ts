import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AttractionModel, AttractionRepository } from '../repositories/attraction-api-repository';
import { CreateAttractionRequest, CreateAttractionResponse } from '../../app/attraction/models/attraction.model';

@Injectable({ providedIn: 'root' })
export class AttractionService {
  constructor(private repo: AttractionRepository) {}
  getAll(): Observable<AttractionModel[]> { return this.repo.getAllAttractions(); }

  create(payload: CreateAttractionRequest): Observable<CreateAttractionResponse> {
    return this.repo.createAttraction(payload);
  }
}
