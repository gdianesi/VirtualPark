import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import {Router, RouterModule} from '@angular/router';
import { DropdownMenuComponent } from '../../components/dropdown-menu/dropdown-menu.component';
import { SessionService } from '../../../backend/services/session/session.service';
import { ButtonsComponent } from '../../components/buttons/buttons.component';
import { AuthRoleService } from '../../auth-role/auth-role.service';

@Component({
    selector: 'app-header',
    standalone: true,
    imports: [CommonModule, RouterModule, DropdownMenuComponent,  ButtonsComponent],
    templateUrl: './header.component.html',
    styleUrls: ['./header.component.css']
})
export class HeaderComponent {
    constructor(
        private sessionService: SessionService,
        private router: Router,
        private authRole: AuthRoleService
    ) {}
    
  attractionsMenu = [
    { label: 'List', path: '/attraction', roles: ['Administrator', 'Operator', 'Visitor'] },
    { label: 'Create', path: '/attraction/register', roles: ['Administrator', 'Operator'] },
    { label: 'Ranking', path: '/ranking', roles: ['Visitor', 'Administrator'] }
  ];

  eventsMenu = [
    { label: 'Event', path: '/events', roles: ['Administrator'] },
    { label: 'Create', path: '/events/new', roles: ['Administrator'] }
  ];

  rewardMenu = [
    { label: 'Reward', path: '/reward', roles: ['Administrator'] },
    { label: 'Create', path: '/rewards/create', roles: ['Administrator'] }
  ];

    canView(roles: string[]): boolean {
    return this.authRole.hasAnyRole(roles);
  }

    logout(): void {
        const token = localStorage.getItem('token');

        if (token) {
            this.sessionService.logout(token);
        }
        
        this.router.navigate(['/user/login']);
    }

}