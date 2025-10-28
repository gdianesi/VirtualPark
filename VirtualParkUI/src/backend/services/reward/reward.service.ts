import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { RewardModel } from './models/RewardModel';
import { CreateRewardRequest } from './models/CreateRewardRequest';
import { CreateRewardResponse } from './models/CreateRewardResponse';
import { RewardRepository } from '../../repositories/reward-api-repository';

@Injectable({
  providedIn: 'root'
})
export class RewardService {
    constructor(private readonly _RewardRepository: RewardRepository) {
    }
  getAll(): Observable<RewardModel[]> {
    return this._RewardRepository.getAllRewards();
  }

  getById(id: string): Observable<RewardModel> {
    return this._RewardRepository.getRewardById(id);
  }

  create(Reward: CreateRewardRequest): Observable<CreateRewardResponse> {
    return this._RewardRepository.createReward(Reward);
  }

  update(id: string, Reward: CreateRewardRequest): Observable<void> {
    return this._RewardRepository.updateById(id, Reward)
  }

  remove(id: string): Observable<void> {
    return this._RewardRepository.deleteReward(id);
  }
}

