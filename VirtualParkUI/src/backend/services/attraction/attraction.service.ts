import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AttractionModel } from './models/AttractionModel';
import { CreateAttractionRequest } from './models/CreateAttractionRequest';
import { AttractionRepository } from '../../repositories/attraction-api-repository';
import { GetAttractionResponse } from './models/GetAttractionRequest';
import { CreateAttractionResponse } from './models/CreateAttractionResponse';
import { ReportAttractionResponse } from './models/ReportAttractionResponse';
import { ValidateEntryResponse } from './models/ValidateEntryResponse';
import { ValidateEntryByQrRequest } from './models/ValidateEntryByQrRequest';
import { ValidateEntryByNfcRequest } from './models/ValidateEntryByNfcRequest';

@Injectable({ providedIn: 'root' })
export class AttractionService {
  constructor(private readonly _repo: AttractionRepository) {}

  getAll(): Observable<AttractionModel[]> {
    return this._repo.getAllAttractions();
  }

  getById(id: string): Observable<GetAttractionResponse> {
    return this._repo.getAttractionById(id);
  }

  create(attraction: CreateAttractionRequest): Observable<CreateAttractionResponse> {
    return this._repo.createAttraction(attraction);
  }

  remove(id: string): Observable<void> {
    return this._repo.deleteAttraction(id);
  }

  update(id: string, attraction: CreateAttractionRequest): Observable<void> {
    return this._repo.updateAttraction(id, attraction);
  }

  getReport(from: string, to: string): Observable<ReportAttractionResponse[]> {
    return this._repo.getAttractionsReport(from, to);
  }

  validateEntryByQr(attractionId: string, qrId: string): Observable<ValidateEntryResponse> {
    const payload: ValidateEntryByQrRequest = { qrId };
    return this._repo.validateEntryByQr(attractionId, payload);
  }

  validateEntryByNfc(attractionId: string, visitorId: string): Observable<ValidateEntryResponse> {
    const payload: ValidateEntryByNfcRequest = { visitorId };
    return this._repo.validateEntryByNfc(attractionId, payload);
  }

  getDeleted(): Observable<AttractionModel[]> {
    return this._repo.getDeletedAttractions();
  }
}
