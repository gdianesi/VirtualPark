import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import GenericApiRepository from './generic-api-repository';

@Injectable({
  providedIn: 'root'
})
export class RewardRepository extends GenericApiRepository {
  constructor(http: HttpClient) {
    super('rewards', http);
  }
}
