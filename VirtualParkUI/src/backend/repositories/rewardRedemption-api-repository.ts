import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import GenericApiRepository from './generic-api-repository';

@Injectable({
  providedIn: 'root'
})
export class RewardRedemptionRepository extends GenericApiRepository {
  constructor(http: HttpClient) {
    super('rewards/redemptions', 'http://localhost:5104', http);
  }
}
