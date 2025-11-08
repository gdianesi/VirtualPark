import { Component } from '@angular/core';
import { AuthRoleService } from '../../auth-role/auth-role.service';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';


@Component({
  imports: [CommonModule, RouterOutlet, RouterLink, RouterLinkActive],
  selector: 'app-attraction-page',
  templateUrl: './attraction-page.component.html',
  styleUrls: ['./attraction-page.component.css']
})
export class AttractionPageComponent {
  constructor(private authRole: AuthRoleService) {}

  canViewCreate(): boolean {
    return this.authRole.hasAnyRole(['Administrator', 'Operator']);
  }
}
