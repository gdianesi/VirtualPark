import {Component, NgModule} from '@angular/core';
import { ButtonsComponent } from '../../components/buttons/buttons.component';

@Component({
    selector: 'home-app',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.css'],
    imports: [ButtonsComponent]
})

export class HomeComponent { }