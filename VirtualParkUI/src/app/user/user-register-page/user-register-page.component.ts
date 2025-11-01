import { Component, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RegisterUserFormComponent } from '../../business-components/user/register-user-form/register-user-form.component';
import { ButtonsComponent } from '../../components/buttons/buttons.component';
import { UserService } from '../../../backend/services/user/user.service';
import { CreateUserRequest } from '../../../backend/services/user/models/CreateUserRequest';
import { Router } from "@angular/router";

@Component({
    selector: 'app-user-register-page',
    imports: [CommonModule, RegisterUserFormComponent, ButtonsComponent],
    templateUrl: './user-register-page.component.html',
    styleUrl: './user-register-page.component.css'
})
export class UserRegisterPageComponent {
    @ViewChild(RegisterUserFormComponent) registerForm!: RegisterUserFormComponent;

    isLoading = false;
    errorMessage: string | null = null;

    constructor(
        private userService: UserService,
        private router: Router
    ) {}

    onSubmit(registerUser: CreateUserRequest): void {
        this.isLoading = true;
        this.errorMessage = null;

        this.userService.create(registerUser).subscribe({
            next: (response) => {
                this.isLoading = false;
                this.router.navigate(['/user/home']);
            },
            error: () => {
                this.isLoading = false;
            }
        });
    }

    handleRegister(): void {
        if (this.registerForm) {
            this.registerForm.emitForm();
        }
    }

    handleSignIn(): void {
        this.router.navigate(['/user/login']);
    }
}
