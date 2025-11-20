import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { IncidenceService } from '../../../backend/services/incident/incident.service';
import { IncidenceModel } from '../../../backend/services/incident/models/IncidenceModel';
import { TableComponent, TableColumn } from '../../components/table/generic-table.component';
import { ButtonsComponent } from '../../components/buttons/buttons.component';
import { MessageComponent } from '../../components/messages/message.component';
import { MessageService } from '../../../backend/services/message/message.service';
import { TypeIncidenceService } from '../../../backend/services/type-incidence/type-incidence.service';
import { TypeIncidenceModel } from '../../../backend/services/type-incidence/models/TypeIncidenceModel';
import { ConfirmDialogComponent } from "../../components/confirm-dialog/confirm-dialog.component";
import { ClockService } from '../../../backend/services/clock/clock.service';
import { ClockModel } from '../../../backend/services/clock/models/ClockModel';


@Component({
  selector: 'app-incidence-list-page',
  standalone: true,
  imports: [CommonModule, ButtonsComponent, MessageComponent, ConfirmDialogComponent],
  templateUrl: './incidence-list-page.component.html',
  styleUrls: ['./incidence-list-page.component.css']
})
export class IncidencePageListComponent implements OnInit {
  incidences: IncidenceModel[] = [];
  loading = false;
  showDialog = false;
  pendingDeleteId: string | null = null;

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
  systemNow: Date = new Date();

  constructor(
    private readonly incidenceService: IncidenceService,
    private readonly messageService: MessageService,
    private readonly router: Router,
    private readonly typeService: TypeIncidenceService,
    private readonly clockService: ClockService
  ) {}

  ngOnInit() {
    this.loadClock();
  }

  loadClock() {
    this.clockService.get().subscribe({
      next: (clock: ClockModel) => {
        this.systemNow = new Date(clock.dateSystem);
        this.loadTypes();
      },
      error: () => {
        console.warn("Clock API failed, falling back to local date");
        this.systemNow = new Date();
        this.loadTypes();
      }
    });
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

askDelete(id: string) {
  this.pendingDeleteId = id;
  this.showDialog = true;
}

  onConfirmDelete(result: boolean) {
    this.showDialog = false;

    if (!result || !this.pendingDeleteId) {
      this.pendingDeleteId = null;
      return;
    }

    this.incidenceService.remove(this.pendingDeleteId).subscribe({
      next: () => {
        this.messageService.show('Incidence deleted.', 'success');
        this.loadIncidences();
      },
      error: () => this.messageService.show('Error deleting incidence.', 'error')
    });

    this.pendingDeleteId = null;
  }


  toggleActive(incidence: any) {
    const updatedStatus = incidence.active === 'True' ? 'False' : 'True';

    const formatDate = (dateStr: string) => {
      const d = this.parseIncDate(dateStr);
      return d.toISOString().split('.')[0];
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

  goToMaintenance() {
    this.router.navigate(['/incidences/new'], {
      state: { maintenance: true }
    });
  }

  parseDate(d: string): Date {
  const parts = d.split(/[\s/:]/); 

  return new Date(
    Number(parts[2]),
    Number(parts[1]) - 1,
    Number(parts[0]),
    Number(parts[3] || 0),
    Number(parts[4] || 0),
    Number(parts[5] || 0)
  );
}

  isExpired(inc: IncidenceModel): boolean {
    const end = this.parseDate(inc.end);
    return end < this.systemNow;
  }

  parseIncDate(dateStr: string): Date {
  const [datePart, timePart] = dateStr.split(" ");
  const [day, month, year] = datePart.split("/").map(Number);
  const [hour, minute, second] = timePart.split(":").map(Number);

  return new Date(year, month - 1, day, hour, minute, second);
  }


}
