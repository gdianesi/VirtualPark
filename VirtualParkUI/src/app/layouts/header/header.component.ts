import { Component, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavigationEnd, Router, RouterModule } from '@angular/router';
import { DropdownItem, DropdownMenuComponent } from '../../components/dropdown-menu/dropdown-menu.component';
import { SessionService } from '../../../backend/services/session/session.service';
import { ButtonsComponent } from '../../components/buttons/buttons.component';
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

    attractionsMenu: RoleGuardedMenuItem[] = [];

    ticketsMenu: RoleGuardedMenuItem[] = [];

    eventsMenu: RoleGuardedMenuItem[] = [];

    rewardRedemptionMenu: RoleGuardedMenuItem[] = [];


    rewardMenu: RoleGuardedMenuItem[] = [
        { label: 'Reward', path: '/reward', roles: ['Administrator'] },
        { label: 'Create', path: '/rewards/create', roles: ['Administrator'] }
    ];

    incidenceMenu = [];

    reportMenu = [];

    typeIncidenceMenu = [];

    clockMenu: RoleGuardedMenuItem[] = [
        { label: 'Clock', path: '/clock', roles: ['Administrator'] }
    ];

    strategyMenu: RoleGuardedMenuItem[] = [
        { label: 'Strategies', path: '/strategy', roles: ['Administrator'] }
    ];
    
    rankingMenu: RoleGuardedMenuItem[] = [];

    userMenu: RoleGuardedMenuItem[] = [
        { label: 'List', path: '/user-home/list', roles: ['Administrator'] },
        { label: 'Create', path: '/user-home/create', roles: ['Administrator'] },
    ];
    
    roleMenu: RoleGuardedMenuItem[] = [
        { label: 'List', path: './role/list', roles: ['Administrator', 'Operator'] },
        { label: 'Create', path: '/role/create', roles: ['Administrator'] },
        { label: 'Edit Permission', path: '/role/permission', roles: ['Administrator'] }
    ]
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
            this.sessionService.logout(token).subscribe({
                next: () => {
                    this.closeSettings();
                    this.router.navigate(['/user/login']);
                },
                error: () => {
                    this.closeSettings();
                    this.router.navigate(['/user/login']);
                }
            });
            return;
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
