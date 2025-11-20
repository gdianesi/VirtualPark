import { Component } from '@angular/core';
import { AuthRoleService } from '../../../backend/services/auth/auth-role.service';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-strategy-page',
  imports: [CommonModule, RouterOutlet, RouterLink, RouterLinkActive],
  standalone: true,
  templateUrl: './strategy-page.component.html',
  styleUrl: './strategy-page.component.css'
})
export class StrategyPageComponent {
    constructor(private authRole: AuthRoleService) {}

  canViewCreate() : boolean {
        return this.authRole.hasAnyRole(['Administrator']);
  }
}
