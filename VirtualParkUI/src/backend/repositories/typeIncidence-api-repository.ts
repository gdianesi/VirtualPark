import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import GenericApiRepository from './generic-api-repository';
import { TypeIncidenceModel } from '../services/type-incidence/models/TypeIncidenceModel';
import { Observable } from 'rxjs';
import { CreateTypeIncidenceRequest } from '../services/type-incidence/models/CreateTypeIncidenceRequest';
import { CreateTypeIncidenceResponse } from '../services/type-incidence/models/CreateTypeIncidenceResponse';


@Injectable({ providedIn: 'root' })
export class TypeIncidenceRepository extends GenericApiRepository {
  constructor(http: HttpClient) {
    super('incidence-types', http);
  }

    public getAllTypeIncidences(): Observable<TypeIncidenceModel[]>{
        return this.getAll<TypeIncidenceModel[]>();
    }
    
    public getTypeIncidenceById(id: string): Observable<TypeIncidenceModel>{
        return  this.getById<TypeIncidenceModel>(id);
    }
    
    public createTypeIncidence(body: CreateTypeIncidenceRequest): Observable<CreateTypeIncidenceResponse>{
        return this.create<CreateTypeIncidenceResponse>(body);
    }
    
    public updateTypeIncidence(id: string, body: CreateTypeIncidenceRequest): Observable<void>{
        return this.updateById<void>(id, body);
    }
    
    public deleteTypeIncidence(id: string): Observable<void>{
        return this.deleteById<void>(id);
    }
}

