import { Observable } from 'rxjs';
import {Injectable} from "@angular/core";
import { LoginResponse } from './models/LoginResponse'
import { LoginRequest } from './models/LoginRequest';
import { SessionApiRepository } from '../../repositories/session-api-repository'
import { GetSessionResponse } from './models/GetSessionResponse'

@Injectable({
    providedIn: 'root'
})
export class SessionService {
    constructor(private readonly _sessionRepository: SessionApiRepository) {}
    
    login(credentials: LoginRequest): Observable<LoginResponse>{
        return this._sessionRepository.login(credentials);
    }

    logout(): Observable<void>{
        return this._sessionRepository.logout();
    }
    
    getSession() {
        return this._sessionRepository.getSession();
    }
}