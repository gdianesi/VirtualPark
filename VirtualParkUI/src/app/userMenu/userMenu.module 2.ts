import { NgModule } from '@angular/core';
import { UserPageComponent } from '../userMenu/user-page/user-page.component';
import { UserMenuRoutingModule } from './userMenu-routing.module';

@NgModule({
  imports: [UserMenuRoutingModule, UserPageComponent],
  declarations: [],
})
export class UserMenuModule {}
