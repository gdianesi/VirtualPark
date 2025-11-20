import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators, AbstractControl } from '@angular/forms';
import { CreateUserRequest } from '../../../../backend/services/user/models/CreateUserRequest';
import { CreateVisitorProfileRequest } from '../../../../backend/services/user/models/CreateVisitorProfileRequest';


@Component({
    selector: 'app-register-user-form',
    standalone: true,
    imports: [
        CommonModule,
        ReactiveFormsModule,
    ],
    templateUrl: './register-user-form.component.html',
    styleUrls: ['./register-user-form.component.css']
})

export class RegisterUserFormComponent implements OnInit {
    form!: FormGroup;

    @Output() formSubmit = new EventEmitter<CreateUserRequest>();

    constructor(private fb: FormBuilder) {}

    ngOnInit() {
        this.form = this.fb.group({
            name: ['', Validators.required],
            lastName: ['', Validators.required],
            email: ['', [Validators.required, Validators.email]],
            password: ['', [Validators.required, Validators.minLength(8)]],
            dateOfBirth: ['', Validators.required],
        });
    }

    c(name: string): AbstractControl {
        return this.form.get(name)!;
    }

    invalid(name: string): boolean {
        const ctl = this.c(name);
        return ctl.touched && ctl.invalid;
    }

    emitForm() {
        if (this.form.invalid) {
            this.form.markAllAsTouched();
            return;
        }

        const v = this.form.value;

        const visitorProfile: CreateVisitorProfileRequest = {
            dateOfBirth: v.dateOfBirth,
            membership: 'Standard',
            score: '0'
        }
        const user: CreateUserRequest = {
            name: v.name!.trim(),
            lastName: v.lastName!.trim(),
            email: v.email!.trim().toLowerCase(),
            password: v.password!,
            rolesIds: ['CCCC1111-1111-1111-1111-111111111111'],
            visitorProfile: visitorProfile
        };

        this.formSubmit.emit(user);

    }
}