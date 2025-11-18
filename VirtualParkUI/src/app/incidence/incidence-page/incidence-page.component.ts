import { Component } from '@angular/core';
import { AuthRoleService } from '../../../backend/services/auth/auth-role.service';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';

@Component({
  standalone: true,
  selector: 'app-incidence-page',
  imports: [CommonModule, RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './incidence-page.component.html',
  styleUrls: ['./incidence-page.component.css']
})
export class IncidencePageComponent {
  constructor(private authRole: AuthRoleService) {}

  canViewCreate(): boolean {
    return this.authRole.hasAnyRole(['Operator', 'Administrator']);
  }
}
