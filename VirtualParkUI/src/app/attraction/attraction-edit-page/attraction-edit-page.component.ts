import { Component, inject } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { ButtonsComponent } from '../../components/buttons/buttons.component';
import { CreateAttractionFormComponent } from '../../business-components/create-attraction-form/create-attraction-form.component';

@Component({
  selector: 'app-attraction-edit-page',
  standalone: true,
  imports: [RouterLink, ButtonsComponent, CreateAttractionFormComponent],
  templateUrl: './attraction-edit-page.component.html',
  styleUrls: ['./attraction-edit-page.component.css']
})
export class AttractionEditPageComponent {
  private route = inject(ActivatedRoute);
  private http = inject(HttpClient);
  private apiUrl = '/api/attractions';

  id = this.route.snapshot.paramMap.get('id')!;
  initial: any = null;

  constructor() {
    this.http.get(`${this.apiUrl}/${this.id}`).subscribe({
      next: (dto) => this.initial = dto,
      error: () => alert('No se pudo cargar la atracción')
    });
  }

  onSubmit(payload: any) {
    this.http.put(`${this.apiUrl}/${this.id}`, payload).subscribe({
      next: () => alert('Attraction actualizada'),
      error: () => alert('Error actualizando')
    });
  }
}