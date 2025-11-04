import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { IncidenceService } from '../../../backend/services/incident/incident.service';
import { IncidenceModel } from '../../../backend/services/incident/models/IncidenceModel';
import { TableComponent, TableColumn } from '../../components/table/generic-table.component';
import { ButtonsComponent } from '../../components/buttons/buttons.component';
import { MessageComponent } from '../../components/messages/message.component';
import { MessageService } from '../../components/messages/service/message.service';

@Component({
  selector: 'app-incidence-list-page',
  standalone: true,
  imports: [CommonModule, TableComponent, ButtonsComponent, MessageComponent],
  templateUrl: './incidence-list-page.component.html',
  styleUrls: ['./incidence-list-page.component.css']
})
export class IncidencePageListComponent implements OnInit {
  incidences: IncidenceModel[] = [];
  loading = false;

  columns: TableColumn[] = [
    { key: 'typeId', label: 'Type', align: 'center' },
    { key: 'description', label: 'Description', align: 'left' },
    { key: 'start', label: 'Start', align: 'center' },
    { key: 'end', label: 'End', align: 'center' },
    { key: 'active', label: 'Active', align: 'center' }
  ];

  constructor(
    private readonly incidenceService: IncidenceService,
    private readonly messageService: MessageService,
    private readonly router: Router
  ) {}

  ngOnInit() {
    this.loadIncidences();
  }

  loadIncidences() {
    this.loading = true;
    this.incidenceService.getAll().subscribe({
      next: res => {
        this.incidences = res;
        this.loading = false;
      },
      error: () => {
        this.messageService.show('Error loading incidences.', "error");
        this.loading = false;
      }
    });
  }

  goToCreate() {
    this.router.navigate(['/incidences/new']);
  }

  goToEdit(id: string) {
    this.router.navigate([`/incidences/edit/${id}`]);
  }

  deleteIncidence(id: string) {
    if (confirm('Are you sure you want to delete this incidence?')) {
      this.incidenceService.remove(id).subscribe({
        next: () => {
          this.messageService.show('Incidence deleted.', "success");
          this.loadIncidences();
        },
        error: () => this.messageService.show('Error deleting incidence.', "error")
      });
    }
  }
}
