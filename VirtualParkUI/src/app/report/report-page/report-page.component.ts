import { Component } from '@angular/core';
import { AuthRoleService } from '../../auth-role/auth-role.service';

@Component({
  selector: 'app-report-page-container',
  templateUrl: './report-page.component.html',
  styleUrls: ['./report-page.component.css'],
  standalone: false
})
export class ReportPageComponent {
  constructor(private auth: AuthRoleService) {}

  canView() {
    return this.auth.hasAnyRole(['Administrator']);
  }
}
