import { Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ButtonsComponent } from '../../components/buttons/buttons.component';
import { CreateAttractionFormComponent } from '../../business-components/attraction/create-attraction-form/create-attraction-form.component';
import { AttractionService } from '../../../backend/services/attraction/attraction.service';
import { CreateAttractionRequest } from '../../../backend/services/attraction/models/CreateAttractionRequest';


@Component({
  selector: 'app-attraction-register-page',
  standalone: true,
  imports: [RouterLink, ButtonsComponent, CreateAttractionFormComponent],
  templateUrl: './attraction-register-page.component.html',
  styleUrls: ['./attraction-register-page.component.css']
})
export class AttractionRegisterPageComponent {
  private attractionService = inject(AttractionService);

  onSubmit(payload: CreateAttractionRequest) {
    this.attractionService.create(payload).subscribe({
      next: () => {
        alert(`Attraction created`);
      },
      error: (e) => alert('Error creating: ' + e.message)
    });
  }
}
