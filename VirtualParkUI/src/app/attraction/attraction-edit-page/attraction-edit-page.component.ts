import { Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ButtonsComponent } from '../../components/buttons/buttons.component';
import { AttractionFormValue, AttractionInitial, CreateAttractionFormComponent } from '../../business-components/attraction/create-attraction-form/create-attraction-form.component';
import { AttractionService } from '../../../backend/services/attraction/attraction.service';
import { GetAttractionResponse } from '../../../backend/services/attraction/models/GetAttractionRequest';

@Component({
  selector: 'app-attraction-edit-page',
  standalone: true,
  imports: [RouterLink, ButtonsComponent, CreateAttractionFormComponent],
  templateUrl: './attraction-edit-page.component.html',
  styleUrls: ['./attraction-edit-page.component.css']
})
export class AttractionEditPageComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private attractionService = inject(AttractionService);

  id = this.route.snapshot.paramMap.get('id')!;
  initial: AttractionInitial | undefined;

  ngOnInit(): void {
    this.attractionService.getById(this.id).subscribe({
      next: (dto) => this.initial = this.mapInitial(dto),
      error: () => alert('No se pudo cargar la atracción')
    });
  }

  onSubmit(payload: AttractionFormValue) {
    this.attractionService.update(this.id, payload).subscribe({
      next: () => alert('Attraction actualizada'),
      error: () => alert('Error actualizando')
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
