import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TypeIncidenceService } from '../../../backend/services/type-incidence/type-incidence.service';
import { TypeIncidenceModel } from '../../../backend/services/type-incidence/models/TypeIncidenceModel';
import { ButtonsComponent } from '../../components/buttons/buttons.component';
import { MessageComponent } from '../../components/messages/message.component';
import { MessageService } from '../../components/messages/service/message.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-type-incidence-list',
  standalone: true,
  imports: [CommonModule, ButtonsComponent, MessageComponent],
  templateUrl: './type-incidence-list.component.html',
  styleUrls: ['./type-incidence-list.component.css']
})
export class TypeIncidenceListComponent implements OnInit {
  types: TypeIncidenceModel[] = [];
  loading = false;

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

  delete(id: string) {
    if (confirm('Are you sure you want to delete this type?')) {
      this.service.delete(id).subscribe({
        next: () => {
          this.messageService.show('Type deleted successfully.', 'success');
          this.load();
        },
        error: () => this.messageService.show('Error deleting type.', 'error')
      });
    }
  }
}
