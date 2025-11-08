import { Component } from '@angular/core';
import { AuthRoleService } from '../../auth-role/auth-role.service';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';

@Component({
  standalone: true,
  selector: 'app-event-page',
  imports: [CommonModule, RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './event-page.component.html',
  styleUrls: ['./event-page.component.css']
})
export class EventPageComponent {
  constructor(private authRole: AuthRoleService) {}

  canViewCreate(): boolean {
    // solo el Administrador puede crear eventos
    return this.authRole.hasAnyRole(['Administrator']);
  }
}
