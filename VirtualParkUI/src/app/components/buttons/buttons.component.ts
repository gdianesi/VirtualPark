import { Component, Input } from '@angular/core';

@Component({
    selector: 'app-buttons',
    standalone: false,
    templateUrl: './buttons.component.html',
    styleUrls: ['./buttons.component.css']
})
export class ButtonsComponent {
    @Input() text: string = 'Button';

    @Input() color: 'primary' | 'secondary' | 'danger' = 'primary';
}
