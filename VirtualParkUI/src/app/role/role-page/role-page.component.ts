import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { AuthRoleService } from '../../auth-role/auth-role.service';

@Component({
  standalone: true,
  selector: 'app-role-page',
  imports: [CommonModule, RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './role-page.component.html',
  styleUrl: './role-page.component.css'
})

export class RolePageComponent {
  constructor(private authRole: AuthRoleService) {}

  canViewRegister(): boolean {
    return this.authRole.hasAnyRole(['Operator', 'Administrator']);
  }

}
