import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { RankingModel } from './models/RankingModel';
import { GetRankingRequest } from './models/GetRankingRequest';
import { RankingRepository } from '../../repositories/ranking-api-repository';

@Injectable({
  providedIn: 'root'
})
export class RankingService {
  constructor(private readonly _rankingRepository: RankingRepository) {}

  getAll(): Observable<RankingModel[]> {
    return this._rankingRepository.getAllRankings();
  }

  getFiltered(request: GetRankingRequest): Observable<RankingModel> {
    return this._rankingRepository.getFilteredRanking(request);
  }
}
