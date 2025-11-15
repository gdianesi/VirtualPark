import { Component, Input, OnChanges, OnDestroy, OnInit, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';
import { UserService } from '../../../../backend/services/user/user.service';
import { VisitorProfileService } from '../../../../backend/services/visitorProfile/visitorProfile.service';
import { GetUserResponse } from '../../../../backend/services/user/models/GetUserResponse';
import { VisitorProfileModel } from '../../../../backend/services/visitorProfile/models/VisitorProfileModel';
import { EditUserRequest } from '../../../../backend/services/user/models/EditUserRequest';
import { CreateVisitorProfileRequest } from '../../../../backend/services/user/models/CreateVisitorProfileRequest';

@Component({
    selector: 'app-user-edit-form',
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule],
    templateUrl: './user-edit-form.component.html',
    styleUrls: ['./user-edit-form.component.css']
})
export class UserEditFormComponent implements OnInit, OnChanges, OnDestroy {
    @Input() userId = '';

    form!: FormGroup;
    loading = false;
    saving = false;
    errorMessage: string | null = null;
    successMessage: string | null = null;
    visitorProfile: VisitorProfileModel | null = null;
    readonly membershipOptions: string[] = ['Standard', 'Premium', 'VIP'];

    private userResponse: GetUserResponse | null = null;
    private visitorProfileSub?: Subscription;
    private roles: string[] = [];

    constructor(
        private fb: FormBuilder,
        private userService: UserService,
        private visitorProfileService: VisitorProfileService
    ) { }

    ngOnInit(): void {
        this.form = this.fb.group({
            name: ['', Validators.required],
            lastName: ['', Validators.required],
            email: [{ value: '', disabled: true }, [Validators.required, Validators.email]],
            dateOfBirth: [''],
            membership: [''],
            score: ['']
        });
    }

    ngOnChanges(changes: SimpleChanges): void {
        if (changes['userId'] && changes['userId'].currentValue) {
            this.loadUser(changes['userId'].currentValue);
        }
    }

    ngOnDestroy(): void {
        this.visitorProfileSub?.unsubscribe();
    }

    submit(): void {
        if (!this.userId || this.form.invalid || !this.userResponse) {
            this.form.markAllAsTouched();
            return;
        }

        this.saving = true;
        this.errorMessage = null;
        this.successMessage = null;

        const raw = this.form.getRawValue();

        const roleIds = [...(this.roles ?? [])];
        if (!roleIds.length) {
            this.saving = false;
            this.errorMessage = 'El usuario debe tener al menos un rol asignado.';
            return;
        }

        let visitorProfilePayload: CreateVisitorProfileRequest | undefined;
        if (this.visitorProfile) {
            const dateOfBirth = raw.dateOfBirth || this.normalizeDate(this.visitorProfile.dateOfBirth);
            const membershipValue = (raw.membership ?? this.visitorProfile.membership ?? '').toString().trim();
            const scoreValue = (raw.score ?? this.visitorProfile.score ?? '').toString().trim();
            if (!dateOfBirth || !membershipValue || !scoreValue) {
                this.saving = false;
                this.errorMessage = 'Completa los datos del perfil del visitante.';
                return;
            }
            visitorProfilePayload = {
                dateOfBirth,
                membership: membershipValue,
                score: scoreValue
            };
        }

        const payload: EditUserRequest = {
            name: raw.name.trim(),
            lastName: raw.lastName.trim(),
            email: this.userResponse.email,
            rolesIds: roleIds,
            visitorProfile: visitorProfilePayload
        };

        this.userService.update(this.userId, payload).subscribe({
            next: () => {
                this.saving = false;
                if (this.userResponse) {
                    this.userResponse = {
                        ...this.userResponse,
                        name: payload.name,
                        lastName: payload.lastName
                    };
                }
                if (this.visitorProfile && payload.visitorProfile) {
                    const updatedProfile = payload.visitorProfile;
                    const currentVisitor = this.visitorProfile;
                    this.visitorProfile = {
                        ...currentVisitor,
                        dateOfBirth: updatedProfile.dateOfBirth,
                        membership: updatedProfile.membership ?? currentVisitor.membership ?? '',
                        score: updatedProfile.score ?? currentVisitor.score ?? ''
                    };
                }
                this.successMessage = 'Cambios guardados correctamente.';
            },
            error: () => {
                this.saving = false;
                this.errorMessage = 'No se pudo guardar los cambios. Intenta nuevamente.';
            }
        });
    }

    private loadUser(id: string): void {
        if (!id) return;

        this.loading = true;
        this.errorMessage = null;
        this.successMessage = null;
        this.visitorProfile = null;
        this.visitorProfileSub?.unsubscribe();

        this.userService.getById(id).subscribe({
            next: (user) => {
                this.userResponse = user;
                this.roles = user.roles ?? [];
                this.form.patchValue({
                    name: user.name ?? '',
                    lastName: user.lastName ?? '',
                    email: user.email ?? ''
                });

                if (user.visitorProfileId) {
                    this.fetchVisitorProfile(user.visitorProfileId);
                } else {
                    this.form.patchValue({
                        dateOfBirth: '',
                        membership: '',
                        score: ''
                    });
                    this.loading = false;
                }
            },
            error: () => {
                this.loading = false;
                this.errorMessage = 'No se pudo obtener la información del usuario.';
            }
        });
    }

    private fetchVisitorProfile(visitorProfileId: string): void {
        this.visitorProfileSub = this.visitorProfileService.getById(visitorProfileId).subscribe({
            next: (profile) => {
                this.visitorProfile = profile;
                this.form.patchValue({
                    dateOfBirth: this.toDateInputValue(profile.dateOfBirth),
                    membership: profile.membership ?? '',
                    score: profile.score ?? ''
                });
                this.loading = false;
            },
            error: () => {
                this.loading = false;
                this.errorMessage = 'No se pudo obtener el perfil del visitante.';
            }
        });
    }

    private normalizeDate(value: string | null | undefined): string {
        if (!value) return '';
        const trimmed = value.trim();
        if (/^\d{4}-\d{2}-\d{2}$/.test(trimmed)) return trimmed;
        if (/^\d{4}-\d{2}-\d{2}T/.test(trimmed)) return trimmed.slice(0, 10);
        const parsed = new Date(trimmed);
        if (Number.isNaN(parsed.getTime())) return trimmed;
        const year = parsed.getUTCFullYear();
        const month = String(parsed.getUTCMonth() + 1).padStart(2, '0');
        const day = String(parsed.getUTCDate()).padStart(2, '0');
        return `${year}-${month}-${day}`;
    }

    private toDateInputValue(value: string | null | undefined): string {
        if (!value) return '';
        const trimmed = value.trim();
        if (/^\d{4}-\d{2}-\d{2}$/.test(trimmed)) return trimmed;
        if (/^\d{4}-\d{2}-\d{2}T/.test(trimmed)) return trimmed.slice(0, 10);
        const parsed = new Date(trimmed);
        if (Number.isNaN(parsed.getTime())) return trimmed;
        const year = parsed.getUTCFullYear();
        const month = String(parsed.getUTCMonth() + 1).padStart(2, '0');
        const day = String(parsed.getUTCDate()).padStart(2, '0');
        return `${year}-${month}-${day}`;
    }
}
