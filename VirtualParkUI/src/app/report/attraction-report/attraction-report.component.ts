import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ButtonsComponent } from '../../components/buttons/buttons.component';
import { MessageComponent } from '../../components/messages/message.component';
import { MessageService } from '../../components/messages/service/message.service';
import { AttractionService } from '../../../backend/services/attraction/attraction.service';
import { ReportAttractionResponse } from '../../../backend/services/attraction/models/ReportAttractionResponse';

@Component({
  selector: 'app-attraction-report',
  standalone: true,
  imports: [CommonModule, FormsModule, ButtonsComponent, MessageComponent],
  templateUrl: './attraction-report.component.html',
  styleUrls: ['./attraction-report.component.css']
})
export class AttractionReportComponent {
  from = '';
  to = '';
  loading = false;
  data: ReportAttractionResponse[] = [];

  constructor(
    private attractionSvc: AttractionService,
    private messageSvc: MessageService
  ) {}

  generate() {
    this.data = [];

    if (!this.from || !this.to) {
      this.messageSvc.show('Both dates are required.', 'error');
      return;
    }

    if (this.from > this.to) {
      this.messageSvc.show('"From" must be before "To".', 'error');
      return;
    }

    this.loading = true;

    this.attractionSvc.getReport(this.from, this.to).subscribe({
      next: (res) => {
        this.data = res;
        this.loading = false;

        if (res.length === 0) {
          this.messageSvc.show('No results found for the selected range.', 'info');
        } else {
          this.messageSvc.show('Report generated successfully.', 'success');
        }
      },
      error: (err) => {
        const msg = err?.error?.message || err?.message || 'Unexpected error.';
        this.messageSvc.show(`Failed to load report: ${msg}`, 'error');
        this.loading = false;
      }
    });
  }
}
