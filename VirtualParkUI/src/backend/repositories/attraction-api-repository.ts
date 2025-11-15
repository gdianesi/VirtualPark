import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import GenericApiRepository from './generic-api-repository';
import { Observable } from 'rxjs';
import { AttractionModel } from '../services/attraction/models/AttractionModel';
import { CreateAttractionRequest } from '../services/attraction/models/CreateAttractionRequest';
import { CreateAttractionResponse } from '../services/attraction/models/CreateAttractionResponse';
import { GetAttractionResponse } from '../services/attraction/models/GetAttractionRequest';
import { ReportAttractionResponse } from '../services/attraction/models/ReportAttractionResponse';

@Injectable({ providedIn: 'root' })
export class AttractionRepository extends GenericApiRepository {
  constructor(http: HttpClient) {
    super('attractions', http);
  }

  getAllAttractions(): Observable<AttractionModel[]> {
    return this.getAll<AttractionModel[]>();
  }

  getAttractionById(id: string): Observable<GetAttractionResponse> {
      return this.getById<GetAttractionResponse>(id);
  }

  deleteAttraction(id: string): Observable<void> {
    return this.deleteById<void>(id);
  }

  createAttraction(body: CreateAttractionRequest): Observable<CreateAttractionResponse> {
    return this.create<CreateAttractionResponse>(body);
  }

  updateAttraction(id: string, body: CreateAttractionRequest): Observable<void> {
    return this.updateById<void>(id, body);
  }

  getAttractionsReport(from: string, to: string) {
    return this.getWithParams<ReportAttractionResponse[]>(
      { from, to },
      true,
      'report'
    );
  }
}
