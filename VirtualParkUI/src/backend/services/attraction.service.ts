import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AttractionModel, AttractionRepository } from '../repositories/attraction-api-repository';

@Injectable({ providedIn: 'root' })
export class AttractionService {
  constructor(private repo: AttractionRepository) {}
  getAll(): Observable<AttractionModel[]> { return this.repo.getAllAttractions(); }
}
