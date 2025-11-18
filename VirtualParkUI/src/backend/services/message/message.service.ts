import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

export type MessageType = 'success' | 'error' | 'info' | null;

@Injectable({ providedIn: 'root' })
export class MessageService {
  private messageSource = new BehaviorSubject<string | null>(null);
  private typeSource = new BehaviorSubject<MessageType>(null);

  message$ = this.messageSource.asObservable();
  type$ = this.typeSource.asObservable();

  show(message: string, type: MessageType = 'info') {
    this.messageSource.next(message);
    this.typeSource.next(type);

    setTimeout(() => this.clear(), 4000);
  }

  clear() {
    this.messageSource.next(null);
    this.typeSource.next(null);
  }
}
