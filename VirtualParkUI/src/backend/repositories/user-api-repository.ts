import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import GenericApiRepository from './generic-api-repository';

@Injectable({ providedIn: 'root' })
export class UserApiRepository extends GenericApiRepository {
    constructor(http: HttpClient) {
        super('users', http);
    }
}