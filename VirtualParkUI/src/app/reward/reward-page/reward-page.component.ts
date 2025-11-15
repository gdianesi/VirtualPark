import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { AuthRoleService } from '../../auth-role/auth-role.service';

@Component({
  standalone: true,
  selector: 'app-reward-page-container',
  imports: [CommonModule, RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './reward-page.component.html',
  styleUrls: ['./reward-page.component.css']
})
export class RewardPageComponent {
  constructor(private authRole: AuthRoleService) {}

  canViewReward(): boolean {
    return this.authRole.hasAnyRole(['Administrator']);
  }
}
