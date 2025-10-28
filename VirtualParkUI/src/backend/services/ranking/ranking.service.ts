import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { RankingRepository } from '../../repositories/ranking-api-repository';
import { Ranking } from '../../services/ranking/models/ranking.model';

@Injectable({
  providedIn: 'root'
})
export class RankingService {
  constructor(private repo: RankingRepository) {}

  getAll(): Observable<Ranking[]> {
    return this.repo.getAll<Ranking[]>();
  }

  getById(id: string): Observable<Ranking> {
    return this.repo.getById<Ranking>(id);
  }

  getByFilter(date: string, period: string): Observable<Ranking> {
    return this.repo.getByFilter<Ranking>(date, period);
  }

  create(ranking: Partial<Ranking>): Observable<Ranking> {
    return this.repo.create<Ranking>(ranking);
  }

  delete(id: string): Observable<void> {
    return this.repo.deleteById<void>(id);
  }
}

