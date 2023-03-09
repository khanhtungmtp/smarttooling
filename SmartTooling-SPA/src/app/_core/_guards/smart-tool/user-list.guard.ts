import { Injectable } from '@angular/core';
import { CanLoad, Router } from '@angular/router';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserListGuard implements CanLoad {
  constructor(private router: Router) { }
  canLoad(): Observable<boolean> | Promise<boolean> | boolean {
      const user = JSON.parse(localStorage.getItem('userSmartTooling'));
      if (user.role.includes('ksmt.UserList')) {
        return true;
      } else {
        this.router.navigate(['/dashboard']);
      }
  }
}
