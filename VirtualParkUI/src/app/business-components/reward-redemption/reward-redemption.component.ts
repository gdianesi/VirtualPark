import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RewardService } from '../../reward/reward.service';
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
  visitorId = '11111111-1111-1111-1111-111111111111'; // temporal
  loading = false;

  constructor(
    private rewardService: RewardService,
    private redemptionService: RewardRedemptionService
  ) {}

  ngOnInit(): void {
    this.loadRewards();

      this.rewards = [
    {
      id: '11111111-aaaa-bbbb-cccc-111111111111',
      name: 'VIP Pass',
      cost: 500,
      quantityAvailable: 10
    },
    {
      id: '22222222-aaaa-bbbb-cccc-222222222222',
      name: 'Combo Familiar',
      cost: 300,
      quantityAvailable: 5
    },
    {
      id: '33333333-aaaa-bbbb-cccc-333333333333',
      name: 'Entrada Premium',
      cost: 200,
      quantityAvailable: 8
    }
  ];
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
      visitorId: this.visitorId,
      date: new Date().toISOString().split('T')[0],
      pointsSpent: reward.cost
    };

    this.redemptionService.redeemReward(redemption).subscribe({
      next: () => alert(`${reward.name} redeemed successfully!`),
      error: (err) => alert('Error redeeming reward: ' + err.message)
    });
  }
}
