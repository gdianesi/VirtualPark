import { Component } from '@angular/core';
import { AuthRoleService } from '../../../backend/services/auth/auth-role.service';

@Component({
  selector: 'app-report-page-container',
  templateUrl: './report-page.component.html',
  styleUrls: ['./report-page.component.css'],
  standalone: true
})
export class ReportPageComponent {
  constructor(private auth: AuthRoleService) {}

  canView() {
    return this.auth.hasAnyRole(['Administrator']);
  }
}
