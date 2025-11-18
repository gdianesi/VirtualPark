import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { VisitScoreService } from '../../../backend/services/visitScore/visit-score.service';
import { MessageComponent } from '../../components/messages/message.component';
import { MessageService } from '../../../backend/services/message/message.service';
import { GetVisitScoreResponse } from '../../../backend/services/visitScore/models/GetVisitScoreResponse';

type ScoreRow = {
  id: string;
  origin: string;
  occurredAt: string;
  points: number;
  strategy: string;
};

@Component({
  selector: 'app-visit-score-history',
  standalone: true,
  imports: [CommonModule, MessageComponent],
  templateUrl: './visit-score-history.component.html',
  styleUrls: ['./visit-score-history.component.css']
})
export class VisitScoreHistoryComponent implements OnInit {
  private scoreService = inject(VisitScoreService);
  private messageService = inject(MessageService);

  loading = true;
  scores: ScoreRow[] = [];
  visitorId: string | null = null;

  ngOnInit(): void {
    this.visitorId = localStorage.getItem('visitorId');
    if (!this.visitorId) {
      this.loading = false;
      this.messageService.show('You need an active visit to view the score history.', 'error');
      return;
    }

    this.scoreService.getHistoryByVisitor(this.visitorId).subscribe({
      next: (items: GetVisitScoreResponse[]) => {
        this.scores = (items ?? []).map(it => ({
          id: it.id,
          origin: it.origin,
          occurredAt: new Date(it.occurredAt).toLocaleString(),
          points: it.points,
          strategy: it.dayStrategyName,
        }));
        this.loading = false;
      },
      error: err => {
        this.loading = false;
        const msg = err?.error?.message ?? err?.message ?? 'Unable to load the score history.';
        this.messageService.show(msg, 'error');
      }
    });
  }
}
