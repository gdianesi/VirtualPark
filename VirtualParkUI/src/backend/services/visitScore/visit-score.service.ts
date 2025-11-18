import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { VisitScoreRepository } from '../../repositories/visitScore-api-repository';
import { GetVisitScoreResponse } from './models/GetVisitScoreResponse';

@Injectable({ providedIn: 'root' })
export class VisitScoreService {
    constructor(private readonly repository: VisitScoreRepository) {}

    getHistoryByVisitor(visitorId: string): Observable<GetVisitScoreResponse[]> {
        return this.repository.getHistoryByVisitor(visitorId);
    }
}
