import { Component } from '@angular/core';
import { RegisterUserFormComponent } from '../../business-components/register-user-form/register-user-form.component';
import { ButtonsComponent } from '../../components/buttons/buttons.component';

@Component({
  selector: 'app-user-register-page',
  imports: [RegisterUserFormComponent, ButtonsComponent],
  templateUrl: './user-register-page.component.html',
  styleUrl: './user-register-page.component.css'
})
export class UserRegisterPageComponent {

}
