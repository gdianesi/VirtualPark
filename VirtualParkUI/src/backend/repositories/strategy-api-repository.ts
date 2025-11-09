import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import GenericApiRepository from './generic-api-repository'
import { StrategyModel } from '../services/strategy/models/StrategyModel'
import { GetStrategiesKeyResponse } from '../services/strategy/models/GetStrategiesKeyResponse';

@Injectable({providedIn: 'root'})
export class StrategyApiRepository extends GenericApiRepository{
    constructor(http: HttpClient) {
        super('strategies', http);
    }

    public createStrategy(body: StrategyModel): Observable<StrategyModel>{{
        return this.create<StrategyModel>(body);
    }

    public getAllStrategies(): Observable<GetStrategiesKeyResponse[]>{
        return this.getAll<GetStrategiesKeyResponse[]>('keys');
    }


}
