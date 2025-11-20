import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthRoleService {

  getUserRoles(): string[] {
    const stored = localStorage.getItem('roles');
    return stored ? JSON.parse(stored) : [];
  }

  hasAnyRole(roles: string[]): boolean {
    const userRoles = this.getUserRoles();
    return roles.some(r => userRoles.includes(r));
  }

  hasAllRoles(roles: string[]): boolean {
    const userRoles = this.getUserRoles();
    return roles.every(r => userRoles.includes(r));
  }
}
