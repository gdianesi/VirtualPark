import { Component } from '@angular/core';
import { RewardService, Reward } from '../../reward/reward.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-reward-form',
  templateUrl: './reward-form.component.html',
  styleUrls: ['./reward-form.component.css'],
  standalone: false
})
export class RewardFormComponent {
  reward: Partial<Reward> = {
    name: '',
    description: '',
    cost: 0,
    quantityAvailable: 0,
    requiredMembershipLevel: ''
  };

  constructor(
    private rewardService: RewardService,
    private router: Router
  ) {}

  onSubmit(): void {
    this.rewardService.create(this.reward as Reward).subscribe({
      next: () => {
        alert('Reward successfully created');
        this.router.navigate(['/rewards']);
      },
      error: (err) => {
        console.error('Error creating reward:', err);
        alert('Error creating reward:');
      }
    });
  }

  onCancel(): void {
    this.router.navigate(['/rewards']);
  }
}

