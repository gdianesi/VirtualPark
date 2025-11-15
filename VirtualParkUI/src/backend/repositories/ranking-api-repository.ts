import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import GenericApiRepository from './generic-api-repository';
import { RankingModel } from '../services/ranking/models/RankingModel';
import { GetRankingRequest } from '../services/ranking/models/GetRankingRequest';

@Injectable({ providedIn: 'root' })
export class RankingRepository extends GenericApiRepository {
  constructor(http: HttpClient) {
    super('rankings', http);
  }

  public getAllRankings(): Observable<RankingModel[]> {
    return this.getAll<RankingModel[]>();
  }

  public getFilteredRanking(request: GetRankingRequest): Observable<RankingModel> {
    const { date, period } = request;
    const params = `filter?date=${date}&period=${period}`;
    return this.http.get<RankingModel>(`${this.baseUrl}/${params}`);
  }
}
