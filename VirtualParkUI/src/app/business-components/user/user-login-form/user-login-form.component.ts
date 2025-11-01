import {Component, OnInit, Output, EventEmitter, ViewChild} from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators, AbstractControl } from '@angular/forms';
import { LoginRequest } from '../../../backend/services/session/models/LoginRequest';

@Component({
    selector: 'app-user-login-form',
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule],
    templateUrl: './user-login-form.component.html',
    styleUrls: ['./user-login-form.component.css']
})
export class UserLoginFormComponent implements OnInit {
    form!: FormGroup;
    
    @ViewChild(UserLoginFormComponent) loginForm!: UserLoginFormComponent;
    @Output() formSubmit = new EventEmitter<LoginRequest>();

    constructor(private fb: FormBuilder) {}

    ngOnInit() {
        this.form = this.fb.group({
            email: ['', [Validators.required, Validators.email]],
            password: ['', [Validators.required, Validators.minLength(6)]]
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
        const credentials: LoginRequest = {
            email: v.email!.trim().toLowerCase(),
            password: v.password!
        };

        this.formSubmit.emit(credentials);
    }
}