import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { AuthRoleService } from '../../../backend/services/auth/auth-role.service';

@Component({
  standalone: true,
  selector: 'app-ticket-page',
  imports: [CommonModule, RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './ticket-page.component.html',
  styleUrls: ['./ticket-page.component.css']
})
export class TicketPageComponent {
  constructor(private authRole: AuthRoleService) {}

  canViewRegister(): boolean {
    return this.authRole.hasAnyRole(['Visitor']);
  }
}
