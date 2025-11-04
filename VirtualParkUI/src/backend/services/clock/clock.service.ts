import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ClockModel } from './models/ClockModel';
import { ClockRepository } from '../../repositories/clock-api-repository';


@Injectable({ providedIn: 'root' })
export class ClockService {
  constructor(private readonly _repo: ClockRepository) {}

  get(): Observable<ClockModel> {
    return this._repo.getClock();
  }

  update(clock: ClockModel): Observable<void> {
    return this._repo.updateClock(clock);
  }
}
