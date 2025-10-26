import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { RewardRedemptionRepository } from '../../backend/repositories/rewardRedemption-api-repository';

@Injectable({
  providedIn: 'root'
})
export class RewardRedemptionService {
  constructor(private repo: RewardRedemptionRepository) {}

  getAll(): Observable<any[]> {
    return this.repo.getAll<any[]>();
  }

  getRedemptionsByVisitor(visitorId: string): Observable<any[]> {
    return this.repo.getById<any[]>(`visitor/${visitorId}`);
  }

  redeemReward(data: any): Observable<any> {
    return this.repo.create<any>(data);
  }
}


