import { BLAuditReportDetailDTO } from './../../_models/best-line/bl-audit-report-detail.model';
import { BLAuditReportDTO } from './../../_models/best-line/bl-audit-report.model';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { BLSearchAuditReportParam } from '../../_helpers/params/best-line/bl-search-audit-report.param';
import { PaginatedResult, Pagination } from '../../_models/smart-tool/pagination';
import { map, tap } from 'rxjs/operators';
import { BehaviorSubject } from 'rxjs';
import { KeyValuePair } from '../../_models/key-value-pair';

@Injectable({
  providedIn: 'root'
})
export class BlAuditReportService {
  apiUrl = environment.apiUrl;
  detailSource = new BehaviorSubject<BLSearchAuditReportParam>(null);
  currentItem = this.detailSource.asObservable();
  //Search
  paramSearchSource = new BehaviorSubject<BLSearchAuditReportParam>(null);
  currentParamSearch = this.paramSearchSource.asObservable();

  constructor(
    private http: HttpClient
  ) { }

  setParamSearch = (item: BLSearchAuditReportParam) => this.paramSearchSource.next(item);
  setDetail = (item: BLSearchAuditReportParam) => this.detailSource.next(item);

  getAllAudit(search: BLSearchAuditReportParam, pagination: Pagination) {
    const paginatedResult: PaginatedResult<BLAuditReportDTO[]> = new PaginatedResult<BLAuditReportDTO[]>();
    const params = new HttpParams()
      .set('factory', search.factory)
      .set('lineID', search.lineID)
      .set('rolloutLineID', search.rolloutLineID)
      .set('lineTypeID', search.lineTypeID)
      .set('modelNo', search.modelNo.toUpperCase())
      .set('stageID', search.stageID)
      .set('operationID', search.operationID)
      .set('pageNumber', pagination.currentPage.toString())
      .set('pageSize', pagination.itemsPerPage.toString());
    return this.http.get<PaginatedResult<BLAuditReportDTO[]>>
      (`${this.apiUrl}BLAuditReport/GetAllAudit`, {observe: "response", params}).pipe(
        map(response => {
          paginatedResult.result = <any>response.body;
          if (response.headers.get("Pagination") != null) {
            paginatedResult.pagination = JSON.parse(response.headers.get("Pagination"));
          }
          return paginatedResult;
        }),
      );
  }


  // Detail
  getAuditDetail(search: BLSearchAuditReportParam, pagination: Pagination) {
    const paginatedResult: PaginatedResult<BLAuditReportDetailDTO[]> = new PaginatedResult<BLAuditReportDetailDTO[]>();
    const params = new HttpParams()
      .set('factory', search.factory)
      .set('lineID', search.lineID)
      .set('rolloutLineID', search.rolloutLineID)
      .set('lineTypeID', search.lineTypeID)
      .set('modelNo', search.modelNo)
      .set('stageID', search.stageID)
      .set('operationID', search.operationID)
      .set('pageNumber', pagination.currentPage.toString())
      .set('pageSize', pagination.itemsPerPage.toString());
    return this.http.get<PaginatedResult<BLAuditReportDetailDTO[]>>
      (`${this.apiUrl}BLAuditReport/GetAuditDetail`, {observe: "response", params}).pipe(
        map(response => {
          paginatedResult.result = <any>response.body;
          if (response.headers.get("Pagination") != null) {
            paginatedResult.pagination = JSON.parse(response.headers.get("Pagination"));
          }
          return paginatedResult;
        }),
      );
  }

  bestPracticeUrl(search: BLSearchAuditReportParam) {
    const params = new HttpParams()
      .set('factory', search.factory)
      .set('lineID', search.lineID)
      .set('rolloutLineID', search.rolloutLineID)
      .set('lineTypeID', search.lineTypeID)
      .set('modelNo', search.modelNo)
      .set('stageID', search.stageID)
      .set('operationID', search.operationID)
    return this.http.get<any>(`${this.apiUrl}BLAuditReport/BestPracticeUrl`, {params});
  }

  layoutLink(search: BLSearchAuditReportParam) {
    const params = new HttpParams()
      .set('factory', search.factory)
      .set('lineID', search.lineID)
      .set('rolloutLineID', search.rolloutLineID)
      .set('lineTypeID', search.lineTypeID)
      .set('modelNo', search.modelNo)
      .set('stageID', search.stageID)
      .set('operationID', search.operationID)
    return this.http.get<any>(`${this.apiUrl}BLAuditReport/LayoutLink`, {params});
  }
  getLineNo(factory: string) {
    let params = new HttpParams().set('factory', factory);
    return this.http.get<KeyValuePair[]>
      (`${this.apiUrl}BLAuditReport/GetLineNo`, {params});
  }
}
