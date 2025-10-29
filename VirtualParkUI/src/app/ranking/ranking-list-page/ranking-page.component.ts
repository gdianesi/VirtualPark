import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RankingService } from '../../../backend/services/ranking/ranking.service';
import { ButtonsComponent } from '../../components/buttons/buttons.component';
import { Ranking } from '../../../backend/services/ranking/models/ranking.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-ranking-page',
  standalone: true,
  imports: [CommonModule, ButtonsComponent],
  templateUrl: './ranking-page.component.html',
  styleUrls: ['./ranking-page.component.css']
})
export class RankingPageComponent implements OnInit {
  rankings: Ranking[] = [];
  loading = false;

  constructor(  private rankingService: RankingService,
  private router: Router) {}

  ngOnInit(): void {
    this.loadRankings();
  }

  loadRankings(): void {
    this.loading = true;
    this.rankingService.getAll().subscribe({
      next: (data) => {
        this.rankings = data;
        this.loading = false;
      },
      error: (err) => {
        console.error('Error loading rankings:', err);
        this.loading = false;
      }
    });
  }
  goToDetail(id: string): void {
    this.router.navigate(['/rankings', id]);
  }
}
