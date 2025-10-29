import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import GenericApiRepository from './generic-api-repository';
import { LoginRequest } from '../services/session/models/LoginRequest' 

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

    getSessionByToken(token: string): Observable<LoginResponse> {
        return this.getById<LoginResponse>(token);
    }
}