import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RewardService } from '../../../backend/services/reward/reward.service';
import { RewardModel } from '../../../backend/services/reward/models/RewardModel';
import { ButtonsComponent } from '../../components/buttons/buttons.component';
import { MessageComponent } from '../../components/messages/message.component';
import { MessageService } from '../../../backend/services/message/message.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-reward-deleted-page',
  standalone: true,
  templateUrl: './reward-deleted-page.component.html',
  styleUrls: ['./reward-deleted-page.component.css'],
  imports: [CommonModule, ButtonsComponent, MessageComponent, FormsModule]
})
export class RewardDeletedListPageComponent implements OnInit {

  rewards: RewardModel[] = [];
  loading = true;

  restoringId: string | null = null;
  restoringQuantity: string = '';

  constructor(
    private readonly rewardService: RewardService,
    private readonly messageService: MessageService
  ) {}

  ngOnInit(): void {
    this.loadDeletedRewards();
  }

  loadDeletedRewards() {
    this.rewardService.getDeleted().subscribe({
      next: (res) => {
        this.rewards = res;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
        this.messageService.show('Error loading deleted rewards.', 'error');
      }
    });
  }

  startRestore(id: string) {
    this.restoringId = id;
    this.restoringQuantity = '';
  }

  cancelRestore() {
    this.restoringId = null;
    this.restoringQuantity = '';
  }

  confirmRestore() {
    if (!this.restoringId) return;

    const qty = Number(this.restoringQuantity);
    if (isNaN(qty) || qty <= 0) {
      this.messageService.show('Quantity must be a positive number.', 'error');
      return;
    }

    this.rewardService.restore(this.restoringId, qty).subscribe({
      next: () => {
        this.messageService.show('Reward restored successfully!', 'success');
        this.restoringId = null;
        this.restoringQuantity = '';
        this.loadDeletedRewards();
      },
      error: () => {
        this.messageService.show('Error restoring reward.', 'error');
      }
    });
  }
}