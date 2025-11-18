import { Component, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { ButtonsComponent } from '../../components/buttons/buttons.component';
import { CreateAttractionFormComponent } from '../../business-components/attraction/create-attraction-form/create-attraction-form.component';
import { AttractionService } from '../../../backend/services/attraction/attraction.service';
import { CreateAttractionRequest } from '../../../backend/services/attraction/models/CreateAttractionRequest';
import { MessageService } from '../../../backend/services/message/message.service';
import { MessageComponent } from "../../components/messages/message.component";


@Component({
  selector: 'app-attraction-register-page',
  standalone: true,
  imports: [RouterLink, ButtonsComponent, CreateAttractionFormComponent, MessageComponent],
  templateUrl: './attraction-register-page.component.html',
  styleUrls: ['./attraction-register-page.component.css']
})
export class AttractionRegisterPageComponent {
    private attractionService = inject(AttractionService);
    private messageService = inject(MessageService);
    private router = inject(Router);


  onSubmit(payload: CreateAttractionRequest) {
    this.attractionService.create(payload).subscribe({
      next: () => {
        this.messageService.show('Attraction created successfully!', 'success');
        this.router.navigate(['/attraction']);
      },
      error: (e) => alert('Error creating: ' + e.message)
    });
  }
}
