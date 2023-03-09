import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { BondingReportParam } from '../../_helpers/params/production-bp/bonding-report-param';
import { Pagination, PaginationResult } from '../../_models/best-line/pagination-best-line';
import { KeyValuePair } from '../../_models/key-value-pair';
import { PBPBondingProgramSetting } from '../../_models/production-bp/pbp-bonding-program-setting';

@Injectable({
  providedIn: 'root'
})
export class BondingReportService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }
  //params search
  paramSearchSource = new BehaviorSubject<BondingReportParam>(null);
  currentParamSearch = this.paramSearchSource.asObservable();
  //page search
  pageSource = new BehaviorSubject<Pagination>(null);
  currentPage = this.pageSource.asObservable();
  //data view
  modelSource = new BehaviorSubject<PBPBondingProgramSetting>(null);
  currentModel = this.modelSource.asObservable();
  
  getAllFactory() {
    return this.http.get<KeyValuePair[]>(this.baseUrl + "BondingReport/GetFactory");
  }
  getProcessType(factory_id: string) {
    let params = new HttpParams().set('factory_id', factory_id);
    return this.http.get<KeyValuePair[]>(this.baseUrl + "BondingReport/GetProcessType", {params});
  }
  getAutoTech(factory_id: string) {
    let params = new HttpParams().set('factory_id', factory_id);
    return this.http.get<KeyValuePair[]>(this.baseUrl + "BondingReport/GetAutoTech", {params});
  }
  getData(searchParam: BondingReportParam, pagination: Pagination) : Observable<PaginationResult<PBPBondingProgramSetting>> {
      let params = new HttpParams()
      .set('factory', searchParam.factory)
      .set('auto_tech_id', searchParam.auto_tech_id)
      .set('chemical_process_type_id', searchParam.chemical_process_type_id)
      .set('model', searchParam.model.toUpperCase())
      .set('prod_season', searchParam.prod_season.toUpperCase())
      .set('pageNumber', pagination.currentPage.toString())
      .set('pageSize', pagination.pageSize.toString());
    return this.http.get<PaginationResult<PBPBondingProgramSetting>>(this.baseUrl + 'BondingReport/Search', { params });
  }
  changeParamSearch(paramSearch: BondingReportParam) {
    this.paramSearchSource.next(paramSearch);
  }
  changePage(pagination: Pagination) {
    this.pageSource.next(pagination);
  }
  changeModel(model: PBPBondingProgramSetting) {
    this.modelSource.next(model);
  }

  exportExcel(searchParam: BondingReportParam, pagination: Pagination): Observable<Blob> {
    let params = new HttpParams()
      .set('factory', searchParam.factory)
      .set('auto_tech_id', searchParam.auto_tech_id)
      .set('chemical_process_type_id', searchParam.chemical_process_type_id)
      .set('model', searchParam.model.toUpperCase())
      .set('prod_season', searchParam.prod_season.toUpperCase())
      .set('pageNumber', pagination.currentPage.toString())
      .set('pageSize', pagination.pageSize.toString());

    return this.http.get(this.baseUrl + "BondingReport/ExportExcel", {
      params,
      responseType: "blob",
    });
  }
}
