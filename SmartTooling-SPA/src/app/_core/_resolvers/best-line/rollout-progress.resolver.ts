import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { Observable, of } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { CaptionConstants } from '../../_constants/system.constants';
import { BL_Lines } from '../../_models/best-line/bl-lines';
import { RolloutProgressService } from '../../_services/best-line/rollout-progress.service';
import { NgSnotifyService } from '../../_services/ng-snotify.service';

@Injectable({
    providedIn: 'root'
})
export class RolloutProgress implements Resolve<BL_Lines[]> {

    constructor(
        private rolloutProgressService: RolloutProgressService, 
        private router: Router, 
        private spinnerService: NgxSpinnerService,
        private snotifyAlert: NgSnotifyService
    ) { }

    resolve(): Observable<BL_Lines[]> {
        this.spinnerService.show();
        return this.rolloutProgressService.getLineNo().pipe(
            tap(() => this.spinnerService.hide()),
            catchError(error => {
                this.snotifyAlert.error('Problem retrieving your data', CaptionConstants.ERROR);
                this.router.navigate(['/dashboard']);
                this.spinnerService.hide();
                return of(null);
            }),
        );
    }
}
