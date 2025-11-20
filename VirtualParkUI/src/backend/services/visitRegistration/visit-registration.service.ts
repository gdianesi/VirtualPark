import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { VisitRegistrationApiRepository } from '../../repositories/visitRegistration-api-repository';
import { VisitScoreRequest } from './models/VisitScoreRequest';
import { VisitRegistrationTodayResponse } from './models/VisitRegistrationTodayResponse';
import { AttractionModel } from '../attraction/models/AttractionModel';
import { VisitorInAttractionModel } from './models/VisitorInAttractionModel';

@Injectable({
    providedIn: 'root'
})
export class VisitRegistrationService {
    constructor(private readonly repository: VisitRegistrationApiRepository) {}

    recordScoreEvent(payload: VisitScoreRequest): Observable<void> {
        return this.repository.recordScoreEvent(payload);
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

    getVisitorsInAttraction(attractionId: string): Observable<VisitorInAttractionModel[]> {
        return this.repository.getVisitorsInAttraction(attractionId);
    }
    
    getTodayVisit(visitorId: string): Observable<VisitRegistrationTodayResponse> {
        return this.repository.getTodayVisit(visitorId);
    }
}
