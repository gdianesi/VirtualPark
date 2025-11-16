import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RoleRoutingModule } from './role-routing.module';
import { RolePageComponent } from './role-page/role-page.component';
import { RoleCreatePageComponent } from './role-create-page/role-create-page.component';

@NgModule({
  declarations: [],
  imports: [CommonModule, RoleRoutingModule, RolePageComponent, RoleCreatePageComponent]
})
export class RoleModule { }
