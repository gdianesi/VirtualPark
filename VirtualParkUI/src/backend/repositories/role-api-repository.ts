import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import GenericRepository from './generic-api-repository'
import { RoleModel } from '../services/role/models/RoleModel'
import { CreateRoleRequest } from '../services/role/models/CreateRoleRequest';
import { CreateRoleResponse } from '../services/role/models/CreateRoleResponse';

@Injectable({providedIn: 'root'})
export class RoleApiRepository extends GenericRepository{
    constructor(http: HttpClient) {
        super('roles', http);
    }
    
    
}