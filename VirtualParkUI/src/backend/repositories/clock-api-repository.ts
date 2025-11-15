import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import GenericApiRepository from './generic-api-repository';
import { ClockModel } from '../services/clock/models/ClockModel';

@Injectable({ providedIn: 'root' })
export class ClockRepository extends GenericApiRepository {
    constructor(http: HttpClient) {
        super('clock', http);
    }
    getClock(): Observable<ClockModel> {
        return this.http.get<ClockModel>(this.baseUrl, this.requestOptions());
    }
    updateClock(clock: ClockModel): Observable<void> {
        return this.http.put<void>(this.baseUrl, clock, this.requestOptions());
    }
}
