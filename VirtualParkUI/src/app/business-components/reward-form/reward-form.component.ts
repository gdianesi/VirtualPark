import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { RewardService } from '../../../backend/services/reward/reward.service';
import { CreateRewardRequest } from '../../../backend/services/reward/models/CreateRewardRequest';
import { ButtonsComponent } from '../../components/buttons/buttons.component';

@Component({
  selector: 'app-reward-form',
  standalone: true,
  imports: [CommonModule, FormsModule, ButtonsComponent],
  templateUrl: './reward-form.component.html',
  styleUrls: ['./reward-form.component.css']
})
export class RewardFormComponent {
  reward: Partial<CreateRewardRequest> = {
    name: '',
    description: '',
    cost: 0,
    quantityAvailable: 0,
    requiredMembershipLevel: ''
  };

  loading = false;

  constructor(
    private readonly rewardService: RewardService,
    private readonly router: Router
  ) {}

  onSubmit(): void {
    this.loading = true;
    this.rewardService.create(this.reward as CreateRewardRequest).subscribe({
      next: () => {
        alert('Reward created successfully');
        this.router.navigate(['/rewards']);
      },
      error: err => {
        console.error('Error creating reward:', err);
        alert('Failed to create reward');
        this.loading = false;
      }
    });
  }

  onCancel(): void {
    this.router.navigate(['/rewards']);
  }
}
