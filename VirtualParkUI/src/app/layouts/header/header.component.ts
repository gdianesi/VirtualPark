import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import {Router, RouterModule} from '@angular/router';
import { DropdownItem, DropdownMenuComponent } from '../../components/dropdown-menu/dropdown-menu.component';
import { SessionService } from '../../../backend/services/session/session.service';
import { ButtonsComponent } from '../../components/buttons/buttons.component';
import { AuthRoleService } from '../../auth-role/auth-role.service';

type RoleGuardedMenuItem = DropdownItem & { roles: string[] };

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
    
    attractionsMenu: RoleGuardedMenuItem[] = [
        { label: 'List', path: '/attraction', roles: ['Administrator', 'Operator', 'Visitor'] },
        { label: 'Create', path: '/attraction/register', roles: ['Administrator', 'Operator'] },
        { label: 'Ranking', path: '/ranking', roles: ['Visitor', 'Administrator'] }
    ];

    ticketsMenu: RoleGuardedMenuItem[] = [
        { label: 'List', path: '/ticket', roles: ['Administrator'] },
        { label: 'Create', path: '/ticket/register', roles: ['Visitor'] }
    ];

    eventsMenu: RoleGuardedMenuItem[] = [
        { label: 'Event', path: '/events', roles: ['Administrator'] },
        { label: 'Create', path: '/events/new', roles: ['Administrator'] }
    ];

    rewardMenu: RoleGuardedMenuItem[] = [
        { label: 'Reward', path: '/reward', roles: ['Administrator'] },
        { label: 'Create', path: '/rewards/create', roles: ['Administrator'] }
    ];

    canView(roles: string[]): boolean {
        return this.authRole.hasAnyRole(roles);
    }

    getVisibleMenu(menu: RoleGuardedMenuItem[]): DropdownItem[] {
        return menu
            .filter(item => this.canView(item.roles))
            .map(({ label, path }) => ({ label, path }));
    }

    logout(): void {
        const token = localStorage.getItem('token');

        if (token) {
            this.sessionService.logout(token);
        }
        
        this.router.navigate(['/user/login']);
    }

}
