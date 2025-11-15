import { Observable } from 'rxjs';
import { Injectable } from "@angular/core";
import { VisitorProfileApiRepository } from '../../repositories/visitorProfile-api-repository';
import { VisitorProfileModel } from './models/VisitorProfileModel';

@Injectable({
    providedIn: 'root'
})
export class VisitorProfileService {
    constructor(private readonly _visitorProfileRepository: VisitorProfileApiRepository) { }

    getById(id: string): Observable<VisitorProfileModel> {
        return this._visitorProfileRepository.getVisitorProfileById(id);
    }
}