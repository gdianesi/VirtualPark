import { Observable } from 'rxjs';
import {Injectable} from "@angular/core";
import { LoginResponse } from './models/LoginResponse'
import { LoginRequest } from './models/LoginRequest';
import { SessionApiRepository } from '../../repositories/session-api-repository'

@Injectable({
    providedIn: 'root'
})
export class SessionService {
    constructor(private readonly _sessionRepositoy: SessionApiRepository) {}
    
    login(credentials: LoginRequest): Observable<LoginResponse>{
        return this._sessionRepositoy.login(credentials);
    }

    logout(token: string): Observable<void>{
        return this._sessionRepositoy.logout(token);
    }
    
    getSession()
}