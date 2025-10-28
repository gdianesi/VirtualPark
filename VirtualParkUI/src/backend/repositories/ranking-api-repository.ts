import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import GenericApiRepository from './generic-api-repository';

@Injectable({
  providedIn: 'root'
})
export class RankingRepository extends GenericApiRepository {
  constructor(http: HttpClient) {
    super('ranking', http);
  }

  public getByFilter<T>(date: string, period: string): Observable<T> {
    const url = `${this.baseUrl}/filter?date=${date}&period=${period}`;
    return this.http.get<T>(url, this.requestOptions());
  }
}
