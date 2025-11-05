import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { IncidenceService } from '../../../backend/services/incident/incident.service';
import { IncidenceModel } from '../../../backend/services/incident/models/IncidenceModel';
import { TableComponent, TableColumn } from '../../components/table/generic-table.component';
import { ButtonsComponent } from '../../components/buttons/buttons.component';
import { MessageComponent } from '../../components/messages/message.component';
import { MessageService } from '../../components/messages/service/message.service';
import { TypeIncidenceService } from '../../../backend/services/type-incidence/type-incidence.service';
import { TypeIncidenceModel } from '../../../backend/services/type-incidence/models/TypeIncidenceModel';


@Component({
  selector: 'app-incidence-list-page',
  standalone: true,
  imports: [CommonModule, ButtonsComponent, MessageComponent],
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
    { key: 'active', label: 'Active', align: 'center' },
    { key: 'actions', label: 'Actions', align: 'center' }
  ];

  typeList: TypeIncidenceModel[] = [];
  typeMap: Record<string, string> = {};

  constructor(
    private readonly incidenceService: IncidenceService,
    private readonly messageService: MessageService,
    private readonly router: Router,
    private readonly typeService: TypeIncidenceService,
  ) {}

  ngOnInit() {
    this.loadTypes();
  }

  loadTypes() {
  this.typeService.getAll().subscribe({
    next: (types) => {
      this.typeList = types;
      this.typeMap = Object.fromEntries(types.map(t => [t.id, t.type]));
      this.loadIncidences();
    },
    error: () => this.messageService.show('Error loading types.', 'error')
  });
}
  loadIncidences() {
    this.loading = true;
    this.incidenceService.getAll().subscribe({
    next: res => {
      this.incidences = res.map(i => ({
        ...i,
          typeName: this.typeMap[i.typeId] || '—' 
      }));
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

  toggleActive(incidence: any) {
    const updatedStatus = incidence.active === 'True' ? 'False' : 'True';

    const formatDate = (dateStr: string) => {
      const date = new Date(dateStr);
      return date.toISOString().split('.')[0];
    };

    const updated = {
      ...incidence,
      start: formatDate(incidence.start),
      end: formatDate(incidence.end),
      active: updatedStatus
    };

    this.incidenceService.update(incidence.id, updated).subscribe({
      next: () => {
        this.messageService.show(
          `Incidence ${updatedStatus === 'True' ? 'activated' : 'deactivated'} successfully.`,
          'success'
        );
        this.loadIncidences();
      },
      error: () =>
        this.messageService.show('Error updating incidence.', 'error')
    });
  }
}
