import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';

export type DropdownItem = { label: string; path: string };

@Component({
    selector: 'app-dropdown-menu',
    standalone: true,
    imports: [CommonModule, RouterModule],
    templateUrl: './dropdown-menu.component.html',
    styleUrls: ['./dropdown-menu.component.css']
})
export class DropdownMenuComponent {
    @Input() label = '';
    @Input() items: DropdownItem[] = [];

    open = false;

    constructor(private router: Router) {}

    toggle() { this.open = !this.open; }
    close()  { this.open = false; }

    go(path: string) {
        this.router.navigate([path]);
        this.close();
    }

    onKeydown(event: KeyboardEvent) {
        if (event.key === 'Escape') this.close();
    }
}