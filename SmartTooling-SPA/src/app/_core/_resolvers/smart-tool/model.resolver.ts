import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { CaptionConstants } from '../../_constants/system.constants';
import { Model } from '../../_models/smart-tool/model';
import { NgSnotifyService } from '../../_services/ng-snotify.service';
import { ModelService } from '../../_services/smart-tool/model.service';

@Injectable()
export class ModelResolver implements  Resolve<Model[]> {
    pageNumber = 1;
    pageSize = 10;
    modelParam = {};
    constructor(private modelService: ModelService, private router: Router, private snotifyAlert: NgSnotifyService) { }

    resolve(route: ActivatedRouteSnapshot): Observable<Model[]> {
        return this.modelService.search(this.pageNumber, this.pageSize, this.modelParam).pipe(
            catchError(error => {
                this.snotifyAlert.error('Problem retrieving data', CaptionConstants.ERROR);
                this.router.navigate(['/dashboard']);
                return of(null);
            }),
        );
    }
}
