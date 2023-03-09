import { BLAuditReportDetailDTO } from './../../_models/best-line/bl-audit-report-detail.model';
import { ActivatedRouteSnapshot, Resolve, Router } from "@angular/router";
import { forkJoin, Observable, of, throwError } from 'rxjs';
import { BlAuditReportService } from '../../_services/best-line/bl-audit-report.service';
import { PaginatedResult, Pagination } from '../../_models/smart-tool/pagination';
import { BLSearchAuditReportParam } from '../../_helpers/params/best-line/bl-search-audit-report.param';
import { Injectable } from '@angular/core';
import { tap, catchError, switchMap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuditReportDetailResolver implements Resolve<Observable<[BLSearchAuditReportParam, PaginatedResult<BLAuditReportDetailDTO[]>]>> {

  detailParam: BLSearchAuditReportParam;

  pagination: Pagination = {
    currentPage: 1,
    itemsPerPage: 5
  } as Pagination;

  constructor(
    private auditService: BlAuditReportService,
    private router: Router
  ) { }

  resolve(route: ActivatedRouteSnapshot): Observable<[BLSearchAuditReportParam, PaginatedResult<BLAuditReportDetailDTO[]>]> {
    // return this.auditService.currentItem.pipe(
    //   switchMap(param => {
    //     this.detailParam = param;
    //     let dataDetail = this.auditService.getAuditDetail(this.detailParam, this.pagination);
    //     return forkJoin([of(this.detailParam), dataDetail]);
    //   })
    // );

    this.auditService.currentItem.subscribe(res => {
      if(res == null)
        this.router.navigate(['/best-line/report/audit-report']);
      this.detailParam = res;
    })
    var dataDetail = this.auditService.getAuditDetail(this.detailParam, this.pagination);

    return forkJoin([of(this.detailParam), dataDetail]);
  }

}
