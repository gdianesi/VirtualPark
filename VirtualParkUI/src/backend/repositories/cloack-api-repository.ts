import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import GenericApiRepository from './generic-api-repository';
import { CloackModel } from '../services/cloack/models/CloackModel';

@Injectable({ providedIn: 'root' })
export class CloackRepository extends GenericApiRepository {
    constructor(http: HttpClient) {
        super('clock', http);
    }
    getClock(): Observable<CloackModel> {
        return this.http.get<CloackModel>(this.baseUrl, this.requestOptions());
    }
    updateClock(cloack: CloackModel): Observable<void> {
        return this.http.put<void>(this.baseUrl, cloack, this.requestOptions());
    }
}
