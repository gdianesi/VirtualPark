import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ButtonsComponent } from '../../components/buttons/buttons.component';
import { StrategyService } from '../../../backend/services/strategy/strategy.service';
import { GetStrategiesKeyResponse } from '../../../backend/services/strategy/models/GetStrategiesKeyResponse';
import { StrategyModel } from '../../../backend/services/strategy/models/StrategyModel';
import { ClockService } from '../../../backend/services/clock/clock.service';
import { switchMap } from 'rxjs/operators'; // ✅ Importar operador

@Component({
  selector: 'app-strategy-select-page',
  standalone: true,
  imports: [CommonModule, FormsModule, ButtonsComponent],
  templateUrl: './strategy-select-page.component.html',
  styleUrls: ['./strategy-select-page.component.css']
})
export class StrategySelectPageComponent implements OnInit {
    strategies: GetStrategiesKeyResponse[] = [];
    selectedKey: string | null = null;
    isActivating: boolean = false;

    constructor(
        private _strategyService: StrategyService, 
        private _clockService: ClockService
    ) {}

    ngOnInit(): void {
        this._strategyService.getAllKeys().subscribe({
            next: (data) => {
                this.strategies = data;
            },
            error: (err) => {
                console.error('Error loading strategies:', err);
            }
        });
    }

    active(): void {
        if (!this.selectedKey) {
            alert('First, you will select a strategy.');
            return;
        }

        this.isActivating = true;

        this._clockService.get().pipe(
            switchMap(clock => {
                const strategyModel: StrategyModel = {
                    key: this.selectedKey!,
                    date: clock.dateSystem
                };
                
                return this._strategyService.create(strategyModel);
            })
        ).subscribe({
            next: (response) => {
                alert(`Strategy ${this.selectedKey} activated successfully.`);
                console.log('Response:', response);
                this.isActivating = false;
            },
            error: (err) => {
                console.error('Error activating strategy:', err);
                alert(`Error: Could not activate strategy. ${err.message || 'Please try again.'}`);
                this.isActivating = false;
            },
            complete: () => {
                console.log('Strategy activation completed');
            }
        });
    }
}