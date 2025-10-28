import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import GenericApiRepository from './generic-api-repository';
import { AttractionModel } from '../../backend/services/attractions/models/AttractionModel';

@Injectable({ providedIn: 'root' })
export class AttractionRepository extends GenericApiRepository {
  constructor(http: HttpClient) {
    super('attractions', http);
  }

  getAllAttractions(): Observable<AttractionModel[]> {
    return this.getAll<AttractionModel[]>();
  }

  getAttractionById(id: string): Observable<AttractionModel> {
    return this.getById<AttractionModel>(id);
  }

  deleteAttraction(id: string): Observable<void> {
    return this.deleteById<void>(id);
  }
}
