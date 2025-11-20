import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
    selector: 'app-buttons',
    standalone: true,
    imports: [CommonModule],
    templateUrl: './buttons.component.html',
    styleUrls: ['./buttons.component.css']
})
export class ButtonsComponent {
    @Input() text: string = 'Button';
    @Input() color: 'primary' | 'secondary' | 'danger' = 'primary';
    @Input() disabled: boolean = false;
    @Input() type: 'button' | 'submit' = 'button';

    @Output() clicked = new EventEmitter<void>(); 

    onClick() {
        if (!this.disabled) {
            this.clicked.emit();
        }
    }
}