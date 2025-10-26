import { Component } from '@angular/core';
import { DropdownMenuComponent } from '../../../components/dropdown-menu/dropdown-menu.component';

@Component({
    selector: 'app-header',
    standalone: true,     
    imports: [DropdownMenuComponent],
    templateUrl: './header.component.html',
    styleUrls: ['./header.component.css']
})
export class HeaderComponent {}
