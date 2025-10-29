import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { ButtonsComponent } from '../../components/buttons/buttons.component';

@Component({
    selector: 'home-app',
    standalone: true,
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.css'],
    imports: [ButtonsComponent, RouterModule] 
})
export class HomeComponent {
    constructor(private router: Router) {}

    goLogin() {
        this.router.navigate(['/user/login']);
    }

    goRegister() {
        this.router.navigate(['/user/register']);
    }
}
