import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { RewardRepository } from '../../repositories/reward-api-repository';

@Injectable({
  providedIn: 'root'
})
export class RewardService {
  constructor(private repo: RewardRepository) {}

  getAll(): Observable<any[]> {
    return this.repo.getAll<any[]>();
  }

  getById(id: string): Observable<any> {
    return this.repo.getById<any>(id);
  }

  create(data: any): Observable<any> {
    return this.repo.create<any>(data);
  }

  update(id: string, data: any): Observable<any> {
    return this.repo.updateById<any>(id, data);
  }

  delete(id: string): Observable<any> {
    return this.repo.deleteById<any>(id);
  }
}
