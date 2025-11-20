import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import GenericApiRepository from './generic-api-repository';

@Injectable({ providedIn: 'root' })
export class IncidenceRepository extends GenericApiRepository {
  constructor(http: HttpClient) {
    super('incidences', http);
  }

  deleteIncidence(id: string) {
    return this.deleteById<void>(id);
  }

  updateIncidence(id: string, body: any) {
    return this.updateById<void>(id, body);
  }
}
