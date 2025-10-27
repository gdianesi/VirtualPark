import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RewardService } from '../../../backend/services/reward.service';
import { RewardRedemptionService } from '../../reward/reward-redemption.service';
import { ButtonsComponent } from '../../components/buttons/buttons.component';

@Component({
  selector: 'app-reward-redemption',
  standalone: true,
  imports: [CommonModule, ButtonsComponent],
  templateUrl: './reward-redemption.component.html',
  styleUrls: ['./reward-redemption.component.css']
})
export class RewardRedemptionComponent implements OnInit {
  rewards: any[] = [];
  loading = false;

  constructor(
    private rewardService: RewardService,
    private redemptionService: RewardRedemptionService
  ) {}

  ngOnInit(): void {
    this.loadRewards();
  }

  loadRewards(): void {
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

  redeem(reward: any): void {
    const redemption = {
      rewardId: reward.id,
      date: new Date().toISOString().split('T')[0],
      pointsSpent: reward.cost
    };

    this.redemptionService.redeemReward(redemption).subscribe({
      next: () => alert(`${reward.name} redeemed successfully!`),
      error: (err) => alert('Error redeeming reward: ' + err.message)
    });
  }
}
