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
    });
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
    if (this.form.invalid) {
      this.messageSvc.show('Please complete all required fields.', 'info');
      this.form.markAllAsTouched();
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

        const backendMsg =
          err?.error?.message ||
          err?.message ||
          'An unexpected error occurred while creating the event.';

        let userMsg = backendMsg;
        if (backendMsg.includes('non-negative') || backendMsg.includes('non-zero')) {
          userMsg = 'The cost must be greater than zero.';
        }

        this.messageSvc.show('Error creating event: ' + userMsg, 'error');
        console.error('Create event error:', err);
      }

    });
  }

  cancel(): void {
    this.router.navigate(['/events']);
  }
}
