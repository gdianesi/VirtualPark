import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RewardService } from '../../../backend/services/reward/reward.service';
import { RewardRedemptionService } from '../../../backend/services/reward-redemption/reward-redemption.service';
import { RewardModel } from '../../../backend/services/reward/models/RewardModel';
import { CreateRewardRedemptionRequest } from '../../../backend/services/reward-redemption/models/CreateRewardRedemptionRequest';
import { ButtonsComponent } from '../../components/buttons/buttons.component';
import { MessageService } from '../../components/messages/service/message.service';
import { MessageComponent } from "../../components/messages/message.component";

@Component({
  selector: 'app-reward-redemption',
  standalone: true,
  imports: [CommonModule, ButtonsComponent, MessageComponent],
  templateUrl: './reward-redemption.component.html',
  styleUrls: ['./reward-redemption.component.css']
})
export class RewardRedemptionComponent implements OnInit {
  rewards: RewardModel[] = [];
  loading = false;
  visitorId = localStorage.getItem("visitorId")!;

  constructor(
    private readonly rewardService: RewardService,
    private readonly redemptionService: RewardRedemptionService,
    private readonly messageService: MessageService
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
      next: () => this.messageService.show(`${reward.name} redeemed successfully!`, 'success'),
      error: (err) => this.messageService.show(err.message || 'Error redeeming reward.', 'error')
    });
  }
}
