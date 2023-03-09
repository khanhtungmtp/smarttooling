import { Injectable } from '@angular/core';
import { CanLoad, Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class ComputerStitchingSettingReportGuard implements CanLoad {
  constructor(private router: Router) { }
  canLoad(): boolean {
    const user: any = JSON.parse(localStorage.getItem('userSmartTooling'));
    if (user.role.includes("pbp.ComputerStitchingSettingReport")) {
      return true;
    } else {
      this.router.navigate(['/dashboard']);
    }
  }
}
