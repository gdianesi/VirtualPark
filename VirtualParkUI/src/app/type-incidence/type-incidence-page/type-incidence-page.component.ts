import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { AuthRoleService } from '../../../backend/services/auth/auth-role.service';

@Component({
  standalone: true,
  selector: 'app-type-incidence-page',
  imports: [CommonModule, RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './type-incidence-page.component.html',
  styleUrls: ['./type-incidence-page.component.css']
})
export class TypeIncidencePageComponent {
  constructor(private authRole: AuthRoleService) {}

  canViewCreate(): boolean {
    return this.authRole.hasAnyRole(['Administrator', 'Operator']);
  }
}
