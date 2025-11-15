import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ButtonsComponent } from '../../components/buttons/buttons.component';
import { MessageComponent } from '../../components/messages/message.component';
import { StrategyService } from '../../../backend/services/strategy/strategy.service';
import { GetStrategiesKeyResponse } from '../../../backend/services/strategy/models/GetStrategiesKeyResponse';
import { StrategyModel } from '../../../backend/services/strategy/models/StrategyModel';
import { ClockService } from '../../../backend/services/clock/clock.service';
import { switchMap } from 'rxjs/operators';
import { MessageService } from '../../components/messages/service/message.service';

@Component({
  selector: 'app-strategy-select-page',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ButtonsComponent,
    MessageComponent
  ],
  templateUrl: './strategy-select-page.component.html',
  styleUrls: ['./strategy-select-page.component.css']
})
export class StrategySelectPageComponent implements OnInit {
  strategies: GetStrategiesKeyResponse[] = [];
  selectedKey: string | null = null;
  isActivating: boolean = false;

  constructor(
    private _strategyService: StrategyService,
    private _clockService: ClockService,
    private _messageService: MessageService
  ) {}

  ngOnInit(): void {
    this._strategyService.getAllKeys().subscribe({
      next: (data) => { this.strategies = data; },
      error: (err) => {
        this._messageService.show(
          `Error fetching strategies: ${err.message || 'Please try again.'}`,
          'error'
        );
      }
    });
  }

  active(): void {
    if (!this.selectedKey) {
      this._messageService.show('First, you will select a strategy.', 'info');
      return;
    }

    this.isActivating = true;

    this._clockService.get().pipe(
      switchMap(clock => {
        const formattedDate = this._strategyService.formatDate(clock.dateSystem);
        const strategyModel: StrategyModel = {
          strategyKey: this.selectedKey!,
          date: formattedDate
        };
        return this._strategyService.create(strategyModel);
      })
    ).subscribe({
      next: () => {
        this._messageService.show('Strategy activated successfully!', 'success');
        this.isActivating = false;
      },
      error: (err) => {
        this._messageService.show(
          `Error activating strategy: ${err.message || 'Please try again.'}`,
          'error'
        );
        this.isActivating = false;
      }
    });
  }
}
