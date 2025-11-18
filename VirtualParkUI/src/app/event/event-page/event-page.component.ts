import { Component } from '@angular/core';
import { AuthRoleService } from '../../../backend/services/auth/auth-role.service';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-event-page',
  standalone: true,
  imports: [CommonModule, RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './event-page.component.html',
  styleUrls: ['./event-page.component.css']
})
export class EventPageComponent {
  constructor(private authRole: AuthRoleService) {}

  canViewCreate(): boolean {
    return this.authRole.hasAnyRole(['Administrator']);
  }
}
