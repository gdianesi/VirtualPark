import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RewardService } from '../../../backend/services/reward/reward.service';
import { RewardRedemptionService } from '../../../backend/services/reward-redemption/reward-redemption.service';
import { RewardModel } from '../../../backend/services/reward/models/RewardModel';
import { CreateRewardRedemptionRequest } from '../../../backend/services/reward-redemption/models/CreateRewardRedemptionRequest';
import { ButtonsComponent } from '../../components/buttons/buttons.component';

@Component({
  selector: 'app-reward-redemption',
  standalone: true,
  imports: [CommonModule, ButtonsComponent],
  templateUrl: './reward-redemption.component.html',
  styleUrls: ['./reward-redemption.component.css']
})
export class RewardRedemptionComponent implements OnInit {
  rewards: RewardModel[] = [];
  loading = false;
  visitorId = '11111111-1111-1111-1111-111111111111';

  constructor(
    private readonly rewardService: RewardService,
    private readonly redemptionService: RewardRedemptionService
  ) {}

  ngOnInit(): void {
    this.loadRewards();
  }

  private loadRewards(): void {
    this.loading = true;
    this.rewardService.getAll().subscribe({
      next: (data) => {
        this.rewards = data;
        this.loading = false;
      },
      error: (err) => {
        console.error('Error loading rewards:', err);
        this.loading = false;
      }
    });
  }

  redeem(reward: RewardModel): void {
    const redemption: CreateRewardRedemptionRequest = {
      rewardId: reward.id,
      visitorId: this.visitorId,
      date: new Date().toISOString().split('T')[0],
      pointsSpent: reward.cost.toString()
    };

    this.redemptionService.create(redemption).subscribe({
      next: () => alert(`${reward.name} redeemed successfully!`),
      error: (err) => {
        console.error('Error redeeming reward:', err);
        alert('Error redeeming reward: ' + err.message);
      }
    });
  }
}
