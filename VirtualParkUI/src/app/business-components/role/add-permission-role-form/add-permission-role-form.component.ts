import { Component, EventEmitter, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms'; 

@Component({
    selector: 'app-add-permission-role-form',
    standalone: true,
    imports: [ReactiveFormsModule],
    templateUrl: './add-permission-role-form.component.html',
    styleUrls: ['./add-permission-role-form.component.css']
})
export class AddPermissionRoleFormComponent {
    @Output() formSubmit = new EventEmitter<any>();

    form: FormGroup;

    constructor(private fb: FormBuilder) {
        this.form = this.fb.group({
            roleId: ['', Validators.required],
            permissionsIds: [[]],
        });
    }

    onSubmit(): void {
        if (this.form.valid) {
            this.formSubmit.emit(this.form.value); 
        } else {
            this.form.markAllAsTouched();
        }
    }
}
