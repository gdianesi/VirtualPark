import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PermissionModel } from './models/PermissionModel';
import { PermissionApiRepository } from '../../repositories/permission-api-repository';

@Injectable({ providedIn: 'root' })
export class PermissionService {
  constructor(private readonly _repo: PermissionApiRepository) {}

    getAll(): Observable<PermissionModel[]> {
        return this._repo.getAllPermissions();
    }

    getById(id: string): Observable<PermissionModel> {
        return this._repo.getPermissionById(id);
    }
}