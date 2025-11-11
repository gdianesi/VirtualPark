import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StrategySelectPageComponent } from './strategy-select-page/strategy-select-page.component';
import { StrategyRoutingModule } from './strategy-routing';
import { ButtonsComponent } from '../components/buttons/buttons.component';
import { StrategyPageComponent } from './strategy-page/strategy-page.component';

@NgModule({
  declarations: [
    StrategyPageComponent
  ],
  imports: [CommonModule, StrategyRoutingModule, StrategySelectPageComponent, ButtonsComponent]
})

export class StrategyModule { }