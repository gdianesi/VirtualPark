import { Component, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { SessionService } from '../../../backend/services/session/session.service';
import { LoginRequest } from '../../../backend/services/session/models/LoginRequest';
import { UserLoginFormComponent } from '../../business-components/user-login-form/user-login-form.component';
import { ButtonsComponent } from '../../components/buttons/buttons.component';

@Component({
    selector: 'app-user-login-page',
    standalone: true,
    imports: [CommonModule, UserLoginFormComponent, ButtonsComponent],
    templateUrl: './user-login-page.component.html',
    styleUrls: ['./user-login-page.component.css'],
})
export class UserLoginPageComponent {
    @ViewChild(UserLoginFormComponent) loginForm!: UserLoginFormComponent;

    isLoading = false;
    errorMessage = '';

    constructor(
        private readonly sessionService: SessionService,
        private readonly router: Router
    ) {}

    onSubmit(credentials: LoginRequest) {
        this.isLoading = true;
        this.errorMessage = '';

        this.sessionService.login(credentials).subscribe({
            next: (res) => {
                const token = res.token
                this.sessionService.getSession(token).subscribe({
                next: () => {
                    this.isLoading = false;
                    this.router.navigate(['/home']);
                },
                error: (err) => {
                    this.isLoading = false;
                    this.errorMessage = 'Error obtaining session: ' + err.message;
                },
                });
            },
            error: (err) => {
                this.isLoading = false;
                this.errorMessage = err.message || 'Invalid email or password.';
            },
        });
    }

    handleLogin() {
        if (this.loginForm) {
            this.loginForm.emitForm();
        }
    }

    handleRegister() {
        this.router.navigate(['/user/register']);
    }
}