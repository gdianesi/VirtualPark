import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import GenericApiRepository from './generic-api-repository'
import { RoleModel } from '../services/role/models/RoleModel'
import { CreateRoleRequest } from '../services/role/models/CreateRoleRequest';
import { CreateRoleResponse } from '../services/role/models/CreateRoleResponse';

@Injectable({providedIn: 'root'})
export class RoleApiRepository extends GenericApiRepository{
    constructor(http: HttpClient) {
        super('roles', http);
    }
    
    public getAllRoles(): Observable<RoleModel[]>{
        return this.getAll<RoleModel[]>();
    }
    
    public getRoleById(id: string): Observable<RoleModel>{
        return  this.getById<RoleModel>(id);
    }
    
    public createRole(body: CreateRoleRequest): Observable<CreateRoleResponse>{
        return this.create<CreateRoleResponse>(body);
    }
    
    public updateRole(id: string, body: CreateRoleRequest): Observable<void>{
        return this.updateById<void>(id, body);
    }
    
    public deleteRole(id: string): Observable<void>{
        return this.deleteById<void>(id);
    }
}