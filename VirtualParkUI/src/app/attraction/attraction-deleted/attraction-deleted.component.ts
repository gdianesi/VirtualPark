import { Component, Input, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AttractionService } from '../../../backend/services/attraction/attraction.service';
import { ButtonsComponent } from '../../components/buttons/buttons.component';
import { ConfirmDialogComponent } from '../../components/confirm-dialog/confirm-dialog.component';
import { MessageComponent } from "../../components/messages/message.component";
import { MessageService } from '../../../backend/services/message/message.service';
import { AuthRoleService } from '../../../backend/services/auth/auth-role.service';
import { RouterLink } from '@angular/router';
import { CreateAttractionRequest } from '../../../backend/services/attraction/models/CreateAttractionRequest';
import { AttractionType } from '../../../backend/services/attraction/models/AttractionType';

type Row = {
  id: string;
  name: string;
  type: string;
  miniumAge: string;
  capacity: string;
  description: string;
  available: string;
  showConfirm?: boolean;
};

@Component({
  selector: 'app-attraction-deleted-page',
  standalone: true,
  imports: [
    CommonModule,
    ButtonsComponent,
    ConfirmDialogComponent,
    MessageComponent,
    RouterLink
  ],
  templateUrl: './attraction-deleted.component.html',
  styleUrls: ['./attraction-deleted.component.css']
})
export class AttractionDeletedComponent implements OnInit {

  private service = inject(AttractionService);
  authRole = inject(AuthRoleService);
  private messageService = inject(MessageService);
  @Input() show = false;

  deleted: Row[] = [];
  loading = true;

  ngOnInit(): void {

    if (!this.authRole.hasAnyRole(["Administrator"])) {
      this.loading = false;
      return;
    }

    this.service.getDeleted().subscribe({
      next: (items: any[]) => {
        this.deleted = items.map(it => ({
          id: it.id ?? it.Id,
          name: it.name ?? it.Name ?? '',
          type: it.type ?? it.Type ?? '',
          miniumAge: String(it.miniumAge ?? it.MiniumAge ?? ''),
          capacity: String(it.capacity ?? it.Capacity ?? ''),
          description: it.description ?? it.Description ?? '',
          available: ((it.available ?? it.Available ?? '') + '').toString().toLowerCase(),
        }));
        this.loading = false;
      },
      error: () => {
        this.messageService.show("Failed to load deleted attractions.", "error");
        this.loading = false;
      }
    });
  }

openConfirm(row: Row) {
  console.log("ABRIENDO CONFIRM", row);
  row.showConfirm = true;
}

  onConfirmDialog(value: boolean, row: Row) {
    if (value) {
      this.restore(row);
    } else {
      row.showConfirm = false;
    }
  }

  restore(row: Row) {

    const payload: CreateAttractionRequest = {
      Name: row.name,
      Type: row.type as AttractionType,
      MiniumAge: row.miniumAge,
      Capacity: row.capacity,
      Description: row.description,
      Available: row.available,
    };

    this.service.update(row.id, payload).subscribe({
      next: () => {
        this.messageService.show("Attraction restored successfully!", "success");
        this.deleted = this.deleted.filter(a => a.id !== row.id);
      },
      error: err => {
        const msg = err?.error?.message ??
                    err?.error?.Message ??
                    "Error restoring attraction.";
        this.messageService.show(msg, "error");
      }
    });
  }
}
