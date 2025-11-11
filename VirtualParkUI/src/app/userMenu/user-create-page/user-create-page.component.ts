import { Component, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { ButtonsComponent } from '../../components/buttons/buttons.component';
import { CreateUserFormComponent } from '../../business-components/user/create-user-form/create-user-form.component';
import { UserService } from '../../../backend/services/user/user.service';
import { CreateUserRequest } from '../../../backend/services/user/models/CreateUserRequest';

@Component({
    selector: 'app-user-create-page',
    standalone: true,
    imports: [CommonModule, ButtonsComponent, CreateUserFormComponent],
    templateUrl: './user-create-page.component.html',
    styleUrls: ['./user-create-page.component.css']
})
export class UserCreatePageComponent {
    @ViewChild(CreateUserFormComponent) createForm!: CreateUserFormComponent;

    isSaving = false;
    errorMessage: string | null = null;

    constructor(
        private userService: UserService,
        private router: Router
    ) {}

    onSubmit(payload: CreateUserRequest): void {
        this.isSaving = true;
        this.errorMessage = null;
        this.userService.create(payload).subscribe({
            next: () => {
                this.isSaving = false;
                this.router.navigate(['/user-home/list']);
            },
            error: () => {
                this.isSaving = false;
                this.errorMessage = 'The user could not be created. Please try again.';
            }
        });
    }

    handleSave(): void {
        this.createForm?.emitForm();
    }

    handleCancel(): void {
        this.router.navigate(['/user-home/list']);
    }
}
