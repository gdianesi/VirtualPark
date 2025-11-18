import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MessageService } from '../../../backend/services/message/message.service';

@Component({
  selector: 'app-message',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div *ngIf="messageService.message$ | async as message"
         [ngClass]="(messageService.type$ | async) ?? ''"
         class="message-box">
      {{ message }}
    </div>
  `,
  styleUrls: ['message.component.css']
})
export class MessageComponent {
  constructor(public messageService: MessageService) {}
}
