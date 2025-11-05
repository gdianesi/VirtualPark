import { Component, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavigationEnd, Router, RouterModule } from '@angular/router';
import { DropdownItem, DropdownMenuComponent } from '../../components/dropdown-menu/dropdown-menu.component';
import { SessionService } from '../../../backend/services/session/session.service';
import { AuthRoleService } from '../../auth-role/auth-role.service';
import { Subscription } from 'rxjs';
import { filter } from 'rxjs/operators';

type RoleGuardedMenuItem = DropdownItem & { roles: string[] };

@Component({
    selector: 'app-header',
    standalone: true,
    imports: [CommonModule, RouterModule, DropdownMenuComponent],
    templateUrl: './header.component.html',
    styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnDestroy {
    private readonly hiddenRoutes = new Set<string>(['', '/', '/user/login', '/user/register']);
    private subscription: Subscription | null = null;
    isVisible = true;
    settingsOpen = false;

    constructor(
        private sessionService: SessionService,
        private router: Router,
        private authRole: AuthRoleService
    ) {
        this.isVisible = !this.shouldHide(this.router.url);

        this.subscription = this.router.events
            .pipe(filter((event): event is NavigationEnd => event instanceof NavigationEnd))
            .subscribe(event => {
                this.isVisible = !this.shouldHide(event.urlAfterRedirects);
                this.closeSettings();
            });
    }

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

    incidenceMenu = [
        { label: 'Incidence', path: '/incidences', roles: ['Operator', 'Administrator'] },
    ];

    typeIncidenceMenu = [
        { label: 'Types', path: '/typeincidences', roles: ['Operator', 'Administrator'] }
    ];

    homeMenu: RoleGuardedMenuItem[] = [
        { label: 'Home', path: '/user/home', roles: ['Administrator', 'Operator', 'Visitor'] }
    ];

    clockMenu: RoleGuardedMenuItem[] = [
        { label: 'Clock', path: '/clock', roles: ['Administrator'] }
    ];

    settingsMenu: RoleGuardedMenuItem[] = [
        { label: 'Profile', path: '/user/profile', roles: ['Administrator', 'Operator', 'Visitor'] }
    ];

    canView(roles: string[]): boolean {
        return this.authRole.hasAnyRole(roles);
    }

    ngOnDestroy(): void {
        this.subscription?.unsubscribe();
    }

    getVisibleMenu(menu: RoleGuardedMenuItem[]): DropdownItem[] {
        return menu
            .filter(item => this.canView(item.roles))
            .map(({ label, path }) => ({ label, path }));
    }

    toggleSettings(): void {
        this.settingsOpen = !this.settingsOpen;
    }

    closeSettings(): void {
        this.settingsOpen = false;
    }

    goProfile(): void {
        this.closeSettings();
        this.router.navigate(['/user/profile']);
    }

    logout(): void {
        const token = localStorage.getItem('token');

        if (token) {
            this.sessionService.logout(token);
        }

        this.closeSettings();
        this.router.navigate(['/user/login']);
    }

    private shouldHide(url: string): boolean {
        if (!url) return true;
        const cleaned = url.split('?')[0].split('#')[0];
        return this.hiddenRoutes.has(cleaned);
    }

}
