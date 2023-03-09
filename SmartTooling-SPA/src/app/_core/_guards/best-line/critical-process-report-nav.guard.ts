import { Injectable } from '@angular/core';
import { CanLoad, Router} from '@angular/router';


@Injectable({
  providedIn: 'root'
})
export class CriticalProcessReportNavGuard implements CanLoad {
  constructor(private router: Router) { }
  canLoad(): boolean {
    const user = JSON.parse(localStorage.getItem('userSmartTooling'));
    if (user.role.includes('bl.CriticalProcessReport')) {
      return true;
    } else {
      this.router.navigate(['/dashboard']);
    }
  }
}
