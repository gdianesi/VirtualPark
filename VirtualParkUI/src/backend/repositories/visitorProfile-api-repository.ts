import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import GenericApiRepository from './generic-api-repository';
import { VisitorProfileModel } from '../services/visitorProfile/models/VisitorProfileModel';

@Injectable({ providedIn: 'root' })
export class VisitorProfileApiRepository extends GenericApiRepository {
    constructor(http: HttpClient) {
        super('visitorProfiles', http);
    }

    public getVisitorProfileById(id: string): Observable<VisitorProfileModel> {
        return this.getById<VisitorProfileModel>(id);
    }
}
