import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import GenericApiRepository from './generic-api-repository';
import { Observable } from 'rxjs';

export interface AttractionModel {
  id: string;
  name: string;
}

@Injectable({ providedIn: 'root' })
export class AttractionRepository extends GenericApiRepository {
  constructor(http: HttpClient) {
    super('attractions', 'http://localhost:5104', http);
  }

  getAllAttractions(): Observable<AttractionModel[]> {
    return this.getAll<AttractionModel[]>();
  }
}
