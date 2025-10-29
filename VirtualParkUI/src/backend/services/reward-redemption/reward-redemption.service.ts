import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { RewardRedemptionModel } from './models/RewardRedemptionModel';
import { CreateRewardRedemptionRequest } from './models/CreateRewardRedemptionRequest';
import { CreateRewardRedemptionResponse } from './models/CreateRewardRedemptionResponse';
import { RewardRedemptionRepository } from '../../repositories/rewardRedemption-api-repository';

@Injectable({
  providedIn: 'root'
})
export class RewardRedemptionService {
    constructor(private readonly _RewardRedemptionRepository: RewardRedemptionRepository) {
    }
  getAll(): Observable<RewardRedemptionModel[]> {
    return this._RewardRedemptionRepository.getAllRewardRedemptions();
  }

  getById(id: string): Observable<RewardRedemptionModel> {
    return this._RewardRedemptionRepository.getRewardRedemptionById(id);
  }

  create(RewardRedemption: CreateRewardRedemptionRequest): Observable<CreateRewardRedemptionResponse> {
    return this._RewardRedemptionRepository.createRewardRedemption(RewardRedemption);
  }

    getByVisitor(id: string): Observable<RewardRedemptionModel[]> {
    return this._RewardRedemptionRepository.getRewardRedemptionsByVisitor(id);
  }
}



