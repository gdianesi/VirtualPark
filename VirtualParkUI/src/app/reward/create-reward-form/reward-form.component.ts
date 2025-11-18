import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { RewardService } from '../../../backend/services/reward/reward.service';
import { CreateRewardRequest } from '../../../backend/services/reward/models/CreateRewardRequest';
import { ButtonsComponent } from '../../components/buttons/buttons.component';
import { MessageComponent } from "../../components/messages/message.component";
import { MessageService } from '../../../backend/services/message/message.service';

@Component({
  selector: 'app-reward-form',
  standalone: true,
  imports: [CommonModule, FormsModule, ButtonsComponent, MessageComponent],
  templateUrl: './reward-form.component.html',
  styleUrls: ['./reward-form.component.css']
})
export class RewardFormComponent {
  
  reward: Partial<CreateRewardRequest> = {
    name: '',
    description: '',
    cost: "0",
    quantityAvailable: "0",
    membership: ''
  };

  loading = false;
    private messageService = inject(MessageService);

  constructor(
    private readonly rewardService: RewardService,
    private readonly router: Router
  ) {}

onSubmit(): void {
  this.loading = true;

  if (!this.reward.name || !this.reward.description || !this.reward.membership) {
    this.messageService.show('Please fill all required fields.', 'error');
    this.loading = false;
    return;
  }

  const qty = Number(this.reward.quantityAvailable);
  const cost = Number(this.reward.cost);

  if (isNaN(cost) || cost < 0) {
    this.messageService.show('Cost must be a valid non-negative number.', 'error');
    this.loading = false;
    return;
  }

  if (isNaN(qty) || qty <= 0) {
    this.messageService.show('Quantity Available must be greater than 0.', 'error');
    this.loading = false;
    return;
  }

  const payload: CreateRewardRequest = {
    name: this.reward.name ?? '',
    description: this.reward.description ?? '',
    cost: String(cost),
    quantityAvailable: String(qty),
    membership: this.reward.membership ?? ''
  };

  this.rewardService.create(payload).subscribe({
    next: () => {
      this.messageService.show('Reward created successfully', 'success');
      this.router.navigate(['/rewards']);
    },
    error: err => {
      const msg = err?.error?.message || 'Failed to create reward';
      this.messageService.show(msg, 'error');
      this.loading = false;
    }
  });
}


  onCancel(): void {
    this.router.navigate(['/rewards']);
  }
}
