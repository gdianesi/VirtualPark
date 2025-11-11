import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReportRoutingModule } from './report-routing.module';
import { ReportPageComponent } from './report-page/report-page.component';
import { AttractionReportComponent } from './attraction-report/attraction-report.component';
import { ButtonsComponent } from '../components/buttons/buttons.component';
import { MessageComponent } from '../components/messages/message.component';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [ReportPageComponent, AttractionReportComponent],
  imports: [
    CommonModule,
    FormsModule,
    ButtonsComponent,
    MessageComponent,
    ReportRoutingModule
  ]
})
export class ReportsModule {}
