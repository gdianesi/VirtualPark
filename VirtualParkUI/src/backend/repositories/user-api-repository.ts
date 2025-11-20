import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import GenericApiRepository from './generic-api-repository';
import { CreateUserRequest } from '../services/user/models/CreateUserRequest';
import { CreateUserResponse } from '../services/user/models/CreateUserResponse';
import { UserModel } from '../services/user/models/UserModel';
import { GetUserResponse } from '../services/user/models/GetUserResponse';
import { EditUserRequest } from '../services/user/models/EditUserRequest';

@Injectable({ providedIn: 'root' })
export class UserApiRepository extends GenericApiRepository {
    constructor(http: HttpClient) {
        super('users', http);
    }
    
    public getAllUsers(): Observable<UserModel[]>{
        return this.getAll<UserModel[]>();
    }
    
    public getUserById(id: string): Observable<GetUserResponse>{
        return this.getById<GetUserResponse>(id);
    }
    
    public createUser(user: CreateUserRequest): Observable<CreateUserResponse>{
        return this.create(user);
    }

    public updateUser(id: string, body: EditUserRequest): Observable<void> {
        return this.updateById(id, body);
    }
    
    public deleteUser(id: string): Observable<void>{
        return this.deleteById(id);
    }
}
