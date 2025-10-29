import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import GenericApiRepository from './generic-api-repository';
import { LoginRequest } from '../services/session/models/LoginRequest' 
import { LoginResponse } from '../services/session/models/LoginResponse';
import { GetSessionResponse } from '../services/session/models/GetSessionResponse';

@Injectable({
    providedIn: 'root'
})
export class SessionApiRepository extends GenericApiRepository {
    constructor(http: HttpClient) {
        super('sessions', http);
    }

    login(credentials: LoginRequest): Observable<LoginResponse> {
        return this.create<LoginResponse>(credentials, false);
    }

    logout(token: string): Observable<void> {
        return this.deleteById<void>(token);
    }

    getSessionByToken(token: string): Observable<GetSessionResponse> {
        return this.getById<GetSessionResponse>(token);
    }
}