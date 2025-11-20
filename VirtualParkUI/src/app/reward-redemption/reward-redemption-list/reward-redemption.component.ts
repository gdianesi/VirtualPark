import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RewardService } from '../../../backend/services/reward/reward.service';
import { RewardRedemptionService } from '../../../backend/services/reward-redemption/reward-redemption.service';
import { RewardModel } from '../../../backend/services/reward/models/RewardModel';
import { CreateRewardRedemptionRequest } from '../../../backend/services/reward-redemption/models/CreateRewardRedemptionRequest';
import { ButtonsComponent } from '../../components/buttons/buttons.component';
import { MessageService } from '../../../backend/services/message/message.service';
import { MessageComponent } from "../../components/messages/message.component";
import { VisitorProfileService } from '../../../backend/services/visitorProfile/visitorProfile.service';
import { VisitorProfileModel } from '../../../backend/services/visitorProfile/models/VisitorProfileModel';
import { VisitRegistrationService } from '../../../backend/services/visitRegistration/visit-registration.service';
import { VisitScoreRequest } from '../../../backend/services/visitRegistration/models/VisitScoreRequest';

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
  visitorScore: number = 0;
  private visitRegistrationId: string | null = null;

  constructor(
    private readonly rewardService: RewardService,
    private readonly redemptionService: RewardRedemptionService,
    private readonly messageService: MessageService,
    private readonly visitorService: VisitorProfileService,
    private readonly visitRegistrationService: VisitRegistrationService
  ) {}

  ngOnInit(): void {
    this.loadVisitor();
    this.loadRewards();
    this.loadVisitRegistration();
  }

  private loadVisitor(): void {
    this.visitorService.getById(this.visitorId).subscribe({
      next: v => this.visitorScore = Number(v.pointsAvailable),
      error: () => this.messageService.show("Error loading visitor profile", "error")
    });
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

  private loadVisitRegistration(): void {
    if (!this.visitorId) return;

    this.visitRegistrationService.getTodayVisit(this.visitorId).subscribe({
      next: resp => this.visitRegistrationId = resp.visitRegistrationId,
      error: () => this.messageService.show('Unable to load today visit information.', 'error')
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
      next: () => {
        this.messageService.show(`${reward.name} redeemed successfully!`, 'success');
        this.recordRedeemScore(reward);
        this.loadRewards();
      },
      error: (err) => this.messageService.show(err.error?.message || 'Error redeeming reward.', 'error')
    });
  }

  private recordRedeemScore(reward: RewardModel): void {
    if (!this.visitRegistrationId) {
      this.messageService.show('Visit information is missing. Please try again later.', 'error');
      return;
    }

    const payload: VisitScoreRequest = {
      visitRegistrationId: this.visitRegistrationId,
      origin: 'Canje',
      points: reward.cost?.toString() ?? '0'
    };

    this.visitRegistrationService.recordScoreEvent(payload).subscribe({
      error: err => {
        console.error('Error registering score event for redeem', err);
      }
    });
  }

}
