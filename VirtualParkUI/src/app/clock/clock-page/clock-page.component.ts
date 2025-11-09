import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { AuthRoleService } from '../../auth-role/auth-role.service';

@Component({
  standalone: true,
  selector: 'app-clock-page',
  imports: [CommonModule, RouterOutlet],
  templateUrl: './clock-page.component.html',
  styleUrls: ['./clock-page.component.css']
})
export class ClockPageComponent {
  constructor(private authRole: AuthRoleService) {}

  canViewClock(): boolean {
    return this.authRole.hasAnyRole(['Administrator']);
  }
}
