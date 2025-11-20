import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { RoleModel } from './models/RoleModel'
import { CreateRoleRequest } from './models/CreateRoleRequest';
import { CreateRoleResponse } from './models/CreateRoleResponse';
import { RoleApiRepository } from '../../repositories/role-api-repository';

@Injectable({
    providedIn: 'root'
})
export class RoleService {
    constructor(private readonly _roleRepository: RoleApiRepository) { }
    
    getAll(): Observable<RoleModel[]>{
        return this._roleRepository.getAllRoles();
    }
    
    getById(id: string): Observable<RoleModel>{
        return this._roleRepository.getRoleById(id);
    }
    
    create(role: CreateRoleRequest): Observable<CreateRoleResponse>{
        return this._roleRepository.createRole(role);
    }
    
    update(id: string, role: CreateRoleRequest): Observable<void>{
        return this._roleRepository.updateRole(id, role);
    }
    
    remove(id: string): Observable<void>{
        return this._roleRepository.deleteById(id);   
    }
}