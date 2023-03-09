import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '../../../../environments/environment';
import { C2BOverallLayoutReport } from '../../_models/best-line/overall-layout-report';
import { C2BOverallLayoutReportDetail } from '../../_models/best-line/overall-layout-report-detail';
import { C2BOverallLayoutReportFiles } from '../../_models/best-line/overall-layout-report-files';
import { C2BOverallLayoutReportParam } from '../../_models/best-line/overall-layout-report-param';
import { KeyValuePair } from '../../_models/key-value-pair';
import { Factory } from '../../_models/smart-tool/factory';
import { PaginatedResult, Pagination } from '../../_models/smart-tool/pagination';

@Injectable({
  providedIn: 'root'
})
export class C2bOverallLayoutReportService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }
  modelSource = new BehaviorSubject<C2BOverallLayoutReport>(null);
  currentModel = this.modelSource.asObservable();
  paramSearchSource = new BehaviorSubject<C2BOverallLayoutReportParam>(null);
  currentParamSearch = this.paramSearchSource.asObservable();

  getAllFactory() {
    return this.http.get<KeyValuePair[]>(this.baseUrl + "C2BOverallLayoutReport/GetFactory");
  }

  search(pagination: Pagination, searchParam: C2BOverallLayoutReportParam) {
    const paginatedResult: PaginatedResult<C2BOverallLayoutReport[]> = new PaginatedResult<C2BOverallLayoutReport[]>();
    let params = new HttpParams()
      .set('factory', searchParam.factory)
      .set('line_no', searchParam.line_no)
      .set('line_type', searchParam.line_type)
      .set('model', searchParam.model.toUpperCase())
      .set('pageNumber', pagination.currentPage.toString())
      .set('pageSize', pagination.itemsPerPage.toString());
    return this.http.get<C2BOverallLayoutReport[]>(this.baseUrl + 'C2BOverallLayoutReport/Search', { observe: 'response', params })
      .pipe(
        map(response => {
          paginatedResult.result = response.body;
          if (response.headers.get('Pagination') != null) {
            paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
          }
          return paginatedResult;
        })
      );
  }

  getLineNo(factory_id: string) {
    let params = new HttpParams().set('factory_id', factory_id);
    return this.http.get<KeyValuePair[]>(this.baseUrl + 'C2BOverallLayoutReport/GetLine', { params });
  }

  getLineType(factory_id: string) {
    let params = new HttpParams().set('factory_id', factory_id);
    return this.http.get<KeyValuePair[]>(this.baseUrl + 'C2BOverallLayoutReport/GetLineType', { params });
  }

  changeModel(model: C2BOverallLayoutReport) {
    this.modelSource.next(model);
  }

  changeParamSearch(paramSearch: C2BOverallLayoutReportParam) {
    this.paramSearchSource.next(paramSearch);
  }

  getDetail(searchParam: C2BOverallLayoutReportParam) {
    let params = new HttpParams()
      .set('factory', searchParam.factory)
      .set('line_no', searchParam.line_no)
      .set('line_type', searchParam.line_type)
      .set('model', searchParam.model.toUpperCase());
    return this.http.get<C2BOverallLayoutReportDetail>(this.baseUrl + 'C2BOverallLayoutReport/GetDetail', { params });
  }

  getFile(pagination: Pagination, searchParam: C2BOverallLayoutReportParam) {
    const paginatedResult: PaginatedResult<C2BOverallLayoutReportFiles[]> = new PaginatedResult<C2BOverallLayoutReportFiles[]>();
    let params = new HttpParams()
      .set('factory', searchParam.factory)
      .set('line_no', searchParam.line_no)
      .set('line_type', searchParam.line_type)
      .set('model', searchParam.model.toUpperCase())
      .set('pageNumber', pagination.currentPage.toString())
      .set('pageSize', pagination.itemsPerPage.toString());
    return this.http.get<C2BOverallLayoutReportFiles[]>(this.baseUrl + 'C2BOverallLayoutReport/GetFiles', { observe: 'response', params })
      .pipe(
        map(response => {
          paginatedResult.result = response.body;
          if (response.headers.get('Pagination') != null) {
            paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
          }
          return paginatedResult;
        })
      );
  }

}
