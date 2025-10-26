import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

export interface Reward {
  id: string;
  name: string;
  description: string;
  cost: number;
  quantityAvailable: number;
  requiredMembershipLevel: string;
}

@Injectable({
  providedIn: 'root'
})
export class RewardService {
  private apiUrl = 'http://localhost:5104/rewards';

  constructor(private http: HttpClient) {}

  getAll(): Observable<Reward[]> {
    return this.http.get<Reward[]>(this.apiUrl);
  }

  create(reward: Reward): Observable<Reward> {
    return this.http.post<Reward>(this.apiUrl, reward);
  }
}
