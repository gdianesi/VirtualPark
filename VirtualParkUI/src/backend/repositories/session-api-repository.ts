import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap, finalize } from 'rxjs';
import GenericApiRepository from './generic-api-repository';
import { LoginRequest } from '../services/session/models/LoginRequest';
import { LoginResponse } from '../services/session/models/LoginResponse';
import { GetSessionResponse } from '../services/session/models/GetSessionResponse';

@Injectable({ providedIn: 'root' })
export class SessionApiRepository extends GenericApiRepository {
    constructor(http: HttpClient) {
        super('sessions', http);
    }

    login(credentials: LoginRequest): Observable<LoginResponse> {
        return this.create<LoginResponse>(credentials, false, 'login').pipe(
            tap(res => {
                if (res?.token) localStorage.setItem('token', res.token);
            })
        );
    }

    logout(token: string): Observable<void> {
        return this.deleteById<void>(token, true,'logout').pipe(
            finalize(() => localStorage.removeItem('token'))
        );
    }

    getSessionByToken(token: string): Observable<GetSessionResponse> {
        return this.getById<GetSessionResponse>(token, true, 'getUser');
    }
}
