import { Injectable } from '@angular/core';
import { CanLoad, Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class C2BOverallLayoutGuard implements CanLoad {
  constructor(private router: Router) { }
  canLoad(): boolean {
    const user: any = JSON.parse(localStorage.getItem('userSmartTooling'));
    if (user.role.includes("bl.C2B.OverallLayout")) {
      return true;
    } else {
      this.router.navigate(['/dashboard']);
    }
  }
}
