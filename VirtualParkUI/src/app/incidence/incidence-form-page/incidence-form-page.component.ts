import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { IncidenceService } from '../../../backend/services/incident/incident.service';
import { CreateIncidenceRequest } from '../../../backend/services/incident/models/CreateIncidenceRequest';
import { ButtonsComponent } from '../../components/buttons/buttons.component';
import { MessageComponent } from '../../components/messages/message.component';
import { MessageService } from '../../../backend/services/message/message.service';
import { AuthRoleService } from '../../../backend/services/auth/auth-role.service';
import { TypeIncidenceService } from '../../../backend/services/type-incidence/type-incidence.service';
import { TypeIncidenceModel } from '../../../backend/services/type-incidence/models/TypeIncidenceModel';
import { AttractionService } from '../../../backend/services/attraction/attraction.service'; 
import { AttractionModel } from '../../../backend/services/attraction/models/AttractionModel';

@Component({
  selector: 'app-incidence-form',
  standalone: true,
  imports: [CommonModule, FormsModule, ButtonsComponent, MessageComponent],
  templateUrl: './incidence-form-page.component.html',
  styleUrls: ['./incidence-form-page.component.css']
})
export class IncidenceFormComponent implements OnInit {
  form: CreateIncidenceRequest = {
    typeId: '',
    description: '',
    start: '',
    end: '',
    attractionId: '',
    active: 'true'
  };
 
  isMaintenanceMode = false;
  typeList: TypeIncidenceModel[] = [];
  attractionList: AttractionModel[] = [];

  isOperator = false;

  constructor(
    private readonly incidenceService: IncidenceService,
    private readonly messageService: MessageService,
    private readonly authRoleService: AuthRoleService,
    private readonly router: Router,
    private readonly typeService: TypeIncidenceService, 
    private readonly attractionService: AttractionService
  ) {}

  ngOnInit() {
    this.isOperator = this.authRoleService.hasAnyRole(['Operator']);
    this.loadTypes();
    this.loadAttractions();
    const navState = history.state;
    this.isMaintenanceMode = !!navState?.maintenance;
  }

  prepareMaintenanceForm() {
    const preventive = this.typeList.find(t => t.type === 'PREVENTIVE_MAINTENANCE');

    this.form = {
      typeId: preventive?.id ?? '',
      description: 'Preventive maintenance',
      start: '',
      end: '',
      attractionId: '',
      active: 'true'
    };
  }
  
  loadTypes() {
    this.typeService.getAll().subscribe({
      next: (types) => {
        this.typeList = types.map(t => ({
          ...t,
          typeLabel: t.type === "PREVENTIVE_MAINTENANCE"
            ? "Preventive Maintenance"
            : t.type
        }));

        const navState = history.state;
        if (navState?.maintenance) {
          this.prepareMaintenanceForm();
        }
      },
      error: () => this.messageService.show('Error loading types.', 'error')
    });
  }


    loadAttractions() {
    this.attractionService.getAll().subscribe({
      next: (res) => (this.attractionList = res),
      error: () => this.messageService.show('Error loading attractions.', 'error')
    });
  }

  save() {
    if (!this.form.typeId || !this.form.description || !this.form.start || !this.form.end || !this.form.attractionId) {
      this.messageService.show('Please fill in all required fields.', 'error');
      return;
    }

    const start = new Date(this.form.start);
    const end = new Date(this.form.end);
    const now = new Date();

    if (start > end) {
      this.messageService.show('Start date cannot be after end date.', 'error');
      return;
    }

    const preventive = this.typeList.find(t => t.type === 'PREVENTIVE_MAINTENANCE');

    if (this.form.typeId === preventive?.id) {
      if (start <= now) {
        this.messageService.show('Maintenance must be scheduled for a future date.', 'error');
        return;
      }
    }

    const formatDate = (date: string) => {
      return date.length === 16 ? `${date}:00` : date;
    };

    const formattedForm = {
      ...this.form,
      start: formatDate(this.form.start),
      end: formatDate(this.form.end)
    };

    this.incidenceService.create(formattedForm).subscribe({
      next: () => {
        this.messageService.show('Incidence created successfully.', 'success');
        this.router.navigate(['/incidences']);
      },
      error: () => this.messageService.show('Error creating incidence.', 'error')
    });
  }



  cancel() {
    this.router.navigate(['/incidences']);
  }
}
