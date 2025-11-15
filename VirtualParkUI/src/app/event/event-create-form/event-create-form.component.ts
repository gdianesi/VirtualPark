import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { EventService } from '../../../backend/services/event/event.service';
import { AttractionService } from '../../../backend/services/attraction/attraction.service';
import { ButtonsComponent } from '../../components/buttons/buttons.component';
import { MessageComponent } from '../../components/messages/message.component';
import { MessageService } from '../../components/messages/service/message.service';
import { CreateEventRequest } from '../../../backend/services/event/models/CreateEventRequest';
import { AttractionModel } from '../../../backend/services/attraction/models/AttractionModel';

@Component({
  selector: 'app-event-create',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, ButtonsComponent, MessageComponent],
  templateUrl: './event-create-form.component.html',
  styleUrls: ['./event-create-form.component.css']
})
export class EventCreateComponent implements OnInit {
  form!: FormGroup;
  attractions: AttractionModel[] = [];
  loading = false;

  constructor(
    private fb: FormBuilder,
    private eventSvc: EventService,
    private attractionSvc: AttractionService,
    private messageSvc: MessageService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.buildForm();
    this.loadAttractions();
  }

  private buildForm(): void {
    this.form = this.fb.group({
      name: ['', Validators.required],
      date: ['', Validators.required],
      capacity: [null, [Validators.required, Validators.min(1)]],
      cost: [null, [Validators.required, Validators.min(0)]],
      attractions: [[]]
    }, {
      validators: [this.futureDateValidator.bind(this)]
    });
  }


  private futureDateValidator(form: FormGroup) {
    const dateValue = form.get('date')?.value;
    if (!dateValue) return null;

    const eventDate = new Date(dateValue);
    const today = new Date();
    today.setHours(0, 0, 0, 0);

    return eventDate < today ? { pastDate: true } : null;
  }


  private loadAttractions(): void {
    this.attractionSvc.getAll().subscribe({
      next: list => (this.attractions = list),
      error: err => this.messageSvc.show('Error loading attractions: ' + err.message, 'error')
    });
  }

  onCheckboxChange(id: string, event: Event): void {
    const checked = (event.target as HTMLInputElement).checked;
    const selected = this.form.value.attractions as string[];

    if (checked) {
      this.form.patchValue({ attractions: [...selected, id] });
    } else {
      this.form.patchValue({ attractions: selected.filter(a => a !== id) });
    }
  }

  create(): void {
    const nameCtrl = this.form.get('name');
    const dateCtrl = this.form.get('date');
    const capacityCtrl = this.form.get('capacity');
    const costCtrl = this.form.get('cost');

    if (this.form.errors?.['pastDate']) {
      this.messageSvc.show('The event date must be today or in the future.', 'error');
      this.form.markAllAsTouched();
      return;
    }

    if (nameCtrl?.hasError('required') || dateCtrl?.hasError('required') ||
        capacityCtrl?.hasError('required') || costCtrl?.hasError('required')) {
      this.messageSvc.show('Please complete all required fields.', 'info');
      this.form.markAllAsTouched();
      return;
    }

    if (capacityCtrl?.hasError('min')) {
      this.messageSvc.show('Capacity must be greater than 0.', 'error');
      capacityCtrl.markAsTouched();
      return;
    }

    if (costCtrl?.hasError('min')) {
      this.messageSvc.show('Cost must be 0 or greater.', 'error');
      costCtrl.markAsTouched();
      return;
    }
  const request: CreateEventRequest = {
    name: this.form.value.name,
    date: this.form.value.date,
    capacity: this.form.value.capacity.toString(),
    cost: this.form.value.cost.toString(),
    attractionsIds: this.form.value.attractions
  };


    this.loading = true;
    this.eventSvc.create(request).subscribe({
      next: () => {
        this.loading = false;
        this.messageSvc.show('Event created successfully!', 'success');
        this.router.navigate(['/events']);
      },
error: (err) => {
  this.loading = false;

  const backendMsg = err?.error?.message;

  if (backendMsg) {
    this.messageSvc.show('Error creating event: ' + backendMsg, 'error');
    return;
  }

  this.messageSvc.show('Unexpected error occurred.', 'error');
}


    });
  }

  cancel(): void {
    this.router.navigate(['/events']);
  }
}
