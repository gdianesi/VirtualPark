import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { RankingService } from '../../../backend/services/ranking/ranking.service';
import { Ranking } from '../../../backend/services/ranking/models/ranking.model';
import { ButtonsComponent } from '../../components/buttons/buttons.component';

@Component({
  selector: 'app-ranking-detail',
  standalone: true,
  imports: [CommonModule, ButtonsComponent],
  templateUrl: './ranking-detail.component.html',
  styleUrls: ['./ranking-detail.component.css']
})
export class RankingDetailComponent implements OnInit {
  ranking: Ranking | null = null;
  loading = false;

  constructor(
    private route: ActivatedRoute,
    private rankingService: RankingService
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) this.loadRanking(id);
  }

  loadRanking(id: string): void {
    this.loading = true;
    this.rankingService.getById(id).subscribe({
      next: (data) => {
        this.ranking = data;
        this.loading = false;
      },
      error: (err) => {
        console.error('Error loading ranking:', err);
        this.loading = false;

        // Mock temporal (para probar la vista)
        this.ranking = {
          id,
          date: '2025-10-25',
          period: 'Daily',
          users: ['Juan Pérez', 'Ana López', 'Carlos García', 'Lucía Torres']
        };
      }
    });
  }
}
