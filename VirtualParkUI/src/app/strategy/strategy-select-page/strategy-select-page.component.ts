import {Component, OnInit} from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ButtonsComponent } from '../../components/buttons/buttons.component';
import { StrategyService } from '../../../backend/services/strategy/strategy.service';
import { GetStrategiesKeyResponse } from '../../../backend/services/strategy/models/GetStrategiesKeyResponse';
import { StrategyModel } from '../../../backend/services/strategy/models/StrategyModel';
import { Clock}

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

    constructor(private _strategyService: StrategyService) {}

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

    aactive(): void {
    if (!this.selectedKey) {
        alert('First, you will select a strategy.');
        return;
    }

    const strategyModel: StrategyModel = {
        key: this.selectedKey,
        date: 
    };

    this._strategyService.create(strategyModel).subscribe({
        next: (response) => {
            alert(`Strategy ${this.selectedKey} activated successfully.`);
            console.log('Response:', response);
        },
        error: (err) => {
            console.error('Error activating strategy:', err);
            alert(`Error: Could not activate strategy. ${err.message || 'Please try again.'}`);
        },
        complete: () => {
            console.log('Strategy activation completed');
        }
    });
}
}