import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import GenericApiRepository from './generic-api-repository'
import { StrategyModel } from '../services/strategy/models/StrategyModel'
import { GetStrategiesKeyResponse } from '../services/strategy/models/GetStrategiesKeyResponse';
import { CreateStrategyResponse } from '../services/strategy/models/CreateStrategyResponse';
@Injectable({providedIn: 'root'})
export class StrategyApiRepository extends GenericApiRepository{
    constructor(http: HttpClient) {
        super('strategies', http);
    }

    public createStrategy(body: StrategyModel): Observable<CreateStrategyResponse> {
            return this.create<CreateStrategyResponse>(body);
    }
    
    public getAllKeyStrategies() : Observable<GetStrategiesKeyResponse[]> {
        return this.getAll<GetStrategiesKeyResponse[]>("keys");
    }
    
    public getAllActiveStrategies() : Observable<StrategyModel[]>{
        return this.getAll<StrategyModel[]>();
    }
    
    public getActiveStrategy(date: string): Observable<StrategyModel>{
        return this.getById(date);
    }
    
    public deleteStrategy(date: string): Observable<void>{
        return  this.deleteById(date);
    }
    
    public updateStrategy(date: string, strategy : GetStrategiesKeyResponse): Observable<void>{
        return this.updateById(date, strategy);
    }
}
