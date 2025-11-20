import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ReportPageComponent } from './report-page/report-page.component';
import { AttractionReportComponent } from './attraction-report/attraction-report.component';

const routes: Routes = [
  {
    path: '',
    component: ReportPageComponent,
    children: [
      { path: '', redirectTo: 'attractions', pathMatch: 'full' },
      { path: 'attractions', component: AttractionReportComponent }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ReportsRoutingModule {}
