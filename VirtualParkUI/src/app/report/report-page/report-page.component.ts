import { Component } from '@angular/core';
import { AuthRoleService } from '../../../backend/services/auth/auth-role.service';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-report-page-container',
  templateUrl: './report-page.component.html',
  styleUrls: ['./report-page.component.css'],
  standalone: true,
  imports: [CommonModule, RouterLink, RouterLinkActive, RouterOutlet]
})
export class ReportPageComponent {
  constructor(private auth: AuthRoleService) {}

  canView() {
    return this.auth.hasAnyRole(['Administrator']);
  }
}
