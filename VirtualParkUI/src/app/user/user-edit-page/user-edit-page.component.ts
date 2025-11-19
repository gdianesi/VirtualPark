import { Component, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { UserEditFormComponent } from '../../business-components/user/user-edit-form/user-edit-form.component';
import { SessionService } from '../../../backend/services/session/session.service';

@Component({
    selector: 'app-user-edit-page',
    standalone: true,
    imports: [CommonModule, UserEditFormComponent],
    templateUrl: './user-edit-page.component.html',
    styleUrls: ['./user-edit-page.component.css']
})
export class UserEditPageComponent implements OnDestroy {
    userId: string | null = null;
    loading = false;
    errorMessage: string | null = null;
    private sessionSub?: Subscription;

    constructor(
        private route: ActivatedRoute,
        private sessionService: SessionService
    ) {
        this.route.paramMap.subscribe(params => {
            const paramId = params.get('id');
            if (paramId) {
                this.userId = paramId;
                this.errorMessage = null;
                this.loading = false;
            } else {
                this.resolveUserFromSession();
            }
        });
    }

    ngOnDestroy(): void {
        this.sessionSub?.unsubscribe();
    }

    private resolveUserFromSession(): void {
        this.loading = true;
        this.errorMessage = null;
        this.sessionSub?.unsubscribe();
        this.sessionSub = this.sessionService.getSession().subscribe({
            next: res => {
                this.userId = res?.id ?? null;
                this.loading = false;
                if (!this.userId) {
                    this.errorMessage = 'No valid user found.';
                }
            },
            error: () => {
                this.loading = false;
                this.errorMessage = 'Could not retrieve session information.';
            }
        });
    }
}
