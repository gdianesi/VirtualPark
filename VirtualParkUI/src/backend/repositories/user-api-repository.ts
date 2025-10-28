import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import GenericApiRepository from './generic-api-repository';
import { CreateUserRequest } from '../services/user/models/CreateUserRequest';
import { CreateUserResponse } from '../services/user/models/CreateUserResponse';
import { UserModel } from '../services/user/models/UserModel';

@Injectable({ providedIn: 'root' })
export class UserApiRepository extends GenericApiRepository {
    constructor(http: HttpClient) {
        super('users', http);
    }
    
    public getAllUsers(): Observable<UserModel[]>{
        return this.getAll<UserModel[]>();
    }
    
    public getUserById(id: string): Observable<UserModel>{
        return this.getById<UserModel>(id);
    }
    
    public createUser(user: CreateUserRequest): Observable<CreateUserResponse>{
        return this.create(user);
    }

    public updateUser(id: string, body: CreateUserRequest): Observable<void> {
        return this.updateById(id, body);
    }
    
    public deleteUser(id: string): Observable<void>{
        return this.deleteById(id);
    }
}