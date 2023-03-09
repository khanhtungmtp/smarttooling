import { Injectable } from '@angular/core';
import { CanLoad, Router} from '@angular/router';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PilotLineSetupSummaryTrackingGuard implements CanLoad {
  constructor(private router: Router) { }
  canLoad(): boolean {
    const user = JSON.parse(localStorage.getItem('userSmartTooling'));
    if (user.role.includes('bl.PilotLineSetupSummaryTracking')) {
      return true;
    } else {
      this.router.navigate(['/dashboard']);
    }
  }
  
}
