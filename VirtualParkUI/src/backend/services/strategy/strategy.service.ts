import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { StrategyModel } from './models/StrategyModel'
import { CreateStrategyResponse } from './models/CreateStrategyResponse';
import { GetStrategiesKeyResponse } from './models/GetStrategiesKeyResponse';
import { StrategyApiRepository } from '../../repositories/strategy-api-repository'

@Injectable({
    providedIn: 'root'
})
export class StrategyService {
    constructor(private readonly _strategyRepository: StrategyApiRepository) { }
    
    getAll(): Observable<StrategyModel[]>{
        return this._strategyRepository.getAllActiveStrategies();
    }
    
    getAllKeys() : Observable<GetStrategiesKeyResponse[]>{
        return this._strategyRepository.getAllKeyStrategies();
    }
    
    get(date: string) : Observable<StrategyModel>{
        return this._strategyRepository.getActiveStrategy(date);
    }
    
    create(body: StrategyModel) : Observable<CreateStrategyResponse>{
        return this._strategyRepository.createStrategy(body);
    } 
    
    delete(date: string) : Observable<void>{
        return this._strategyRepository.deleteStrategy(date);
    }
    
    update(date: string, strategy : GetStrategiesKeyResponse): Observable<void>{
        return this._strategyRepository.updateStrategy(date, strategy);
    }

    formatDate(dateString: string): string {
        if (/^\d{4}-\d{2}-\d{2}$/.test(dateString)) {
            return dateString;
        }
            
        const date = new Date(dateString);
        const year = date.getFullYear();
        const month = String(date.getMonth() + 1).padStart(2, '0');
        const day = String(date.getDate()).padStart(2, '0');
            
        return `${year}-${month}-${day}`;
    }
}