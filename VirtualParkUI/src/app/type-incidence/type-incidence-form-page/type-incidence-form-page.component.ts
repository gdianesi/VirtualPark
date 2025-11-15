import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { TypeIncidenceService } from '../../../backend/services/type-incidence/type-incidence.service';
import { CreateTypeIncidenceRequest } from '../../../backend/services/type-incidence/models/CreateTypeIncidenceRequest';
import { ButtonsComponent } from '../../components/buttons/buttons.component';
import { MessageComponent } from '../../components/messages/message.component';
import { MessageService } from '../../components/messages/service/message.service';

@Component({
  selector: 'app-type-incidence-form',
  standalone: true,
  imports: [CommonModule, FormsModule, ButtonsComponent, MessageComponent],
  templateUrl: './type-incidence-form-page.component.html',
  styleUrls: ['./type-incidence-form-page.component.css']
})
export class TypeIncidenceFormComponent {
  form: CreateTypeIncidenceRequest = { Type: '' };

  constructor(
    private readonly service: TypeIncidenceService,
    private readonly messageService: MessageService,
    private readonly router: Router
  ) {}

  save() {
    if (!this.form.Type.trim()) {
      this.messageService.show('Type name is required.', 'error');
      return;
    }

    this.service.create(this.form).subscribe({
      next: () => {
        this.messageService.show('Type created successfully.', 'success');
        this.router.navigate(['/typeincidences']);
      },
      error: () => this.messageService.show('Error creating type.', 'error')
    });
  }

  cancel() {
    this.router.navigate(['/typeincidences']);
  }
}

