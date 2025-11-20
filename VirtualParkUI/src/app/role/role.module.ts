import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RoleRoutingModule } from './role-routing.module';
import { RolePageComponent } from './role-page/role-page.component';
import { RoleCreatePageComponent } from './role-create-page/role-create-page.component';
import { RoleListPageComponent } from './role-list-page/role-list-page.component';
import { RoleEditPageComponent } from './role-edit-page/role-edit-page.component';

@NgModule({
  declarations: [],
  imports: [CommonModule, RoleRoutingModule, RolePageComponent, RoleCreatePageComponent, RoleListPageComponent, RoleEditPageComponent]
})
export class RoleModule { }
