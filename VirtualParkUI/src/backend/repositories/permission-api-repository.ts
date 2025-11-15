import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import GenericApiRepository from './generic-api-repository';
import { PermissionModel } from '../services/permission/models/PermissionModel';

@Injectable({ providedIn: 'root' })
export class PermissionApiRepository extends GenericApiRepository {
  constructor(http: HttpClient) {
    super('permissions', http);
  }
  
    getAllPermissions(): Observable<PermissionModel[]> {
        return this.getAll<PermissionModel[]>();
    }

    getPermissionById(id: string): Observable<PermissionModel> {
        return this.getById<PermissionModel>(id);
    }
}