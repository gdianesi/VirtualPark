import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import {Router, RouterModule} from '@angular/router';
import { DropdownMenuComponent } from '../../components/dropdown-menu/dropdown-menu.component';
import { SessionService } from '../../../backend/services/session/session.service';
import { ButtonsComponent } from '../../components/buttons/buttons.component';

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
        private router: Router
    ) {}
    
    attractionsMenu = [
        { label: 'List', path: '/attraction' },
        { label: 'Create', path: '/attraction/new' },
        { label: 'Ranking', path: '/ranking' }
    ];

    eventsMenu = [
        { label: 'Event', path: '/events' },
        { label: 'Create', path: '/events/new' }
    ];

    rewardMenu = [
        { label: 'Reward', path: '/reward' },
        { label: 'Create', path: '/rewards/create' },
        
    ]

    logout(): void {
        const token = localStorage.getItem('token');

        if (token) {
            this.sessionService.logout(token);
        }
        
        this.router.navigate(['/user/login']);
    }

}