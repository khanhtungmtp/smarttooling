import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable } from "rxjs";
import { environment } from "../../../../environments/environment";
import { ComputerReportParam } from "../../_helpers/params/production-bp/computer-report-param";
import {
  Pagination,
  PaginationResult,
} from "../../_models/best-line/pagination-best-line";
import { KeyValuePair } from "../../_models/key-value-pair";
import { PbpComputerstitchingsetting } from "../../_models/production-bp/pbp-computerstitchingsetting";

@Injectable({
  providedIn: "root",
})
export class ComputerReportService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) {}
  //params search
  paramSearchSource = new BehaviorSubject<ComputerReportParam>(null);
  currentParamSearch = this.paramSearchSource.asObservable();
  //page search
  pageSource = new BehaviorSubject<Pagination>(null);
  currentPage = this.pageSource.asObservable();
  //data detail
  modelSource = new BehaviorSubject<PbpComputerstitchingsetting>(null);
  currentModel = this.modelSource.asObservable();
  getAllFactory() {
    return this.http.get<KeyValuePair[]>(
      this.baseUrl + "ComputerReport/GetFactory"
    );
  }
  getData(
    searchParam: ComputerReportParam,
    pagination: Pagination
  ): Observable<PaginationResult<PbpComputerstitchingsetting>> {
    let params = new HttpParams()
      .set("factory", searchParam.factory)
      .set("model", searchParam.model.toUpperCase())
      .set("production_adoption", searchParam.production_adoption)
      .set("dev_season", searchParam.dev_season.toUpperCase())
      .set("prod_season", searchParam.prod_season.toUpperCase())
      .set("pageNumber", pagination.currentPage.toString())
      .set("pageSize", pagination.pageSize.toString());
    return this.http.get<PaginationResult<PbpComputerstitchingsetting>>(
      this.baseUrl + "ComputerReport/Search",
      { params }
    );
  }
  changeParamSearch(paramSearch: ComputerReportParam) {
    this.paramSearchSource.next(paramSearch);
  }
  changePage(pagination: Pagination) {
    this.pageSource.next(pagination);
  }
  changeModel(model: PbpComputerstitchingsetting) {
    this.modelSource.next(model);
  }

  exportExcel(
    pagination: Pagination,
    searchParam: ComputerReportParam
  ){
    let params = new HttpParams()
      .set("factory", searchParam.factory)
      .set("model", searchParam.model.toUpperCase())
      .set("production_adoption", searchParam.production_adoption)
      .set("dev_season", searchParam.dev_season.toUpperCase())
      .set("prod_season", searchParam.prod_season.toUpperCase())
      .set("pageNumber", pagination.currentPage.toString())
      .set("pageSize", pagination.pageSize.toString());
    return this.http.get(this.baseUrl + "ComputerReport/ExportExcel", {
      params,
      responseType: "blob",
    });
  }
}
