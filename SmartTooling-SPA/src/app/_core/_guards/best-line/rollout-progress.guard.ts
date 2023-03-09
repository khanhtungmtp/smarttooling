import { Injectable } from '@angular/core';
import { CanActivate, CanLoad, Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class RolloutProgressGuard implements CanLoad {
  constructor(private router: Router) { }
  canLoad(): boolean {
    const user: any = JSON.parse(localStorage.getItem('userSmartTooling'));
    if (user.role.includes("bl.RolloutProgress")) {
      return true;
    } else {
      this.router.navigate(['/dashboard']);
    }
  }
}
