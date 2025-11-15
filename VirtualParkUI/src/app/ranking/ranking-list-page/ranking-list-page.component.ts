import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RankingService } from '../../../backend/services/ranking/ranking.service';
import { GetRankingRequest } from '../../../backend/services/ranking/models/GetRankingRequest';
import { RankingModel } from '../../../backend/services/ranking/models/RankingModel';
import { ButtonsComponent } from '../../components/buttons/buttons.component';
import { MessageComponent } from '../../components/messages/message.component';
import { MessageService } from '../../components/messages/service/message.service';
import { UserService } from '../../../backend/services/user/user.service';
import { UserModel } from '../../../backend/services/user/models/UserModel';

@Component({
  selector: 'app-ranking-page',
  standalone: true,
  imports: [CommonModule, FormsModule, ButtonsComponent, MessageComponent],
  templateUrl: './ranking-list-page.component.html',
  styleUrls: ['./ranking-list-page.component.css']
})
export class RankingListPageComponent implements OnInit {
  ranking: RankingModel | null = null;
  date = new Date().toISOString().split('T')[0];
  period = 'Daily';
  loading = false;

  userMap: Record<string, string> = {};

  constructor(
    private readonly rankingService: RankingService,
    private readonly userService: UserService,
    private readonly messageService: MessageService
  ) {}

  ngOnInit(): void {
    this.loadUsers();
  }

  private loadUsers(): void {
    this.userService.getAll().subscribe({
      next: (users: UserModel[]) => {
        this.userMap = users.reduce((map, user) => {
          map[user.id] = user.name;
          return map;
        }, {} as Record<string, string>);
      },
      error: () => this.messageService.show('Error loading users.', 'error')
    });
  }

  loadRanking(): void {
    this.loading = true;
    this.ranking = null;

    const request: GetRankingRequest = { date: this.date, period: this.period };

    this.rankingService.getFiltered(request).subscribe({
      next: (data) => {
        this.ranking = data;
        this.loading = false;
        if (!data.users || data.users.length === 0) {
          this.messageService.show('No ranking data for this date and period.', 'info');
        }
      },
      error: (err) => {
        this.loading = false;
        this.messageService.show('Error loading ranking: ' + err.message, 'error');
      }
    });
  }

  getUserName(userId: string): string {
    return this.userMap[userId] || userId;
  }

  getScoreFor(userId: string, index: number): string {
  if (!this.ranking || !this.ranking.scores) return '0';
  return this.ranking.scores[index] || '0';
}
}
