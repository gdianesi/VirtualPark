import { Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { ButtonsComponent } from '../../components/buttons/buttons.component';
import { AttractionFormValue, AttractionInitial, CreateAttractionFormComponent } from '../../business-components/attraction/create-attraction-form/create-attraction-form.component';
import { AttractionService } from '../../../backend/services/attraction/attraction.service';
import { GetAttractionResponse } from '../../../backend/services/attraction/models/GetAttractionRequest';
import { MessageService } from '../../../backend/services/message/message.service';
import { MessageComponent } from '../../components/messages/message.component';

@Component({
  selector: 'app-attraction-edit-page',
  standalone: true,
  imports: [RouterLink, ButtonsComponent, CreateAttractionFormComponent, MessageComponent],
  templateUrl: './attraction-edit-page.component.html',
  styleUrls: ['./attraction-edit-page.component.css']
})
export class AttractionEditPageComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private attractionService = inject(AttractionService);
  private messageService = inject(MessageService);
  private router = inject(Router);

  id = this.route.snapshot.paramMap.get('id')!;
  initial: AttractionInitial | undefined;

  ngOnInit(): void {
    this.attractionService.getById(this.id).subscribe({
      next: (dto) => this.initial = this.mapInitial(dto),
      error: (e) => this.messageService.show(e.error?.message ?? 'Error creating attraction.', 'error')
    });
  }

onSubmit(payload: AttractionFormValue) {
  this.attractionService.update(this.id, payload).subscribe({
    next: () => {
      this.messageService.show('Attraction updated successfully!', 'success');
      this.router.navigate(['/attraction']);
    },
    error: (e) =>
      this.messageService.show(e.error?.message ?? 'Error updating attraction.', 'error')
  });
}

  private mapInitial(dto: GetAttractionResponse): AttractionInitial {
    return {
      name: dto.Name ?? (dto as any).name ?? '',
      type: dto.Type as AttractionFormValue['Type'],
      miniumAge: dto.MiniumAge ?? (dto as any).miniumAge ?? '',
      capacity: dto.Capacity ?? (dto as any).capacity ?? '',
      description: dto.Description ?? (dto as any).description ?? '',
      available: dto.Available ?? (dto as any).available ?? 'true'
    };
  }
}
