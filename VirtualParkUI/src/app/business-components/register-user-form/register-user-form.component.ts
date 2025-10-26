import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators, AbstractControl } from '@angular/forms';

type RegisterPayload = {
    Name: string;
    LastName: string;
    Email: string;
    Password: string;
    DateOfBirth: string;
};

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

    submit() {
        if (this.form.invalid) {
            this.form.markAllAsTouched();
            return;
        }

        const v = this.form.value;
        const payload: RegisterPayload = {
            Name: v.name!.trim(),
            LastName: v.lastName!.trim(),
            Email: v.email!.trim().toLowerCase(),
            Password: v.password!,
            DateOfBirth: v.dateOfBirth!
        };

    }
}