import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RewardRedemptionService {
  private apiUrl = 'http://localhost:5104/rewards/redemptions';

  constructor(private http: HttpClient) {}

  redeemReward(data: any): Observable<any> {
    return this.http.post(this.apiUrl, data);
  }

  getRedemptionsByVisitor(visitorId: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/visitor/${visitorId}`);
  }

  getAllRedemptions(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }
}
