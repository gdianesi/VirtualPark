import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CloackModel } from './models/CloackModel';
import { CloackRepository } from '../../repositories/cloack-api-repository';


@Injectable({ providedIn: 'root' })
export class CloackService {
  constructor(private readonly _repo: CloackRepository) {}

  get(): Observable<CloackModel> {
    return this._repo.getClock();
  }

  update(cloack: CloackModel): Observable<void> {
    return this._repo.updateClock(cloack);
  }
}
