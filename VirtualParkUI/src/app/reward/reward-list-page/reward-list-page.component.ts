import { Component, OnInit } from '@angular/core';
import { RewardService } from '../../../backend/services/reward/reward.service';
import { Router } from '@angular/router';
import { RewardModel } from '../../../backend/services/reward/models/RewardModel';
import { ButtonsComponent } from '../../components/buttons/buttons.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-reward-page',
  standalone: true,
  templateUrl: './reward-list-page.component.html',
  styleUrl: './reward-list-page.component.css',
  imports: [ButtonsComponent, CommonModule]
})

export class RewardListPageComponent implements OnInit {
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
