import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators, AbstractControl } from '@angular/forms';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';

type CreateAttractionPayload = {
  Name: string;
  Type: string;
  MiniumAge: string;
  Capacity: string;
  Description: string;
  Available: string;
};

@Component({
  selector: 'app-create-attraction-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './create-attraction-form.component.html',
  styleUrls: ['./create-attraction-form.component.css']
})
export class CreateAttractionFormComponent implements OnInit {
  form!: FormGroup;

  private http = inject(HttpClient);
  private fb = inject(FormBuilder);
  private apiUrl = '/api/attractions';

  attractionTypes = [
    'RollerCoaster',
    'FerrisWheel',
    'WaterRide',
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
  }

  c(name: string): AbstractControl {
    return this.form.get(name)!;
  }

  invalid(name: string): boolean {
    const ctl = this.c(name);
    return ctl.touched && ctl.invalid;
  }

  async submit(): Promise<void> {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const v = this.form.value;

    const payload: CreateAttractionPayload = {
      Name: (v.name as string).trim(),
      Type: v.type as string,
      MiniumAge: String(v.miniumAge).trim(),
      Capacity: String(v.capacity).trim(),
      Description: (v.description as string).trim(),
      Available: v.available as string
    };

    try {
      await this.http.post(this.apiUrl, payload).toPromise();
      alert('Attraction creada con éxito');
      this.form.reset({
        name: '',
        type: '',
        miniumAge: '',
        capacity: '',
        description: '',
        available: 'true'
      });
    } catch (err) {
      const e = err as HttpErrorResponse;
      console.error(e);
      alert(`Error creando la attraction: ${e.status} ${e.message}`);
    }
  }
}