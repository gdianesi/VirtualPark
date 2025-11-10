import {Component, OnInit} from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ButtonsComponent } from '../../components/buttons/buttons.component';
import { StrategyService } from '../../../backend/services/strategy/strategy.service';
import { GetStrategiesKeyResponse } from '../../../backend/services/strategy/models/GetStrategiesKeyResponse';

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

    active() {
        if (!this.selectedKey) {
            alert('First, you will select a strategy.');
            return;
        }
        console.log('Activating strategy', this.selectedKey);
    }
}