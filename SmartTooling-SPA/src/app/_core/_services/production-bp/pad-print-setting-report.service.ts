import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { PBPPadPrintSettingParams } from '../../_helpers/params/production-bp/pbp-pad-print-setting-params';
import { Pagination, PaginationResult } from '../../_models/production-bp/pagination-production-bp';
import { PbpPadPrintSetting } from '../../_models/production-bp/pbp-pad-print-setting';
import { KeyValuePair } from '../../_utility/key-value-pair';

@Injectable({
  providedIn: 'root'
})
export class PadPrintSettingReportService {
  baseUrl = environment.apiUrl;
  paramSearch = new BehaviorSubject<PBPPadPrintSettingParams>(null);
  currentParamSearch = this.paramSearch.asObservable();
  pageSource = new BehaviorSubject<Pagination>(null);
  currentPage = this.pageSource.asObservable();

  pBPPadPrintSetting = new BehaviorSubject<PbpPadPrintSetting>(null);
  currentPBPPadPrintSetting = this.pBPPadPrintSetting.asObservable();

  constructor(private http: HttpClient) { }

  getAllFactory() {
    return this.http.get<KeyValuePair[]>(this.baseUrl + "PBPPadPrintSettingReport/GetFactory");
  }

  search(pagination: Pagination, searchParam: PBPPadPrintSettingParams): Observable<PaginationResult<PbpPadPrintSetting>> {
    let params = new HttpParams()
      .set('pageNumber', pagination.currentPage.toString())
      .set('pageSize', pagination.pageSize.toString());
    return this.http.post<PaginationResult<PbpPadPrintSetting>>(this.baseUrl + "PBPPadPrintSettingReport/Search", searchParam, { params });
  }

  changePBPPadPrintSetting(pBPPadPrintSetting: PbpPadPrintSetting) {
    this.pBPPadPrintSetting.next(pBPPadPrintSetting);
  }
  changeParamSearch(paramSearch: PBPPadPrintSettingParams) {
    this.paramSearch.next(paramSearch);
  }
  changePage(pagination: Pagination) {
    this.pageSource.next(pagination);
  }

  exportExcel(pagination: Pagination, searchParam: PBPPadPrintSettingParams): Observable<Blob> {
    let params = new HttpParams()
      .set('factory_id', searchParam.factory_id)
      .set('model', searchParam.model)
      .set('dev_season', searchParam.dev_season)
      .set('production_season', searchParam.production_season)
      .set('pageNumber', pagination.currentPage.toString())
      .set('pageSize', pagination.pageSize.toString());

    return this.http.get(this.baseUrl + "PBPPadPrintSettingReport/ExportExcel", { params, responseType: "blob", });
  }

}
