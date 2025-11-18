import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { AuthRoleService } from '../../../backend/services/auth/auth-role.service';

@Component({
  standalone: true,
  selector: 'app-ranking-page-container',
  imports: [CommonModule, RouterOutlet],
  templateUrl: './ranking-page.component.html',
  styleUrls: ['./ranking-page.component.css']
})
export class RankingPageComponent {
  constructor(private authRole: AuthRoleService) {}

  canViewRanking(): boolean {
    return this.authRole.hasAnyRole(['Administrator', 'Visitor']);
  }
}
