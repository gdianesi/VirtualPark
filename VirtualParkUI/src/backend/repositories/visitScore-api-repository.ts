import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import GenericApiRepository from './generic-api-repository';
import { GetVisitScoreResponse } from '../services/visitScore/models/GetVisitScoreResponse';

@Injectable({ providedIn: 'root' })
export class VisitScoreRepository extends GenericApiRepository {
    constructor(http: HttpClient) {
        super('visitScores', http);
    }

    getHistoryByVisitor(visitorId: string): Observable<GetVisitScoreResponse[]> {
        return this.getAll<GetVisitScoreResponse[]>(`getHistory/${visitorId}`);
    }
}
