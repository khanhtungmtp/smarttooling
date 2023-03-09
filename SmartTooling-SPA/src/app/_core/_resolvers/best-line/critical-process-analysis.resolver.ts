import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { SearchBLCriticalParam } from '../../_helpers/params/best-line/search-critical-Param';
import { BLCriticalProcess } from '../../_models/best-line/bl-critical-process';
import { Pagination, PaginationResult } from '../../_models/best-line/pagination-best-line';
import { CriticalProcessAnalysisService } from '../../_services/best-line/critical-process-analysis.service';


@Injectable({ providedIn: 'root' })
export class CriticalProcessAnalysisResolver implements Resolve<PaginationResult<BLCriticalProcess>> {

    SearchBLCriticalParam: SearchBLCriticalParam = {
        lineNo: '',
        lineType: '',
        modelNo: '',
        stage: '',

    };
    pagination: Pagination = {
        pageSize: 10,
        currentPage: 1
    } as Pagination;

    constructor(private criticalProcessAnalysisService: CriticalProcessAnalysisService) { }
    resolve(route: ActivatedRouteSnapshot): Observable<PaginationResult<BLCriticalProcess>> {
        return this.criticalProcessAnalysisService.getAll(this.SearchBLCriticalParam, this.pagination);
    }
}
