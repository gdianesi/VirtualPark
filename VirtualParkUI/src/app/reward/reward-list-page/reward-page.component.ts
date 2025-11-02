import { Component, OnInit } from '@angular/core';
import { RewardService } from '../../../backend/services/reward/reward.service';
import { Router } from '@angular/router';
import { RewardModel } from '../../../backend/services/reward/models/RewardModel';

@Component({
  selector: 'app-reward-page',
  standalone: false,
  templateUrl: './reward-page.component.html',
  styleUrl: './reward-page.component.css',
})

export class RewardPageComponent implements OnInit {
  rewards: RewardModel[] = [];
  loading = true;

  constructor(
    private rewardService: RewardService, 
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadRewards();
  }

  loadRewards(): void {
    this.rewardService.getAll().subscribe({
      next: (data) => {
        this.rewards = data;
        this.loading = false;
      },
      error: (err) => {
        console.error('Error loading rewards', err);
        this.loading = false;
      }
    });
  }

  onCreateReward(): void {
    this.router.navigate(['/rewards/create']);
  }
}
