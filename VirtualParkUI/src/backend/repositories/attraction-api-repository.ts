import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import GenericApiRepository from './generic-api-repository';
import { Observable } from 'rxjs';
import { AttractionModel } from '../services/attraction/models/AttractionModel';
import { CreateAttractionRequest } from '../services/attraction/models/CreateAttractionRequest';
import { CreateAttractionResponse } from '../services/attraction/models/CreateAttractionResponse';
import { GetAttractionResponse } from '../services/attraction/models/GetAttractionRequest';

@Injectable({ providedIn: 'root' })
export class AttractionRepository extends GenericApiRepository {
  constructor(http: HttpClient) {
    super('attractions', http);
  }

  getAllAttractions(): Observable<AttractionModel[]> {
    return this.getAll<AttractionModel[]>();
  }

  public getAttractionById(id: string): Observable<GetAttractionResponse> {
      return this.getById<GetAttractionResponse>(id);
  }

  deleteAttraction(id: string): Observable<void> {
    return this.deleteById<void>(id);
  }

  public createAttraction(body: CreateAttractionRequest): Observable<CreateAttractionResponse> {
    return this.create<CreateAttractionResponse>(body);
  }
}