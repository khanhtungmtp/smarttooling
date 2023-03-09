import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { CaptionConstants } from '../../_constants/system.constants';
import { Model } from '../../_models/smart-tool/model';
import { NgSnotifyService } from '../../_services/ng-snotify.service';
import { ModelService } from '../../_services/smart-tool/model.service';

@Injectable()
export class ModelEditResolver implements Resolve<Model> {

    constructor(private modelService: ModelService, private router: Router, private snotifyAlert: NgSnotifyService) { }

    resolve(route: ActivatedRouteSnapshot): Observable<Model> {
        const modelNo = route.paramMap.get('modelNo');
        return this.modelService.getModelNoEdit(modelNo).pipe(
            catchError(error => {
                this.snotifyAlert.error('Problem retrieving your data', CaptionConstants.ERROR);
                this.router.navigate(['/category']);
                return of(null);
            }),
        );
    }
}
