import { Injectable } from '@angular/core';
import { CanLoad, Router } from '@angular/router';
import { User } from '../../_models/user';

@Injectable({
  providedIn: 'root'
})
export class LineBalancingGuard implements CanLoad {
  constructor( private router: Router) { }
  canLoad(): boolean {
    const user: User = JSON.parse(localStorage.getItem('userSmartTooling'));
    if (user.role.includes('bl.LineBalancing')) {
      return true;
    } else {
      this.router.navigate(['/dashboard']);
    }
  }
}
