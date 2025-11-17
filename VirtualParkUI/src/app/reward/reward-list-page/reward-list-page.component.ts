import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

import { RewardService } from '../../../backend/services/reward/reward.service';
import { RewardModel } from '../../../backend/services/reward/models/RewardModel';

import { ButtonsComponent } from '../../components/buttons/buttons.component';
import { ConfirmDialogComponent } from '../../components/confirm-dialog/confirm-dialog.component';
import { MessageService } from '../../components/messages/service/message.service';

@Component({
  selector: 'app-reward-page',
  standalone: true,
  templateUrl: './reward-list-page.component.html',
  styleUrls: ['./reward-list-page.component.css'],
  imports: [
    CommonModule,
    ButtonsComponent,
    ConfirmDialogComponent
  ]
})
export class RewardListPageComponent implements OnInit {

  rewards: RewardModel[] = [];
  loading = true;

  showDialog = false;
  pendingDeleteId: string | null = null;

  constructor(
    private readonly rewardService: RewardService,
    private readonly router: Router,
    private readonly messageService: MessageService
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

  askDelete(id: string) {
    this.pendingDeleteId = id;
    this.showDialog = true;
  }

  onConfirmDelete(result: boolean) {
    this.showDialog = false;

    if (!result || !this.pendingDeleteId) {
      this.pendingDeleteId = null;
      return;
    }

    this.rewardService.remove(this.pendingDeleteId).subscribe({
      next: () => {
        this.messageService.show('Reward deleted successfully', 'success');
        this.loadRewards();
      },
      error: () => {
        this.messageService.show('Failed to delete reward', 'error');
      }
    });

    this.pendingDeleteId = null;
  }
}
