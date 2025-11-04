import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RolePermissionPageComponent } from './role-permission-page/role-permission-page.component';
import { RoleRoutingModule } from './role-routing.module';

@NgModule({
  declarations: [RolePermissionPageComponent],
  imports: [CommonModule, RoleRoutingModule]
})
export class RoleModule { }
