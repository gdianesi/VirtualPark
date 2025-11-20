import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RewardRedemptionService } from '../../../backend/services/reward-redemption/reward-redemption.service';
import { RewardRedemptionModel } from '../../../backend/services/reward-redemption/models/RewardRedemptionModel';
import { RewardService } from '../../../backend/services/reward/reward.service';
import { RewardModel } from '../../../backend/services/reward/models/RewardModel';
import { Router } from '@angular/router';
import { ButtonsComponent } from '../../components/buttons/buttons.component';

@Component({
  selector: 'app-reward-redemption-history',
  standalone: true,
  imports: [CommonModule, ButtonsComponent],
  templateUrl: './reward-redemption-history.component.html',
  styleUrls: ['./reward-redemption-history.component.css']
})
export class RewardRedemptionHistoryComponent implements OnInit {
  redemptions: RewardRedemptionModel[] = [];
  rewards: RewardModel[] = [];
  loading = false;
  error = '';
  visitorId: string | null = null;

  constructor(
    private readonly redemptionService: RewardRedemptionService,
    private readonly rewardService: RewardService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.visitorId = localStorage.getItem('visitorId');

    if (!this.visitorId) {
      this.error = 'No visitor profile found for this user.';
      return;
    }

    this.loadHistory();
  }

  private loadHistory(): void {
    this.loading = true;

    this.rewardService.getAll().subscribe({
      next: rewards => {
        this.rewards = rewards;
        this.loadVisitorRedemptions();
      },
      error: err => {
        this.error = 'Error loading rewards';
        console.error(err);
        this.loading = false;
      }
    });
  }

  private loadVisitorRedemptions(): void {
    this.redemptionService.getByVisitor(this.visitorId!).subscribe({
      next: data => {
        this.redemptions = data;
        this.loading = false;
      },
      error: err => {
        this.error = 'Error loading redemptions';
        console.error(err);
        this.loading = false;
      }
    });
  }

  getRewardName(rewardId: string): string {
    const found = this.rewards.find(r => r.id === rewardId);
    return found ? found.name : '(Unknown reward)';
  }

  onBack(): void {
  this.router.navigate(['/reedem']);
}
}
