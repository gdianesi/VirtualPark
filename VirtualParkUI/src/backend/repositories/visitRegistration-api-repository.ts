import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import GenericApiRepository from './generic-api-repository';
import { VisitScoreRequest } from '../services/visitRegistration/models/VisitScoreRequest';
import { AttractionModel } from '../services/attraction/models/AttractionModel';
import { VisitorInAttractionModel } from '../services/visitRegistration/models/VisitorInAttractionModel';

@Injectable({ providedIn: 'root' })
export class VisitRegistrationApiRepository extends GenericApiRepository {
    constructor(http: HttpClient) {
        super('visitRegistrations', http);
    }

    recordScoreEvent(body: VisitScoreRequest): Observable<void> {
        return this.create<void>(body, true, `scoreEvents/`);
    }

    upToAttraction(visitorId: string, attractionId: string): Observable<void> {
        return this.create<void>({}, true, `${visitorId}/currentAttraction/${attractionId}`);
    }

    downToAttraction(visitorId: string): Observable<void> {
        return this.create<void>({}, true, `${visitorId}/currentAttraction`);
    }

    getAvailableAttractions(visitorId: string): Observable<AttractionModel[]> {
        return this.getAll<AttractionModel[]>(`${visitorId}/availableAttractions`);
    }

    getVisitorsInAttraction(attractionId: string): Observable<VisitorInAttractionModel[]> {
        return this.getAll<VisitorInAttractionModel[]>(`attractions/${attractionId}/visitors`);
    }
}
