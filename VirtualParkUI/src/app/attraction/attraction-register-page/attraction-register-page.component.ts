import { Component } from '@angular/core';
import { CreateAttractionFormComponent } from '../../business-components/create-attraction-form/create-attraction-form.component';
import { ButtonsComponent } from '../../components/buttons/buttons.component';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-attraction-register-page',
  standalone: true,
  imports: [CreateAttractionFormComponent, ButtonsComponent, RouterLink],
  templateUrl: './attraction-register-page.component.html',
  styleUrls: ['./attraction-register-page.component.css']
})
export class AttractionRegisterPageComponent {}
