import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { AuthRoleService } from '../../auth-role/auth-role.service';

@Component({
  standalone: true,
  selector: 'app-reward-redemption-page',
  imports: [CommonModule, RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './reward-redemption-page.component.html',
  styleUrls: ['./reward-redemption-page.component.css']
})
export class RewardRedemptionPageComponent {
  constructor(private authRole: AuthRoleService) {}

  canViewHistory(): boolean {
    return this.authRole.hasAnyRole(['Visitor']);
  }
}
