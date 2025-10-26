import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

export type DropdownItem = {
    label: string;
    path: string;
};

@Component({
    standalone: true,
    selector: 'app-dropdown-menu',
    templateUrl: './dropdown-menu.component.html',
    styleUrls: ['./dropdown-menu.component.css'],
    imports: [CommonModule],

})
export class DropdownMenuComponent {
    @Input() label = '';
    @Input() items: DropdownItem[] = [];
    @Output() opened = new EventEmitter<void>();
    @Output() closed = new EventEmitter<void>();

    open = false;

    constructor(private router: Router) {}

    toggle() {
        this.open = !this.open;
        this.open ? this.opened.emit() : this.closed.emit();
    }

    close() {
        if (this.open) {
            this.open = false;
            this.closed.emit();
        }
    }

    async go(path: string) {
        await this.router.navigate([path]);
        this.close();
    }

    onKeydown(e: KeyboardEvent) {
        if (e.key === 'Escape') this.close();
    }
}