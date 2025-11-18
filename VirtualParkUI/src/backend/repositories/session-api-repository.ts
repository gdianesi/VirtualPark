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
        return this.create<LoginResponse>(credentials, false).pipe(
            tap(res => {
                if (res?.token) localStorage.setItem('token', res.token);
            })
        );
    }

    logout(): Observable<void> {
        return this.deleteById<void>('', true).pipe(
            finalize(() => localStorage.removeItem('token'))
        );
    }

    getSession(): Observable<GetSessionResponse> {
        return this.getAll<GetSessionResponse>('me').pipe(
            tap(res => {
                if (res?.visitorId) localStorage.setItem('visitorId', res.visitorId);
                localStorage.setItem('roles', JSON.stringify(res.roles));
            })
        );
    }
}
