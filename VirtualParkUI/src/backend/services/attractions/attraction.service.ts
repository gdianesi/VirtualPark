import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AttractionRepository } from '../../repositories/attraction-api-repository';
import { AttractionModel } from './models/AttractionModel';

@Injectable({ providedIn: 'root' })
export class AttractionService {
  constructor(private readonly _attractionRepo: AttractionRepository) {}

  getAll(): Observable<AttractionModel[]> {
    return this._attractionRepo.getAllAttractions();
  }

  getById(id: string): Observable<AttractionModel> {
    return this._attractionRepo.getAttractionById(id);
  }

  remove(id: string): Observable<void> {
    return this._attractionRepo.deleteAttraction(id);
  }
}

