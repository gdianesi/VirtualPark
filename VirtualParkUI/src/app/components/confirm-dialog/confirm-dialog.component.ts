import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ButtonsComponent } from '../buttons/buttons.component';

@Component({
  selector: 'app-confirm-dialog',
  standalone: true,
  imports: [CommonModule, ButtonsComponent],
  templateUrl: './confirm-dialog.component.html',
  styleUrls: ['./confirm-dialog.component.css']
})
export class ConfirmDialogComponent {
  @Input() title: string = 'Confirm';
  @Input() message: string = 'Are you sure?';
  @Input() show = false;
  @Input() confirmLabel = 'Confirm';
  @Input() confirmColor: 'primary' | 'danger' | 'secondary' = 'primary';
  @Input() cancelLabel = 'Cancel';
  @Input() cancelColor: 'primary' | 'danger' | 'secondary' = 'secondary';

  @Output() confirmed = new EventEmitter<boolean>();

  confirm(value: boolean) {
    this.confirmed.emit(value);
    this.show = false;
  }
}
