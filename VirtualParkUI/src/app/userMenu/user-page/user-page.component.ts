import { Component } from '@angular/core';
import { AuthRoleService } from '../../../backend/services/auth/auth-role.service';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';


@Component({
  imports: [CommonModule, RouterOutlet, RouterLink, RouterLinkActive],
  selector: 'app-user-page',
  standalone: true, 
  templateUrl: './user-page.component.html',
  styleUrls: ['./user-page.component.css']
})
export class UserPageComponent {
  constructor(private authRole: AuthRoleService) {}

  canViewCreate(): boolean {
    return this.authRole.hasAnyRole(['Administrator']);
  }
}
