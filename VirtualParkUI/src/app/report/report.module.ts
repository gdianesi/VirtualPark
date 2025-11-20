import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReportsRoutingModule } from './report-routing.module';
import { AttractionReportComponent } from './attraction-report/attraction-report.component';
import { ButtonsComponent } from '../components/buttons/buttons.component';
import { MessageComponent } from '../components/messages/message.component';
import { ReportPageComponent } from './report-page/report-page.component';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    FormsModule,
    ButtonsComponent,
    MessageComponent,
    ReportsRoutingModule,
    AttractionReportComponent,
    ReportPageComponent
  ]
})
export class ReportsModule {}
