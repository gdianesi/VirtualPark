import { Component, EventEmitter, OnDestroy, OnInit, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { RoleService } from '../../../../backend/services/role/role.service';
import { RoleModel } from '../../../../backend/services/role/models/RoleModel';
import { CreateUserRequest } from '../../../../backend/services/user/models/CreateUserRequest';
import { CreateVisitorProfileRequest } from '../../../../backend/services/user/models/CreateVisitorProfileRequest';
import { Subscription } from 'rxjs';
import { MessageService } from '../../../../backend/services/message/message.service';

@Component({
    selector: 'app-create-user-form',
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule],
    templateUrl: './create-user-form.component.html',
    styleUrls: ['./create-user-form.component.css']
})
export class CreateUserFormComponent implements OnInit, OnDestroy {
    @Output() formSubmit = new EventEmitter<CreateUserRequest>();

    form!: FormGroup;
    roles: RoleModel[] = [];
    loadingRoles = true;
    rolesError = '';
    readonly membershipOptions = ['Standard', 'Premium', 'VIP'];

    private roleSub?: Subscription;

    constructor(
        private fb: FormBuilder,
        private roleService: RoleService,
        private messageService: MessageService
    ) {}

    ngOnInit(): void {
        this.form = this.fb.group({
            name: ['', Validators.required],
            lastName: ['', Validators.required],
            email: ['', [Validators.required, Validators.email]],
            password: ['', [Validators.required, Validators.minLength(8)]],
            roleId: ['', Validators.required],
            dateOfBirth: [''],
            membership: ['Standard'],
        });

        this.form.get('roleId')?.valueChanges.subscribe(() => this.updateVisitorValidators());
        this.loadRoles();
    }

    ngOnDestroy(): void {
        this.roleSub?.unsubscribe();
    }

    emitForm(): void {
        if (this.form.invalid || this.loadingRoles || this.rolesError) {
            this.form.markAllAsTouched();
            this.messageService.show('Please fill all required fields.', 'info');
            return;
        }

        const raw = this.form.getRawValue();
        const selectedRoleId: string = raw.roleId;

        const visitorProfile: CreateVisitorProfileRequest | undefined = this.isVisitorSelected
            ? {
                dateOfBirth: raw.dateOfBirth,
                membership: raw.membership,
                score: '0'
            }
            : undefined;

        const payload: CreateUserRequest = {
            name: raw.name.trim(),
            lastName: raw.lastName.trim(),
            email: raw.email.trim().toLowerCase(),
            password: raw.password,
            rolesIds: [selectedRoleId],
            visitorProfile
        };

        this.formSubmit.emit(payload);
    }

    get isVisitorSelected(): boolean {
        const selectedId = this.form?.get('roleId')?.value;
        const role = this.roles.find(r => r.id === selectedId);
        return (role?.name ?? '').toLowerCase() === 'visitor';
    }

    private loadRoles(): void {
        this.loadingRoles = true;
        this.rolesError = '';
        this.roleSub = this.roleService.getAll().subscribe({
            next: (roles) => {
                this.roles = roles ?? [];
                this.loadingRoles = false;
            },
            error: () => {
                this.loadingRoles = false;
                this.rolesError = 'No se pudieron cargar los roles disponibles.';
            }
        });
    }

    private updateVisitorValidators(): void {
        const dateCtrl = this.form.get('dateOfBirth');
        const membershipCtrl = this.form.get('membership');

        if (this.isVisitorSelected) {
            dateCtrl?.addValidators([Validators.required]);
            membershipCtrl?.addValidators([Validators.required]);
        } else {
            dateCtrl?.clearValidators();
            membershipCtrl?.clearValidators();
            dateCtrl?.setValue('');
            membershipCtrl?.setValue('Standard');
        }

        dateCtrl?.updateValueAndValidity();
        membershipCtrl?.updateValueAndValidity();
    }
}
