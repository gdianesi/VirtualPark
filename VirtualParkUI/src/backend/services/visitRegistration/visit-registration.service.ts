import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { VisitRegistrationApiRepository } from '../../repositories/visitRegistration-api-repository';
import { VisitScoreRequest } from './models/VisitScoreRequest';
import { AttractionModel } from '../attraction/models/AttractionModel';

@Injectable({
    providedIn: 'root'
})
export class VisitRegistrationService {
    constructor(private readonly repository: VisitRegistrationApiRepository) {}

    recordScoreEvent(token: string, payload: VisitScoreRequest): Observable<void> {
        return this.repository.recordScoreEvent(token, payload);
    }

    upToAttraction(visitorId: string, attractionId: string): Observable<void> {
        return this.repository.upToAttraction(visitorId, attractionId);
    }

    downToAttraction(visitorId: string): Observable<void> {
        return this.repository.downToAttraction(visitorId);
    }

    getAvailableAttractions(visitorId: string): Observable<AttractionModel[]> {
        return this.repository.getAvailableAttractions(visitorId);
    }
}
