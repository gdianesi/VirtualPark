import { NgModule } from '@angular/core';
import { ClockRoutingModule } from './clock-routing.module';
import { ClockPageComponent } from './clock-page/clock-page.component';

@NgModule({
  imports: [
    ClockRoutingModule,
    ClockPageComponent
  ]
})
export class ClockModule {}
