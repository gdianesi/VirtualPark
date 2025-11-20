import { Observable } from 'rxjs';
import { Injectable } from "@angular/core";
import { UserModel } from './models/UserModel';
import { CreateUserRequest } from './models/CreateUserRequest';
import { CreateUserResponse } from './models/CreateUserResponse';
import { UserApiRepository } from '../../repositories/user-api-repository';
import { GetUserResponse } from './models/GetUserResponse';
import { EditUserRequest } from './models/EditUserRequest';

@Injectable({
    providedIn: 'root'})
export class UserService{
    constructor(private readonly _userRepository: UserApiRepository) {}
    
    getAll(): Observable<UserModel[]>{
        return this._userRepository.getAllUsers();
    }
    
    getById(id: string): Observable<GetUserResponse>{
        return this._userRepository.getUserById(id);
    }
    
    create(user: CreateUserRequest): Observable<CreateUserResponse>{
        return this._userRepository.create(user);
    }
    
    update(id: string, user: EditUserRequest): Observable<void>{
        return this._userRepository.updateUser(id, user);
    }
    
    remove(id: string): Observable<void>{
        return this._userRepository.deleteById(id);
    }
}
