import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import GenericApiRepository from './generic-api-repository';
import { RewardModel } from '../services/reward/models/RewardModel';
import { CreateRewardRequest } from '../services/reward/models/CreateRewardRequest';
import { CreateRewardResponse } from '../services/reward/models/CreateRewardResponse';

@Injectable({ providedIn: 'root' })
export class RewardRepository extends GenericApiRepository {
  constructor(http: HttpClient) {
    super('rewards',  http);
  }

  public getAllRewards(): Observable<RewardModel[]> {
    return this.getAll<RewardModel[]>();
  }

  public getRewardById(id: string): Observable<RewardModel> {
    return this.getById<RewardModel>(id);
  }

public createReward(body: CreateRewardRequest): Observable<CreateRewardResponse> {
    return this.create<CreateRewardResponse>(body);
  }

  public updateReward(id: string, body: CreateRewardRequest): Observable<void> {
    return this.updateById<void>(id, body);
  }

  public deleteReward(id: string): Observable<void> {
    return this.deleteById<void>(id);
  }
  public getDeletedRewards(): Observable<RewardModel[]> {
    return this.getAll<RewardModel[]>('deleted');
  }

  public restoreReward(id: string, quantity: number): Observable<void> {
    return this.patchById<void>(`${id}/restore`, { quantityAvailable: quantity.toString() });
  }
}
