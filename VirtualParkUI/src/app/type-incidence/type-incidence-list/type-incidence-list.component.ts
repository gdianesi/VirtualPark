import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TypeIncidenceService } from '../../../backend/services/type-incidence/type-incidence.service';
import { TypeIncidenceModel } from '../../../backend/services/type-incidence/models/TypeIncidenceModel';
import { ButtonsComponent } from '../../components/buttons/buttons.component';
import { MessageComponent } from '../../components/messages/message.component';
import { MessageService } from '../../../backend/services/message/message.service';
import { Router } from '@angular/router';
import { ConfirmDialogComponent } from '../../components/confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-type-incidence-list',
  standalone: true,
  imports: [CommonModule, ButtonsComponent, MessageComponent, ConfirmDialogComponent],
  templateUrl: './type-incidence-list.component.html',
  styleUrls: ['./type-incidence-list.component.css']
})
export class TypeIncidenceListComponent implements OnInit {
  types: TypeIncidenceModel[] = [];
  loading = false;

  showDialog = false;
  pendingDeleteId: string | null = null;

  constructor(
    private readonly service: TypeIncidenceService,
    private readonly messageService: MessageService,
    private readonly router: Router
  ) {}

  ngOnInit() {
    this.load();
  }

  load() {
    this.loading = true;
    this.service.getAll().subscribe({
      next: (res) => {
        this.types = res;
        this.loading = false;
      },
      error: () => {
        this.messageService.show('Error loading types.', 'error');
        this.loading = false;
      }
    });
  }

  goToCreate() {
    this.router.navigate(['/typeincidences/new']);
  }

  openDialog(id: string) {
    this.pendingDeleteId = id;
    this.showDialog = true;
  }

  onConfirmed(result: boolean) {
    this.showDialog = false;

    if (!result || !this.pendingDeleteId) {
      this.pendingDeleteId = null;
      return;
    }

    this.service.delete(this.pendingDeleteId).subscribe({
      next: () => {
        this.messageService.show('Type deleted successfully.', 'success');
        this.load();
      },
      error: () => this.messageService.show('Error deleting type.', 'error')
    });

    this.pendingDeleteId = null;
  }
}
