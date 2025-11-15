import { NgModule } from '@angular/core';
import { RouterModule,Routes } from '@angular/router';
import { StrategySelectPageComponent } from './strategy-select-page/strategy-select-page.component';

const routes: Routes = [
    { 
      path: '', component: StrategySelectPageComponent 
    }
  ]

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class StrategyRoutingModule { }