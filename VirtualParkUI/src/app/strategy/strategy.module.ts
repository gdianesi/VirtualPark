import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StrategySelectPageComponent } from './strategy-select-page/strategy-select-page.component';
import { StrategyRoutingModule } from './strategy-routing.module';
import { ButtonsComponent } from '../components/buttons/buttons.component';
import { StrategyPageComponent } from './strategy-page/strategy-page.component';

@NgModule({
  declarations: [],
  imports: [CommonModule, StrategyRoutingModule, StrategySelectPageComponent, ButtonsComponent, StrategyPageComponent]
})

export class StrategyModule { }