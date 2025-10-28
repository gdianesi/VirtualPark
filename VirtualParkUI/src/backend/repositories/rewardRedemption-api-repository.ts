import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import GenericApiRepository from './generic-api-repository';
import { RewardRedemptionModel } from '../services/reward-redemption/models/RewardRedemptionModel';
import { CreateRewardRedemptionRequest } from '../services/reward-redemption/models/CreateRewardRedemptionRequest';
import { CreateRewardRedemptionResponse } from '../services/reward-redemption/models/CreateRewardRedemptionResponse';

@Injectable({ providedIn: 'root' })
export class RewardRedemptionRepository extends GenericApiRepository {
  constructor(http: HttpClient) {
    super('RewardRedemptions',  http);
  }

  public getAllRewardRedemptions(): Observable<RewardRedemptionModel[]> {
    return this.getAll<RewardRedemptionModel[]>();
  }

  public getRewardRedemptionRedemptionById(id: string): Observable<RewardRedemptionModel> {
    return this.getById<RewardRedemptionModel>(id);
  }

public createRewardRedemption(body: CreateRewardRedemptionRequest): Observable<CreateRewardRedemptionResponse> {
    return this.create<CreateRewardRedemptionResponse>(body);
  }

  public updateRewardRedemption(id: string, body: CreateRewardRedemptionRequest): Observable<void> {
    return this.updateById<void>(id, body);
  }

  public deleteRewardRedemption(id: string): Observable<void> {
    return this.deleteById<void>(id);
  }
}
