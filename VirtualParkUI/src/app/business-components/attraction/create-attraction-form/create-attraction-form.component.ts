import { Component, EventEmitter, Input, OnInit, Output, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators, AbstractControl } from '@angular/forms';

export type AttractionFormValue = {
    Name: string;
    Type: 'RollerCoaster' | 'Simulator' | 'Show';
    MiniumAge: string;
    Capacity: string;
    Description: string;
    Available: string;
};

export type AttractionInitial = Partial<{
    name: string;
    type: AttractionFormValue['Type'];
    miniumAge: string | number;
    minimumAge: string | number;
    capacity: string | number;
    description: string;
    available: string | boolean;
}>;

@Component({
    selector: 'app-create-attraction-form',
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule],
    templateUrl: './create-attraction-form.component.html',
    styleUrls: ['./create-attraction-form.component.css']
})

export class CreateAttractionFormComponent implements OnInit {
    private _initial?: AttractionInitial;
    @Input()
    set initial(value: AttractionInitial | undefined) {
        this._initial = value;
        if (value && this.form) {
            this.patchForm(value);
        }
    }
    get initial(): AttractionInitial | undefined {
        return this._initial;
    }
    @Input() submitText = 'Create';
    @Output() formSubmit = new EventEmitter<AttractionFormValue>();

    form!: FormGroup;
    private fb = inject(FormBuilder);

    attractionTypes = [
        'RollerCoaster',
        'Simulator',
        'Show'
    ];

    ngOnInit(): void {
        this.form = this.fb.group({
            name: ['', [Validators.required, Validators.maxLength(100)]],
            type: ['', [Validators.required]],
            miniumAge: ['', [Validators.required, Validators.pattern(/^\d+$/)]],
            capacity: ['', [Validators.required, Validators.pattern(/^\d+$/)]],
            description: ['', [Validators.required, Validators.maxLength(500)]],
            available: ['true', [Validators.required]]
        });

         if (this._initial) {
            this.patchForm(this._initial);
        }
    }

    c(name: string): AbstractControl { return this.form.get(name)!; }
    invalid(name: string): boolean { const ctl = this.c(name); return ctl.touched && ctl.invalid; }

    submit(): void {
        if (this.form.invalid) { this.form.markAllAsTouched(); return; }
        const v = this.form.value;
        const payload: AttractionFormValue = {
            Name: (v.name as string).trim(),
            Type: v.type as any,
            MiniumAge: String(v.miniumAge).trim(),
            Capacity: String(v.capacity).trim(),
            Description: (v.description as string).trim(),
            Available: v.available as string
        };
        this.formSubmit.emit(payload);
    }

    private patchForm(initial: AttractionInitial): void {
        const age = (initial.miniumAge ?? initial.minimumAge ?? '') as string | number;
        const available = (typeof initial.available === 'boolean')
            ? String(initial.available)
            : (initial.available ?? 'true');
        this.form.patchValue({
            name: initial.name ?? '',
            type: initial.type ?? '',
            miniumAge: String(age ?? ''),
            capacity: String(initial.capacity ?? ''),
            description: initial.description ?? '',
            available: available === 'false' ? 'false' : 'true'
        });
    }
}