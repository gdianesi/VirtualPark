import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { IncidenceService } from '../../../backend/services/incident/incident.service';
import { CreateIncidenceRequest } from '../../../backend/services/incident/models/CreateIncidenceRequest';
import { ButtonsComponent } from '../../components/buttons/buttons.component';
import { MessageComponent } from '../../components/messages/message.component';
import { MessageService } from '../../components/messages/service/message.service';
import { AuthRoleService } from '../../auth-role/auth-role.service';

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

  isOperator = false;

  constructor(
    private readonly incidenceService: IncidenceService,
    private readonly messageService: MessageService,
    private readonly authRoleService: AuthRoleService,
    private readonly router: Router
  ) {}

  ngOnInit() {
    this.isOperator = this.authRoleService.hasAnyRole(['Operator']);
  }

  save() {
    if (!this.form.typeId || !this.form.description || !this.form.start || !this.form.end || !this.form.attractionId) {
      this.messageService.show('Please fill in all required fields.', 'error');
      return;
    }

    this.incidenceService.create(this.form).subscribe({
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
