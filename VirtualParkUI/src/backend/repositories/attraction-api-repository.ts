import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import GenericApiRepository from './generic-api-repository';
import { Observable } from 'rxjs';
import { CreateAttractionRequest } from '../services/attraction/models/CreateAttractionRequest';
import { CreateAttractionResponse } from '../services/attraction/models/CreateAttractionResponse';
import { GetAttractionResponse } from '../services/attraction/models/GetAttractionRequest';

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

  createAttraction(body: CreateAttractionRequest): Observable<CreateAttractionResponse> {
    return this.create<CreateAttractionResponse>(body);
  }

  getAttractionById(id: string): Observable<GetAttractionResponse> {
    return this.getById<GetAttractionResponse>(id);
  }
}
