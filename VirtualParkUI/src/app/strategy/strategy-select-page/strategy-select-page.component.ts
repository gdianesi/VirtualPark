import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ButtonsComponent } from '../../components/buttons/buttons.component';

@Component({
  selector: 'app-strategy-select-page',
  standalone: true,
  imports: [CommonModule, FormsModule, ButtonsComponent],
  templateUrl: './strategy-select-page.component.html',
  styleUrls: ['./strategy-select-page.component.css']
})
export class StrategySelectPageComponent {
  strategies = [
    { key: 'Attraction' },
    { key: 'Ticket' },
    { key: 'Visitor' }
  ];

  selectedKey: string | null = null;

  active() {
    if (!this.selectedKey) {
      alert('Primero seleccioná una estrategia');
      return;
    }
    console.log('Activando estrategia:', this.selectedKey);
  }
}
