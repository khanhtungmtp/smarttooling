import { BLRolloutReportModel } from './../../_models/best-line/bl-rollout-report.model';
import { HttpParams } from '@angular/common/http';
import { PaginatedResult, Pagination } from './../../_models/smart-tool/pagination';
import { environment } from './../../../../environments/environment';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BLSearchRolloutReportParam } from '../../_helpers/params/best-line/bl-search-rollout-report.param';
import { KeyValuePair } from '../../_utility/key-value-pair';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class BlRolloutReportService {
  apiUrl = environment.apiUrl;
  constructor(
    private http: HttpClient,
  ) { }

  getAllRollout(searchParam: BLSearchRolloutReportParam, pagination: Pagination) {
    const paginatedResult: PaginatedResult<BLRolloutReportModel[]> = new PaginatedResult<BLRolloutReportModel[]>();
    const params = new HttpParams()
      .set('Factory', searchParam.factory)
      .set('LineID', searchParam.lineID)
      .set('LineTypeID', searchParam.lineTypeID)
      .set('ModelNo', searchParam.modelNo.toUpperCase())
      .set('StageID', searchParam.stageID)
      .set('pageNumber', pagination.currentPage.toString())
      .set('pageSize', pagination.itemsPerPage.toString());

    return this.http.get<PaginatedResult<BLRolloutReportModel[]>>
      (`${this.apiUrl}BLRolloutReport/GetAllRollout`, { observe: "response", params }).pipe(
        map(response => {
          paginatedResult.result = <any>response.body;
          if (response.headers.get("Pagination") != null) {
            paginatedResult.pagination = JSON.parse(response.headers.get("Pagination"));
          }
          return paginatedResult;
        }),
      );
  }

  getFactory() {
    return this.http.get<KeyValuePair[]>
      (`${this.apiUrl}BLRolloutReport/GetFactory`);
  }

  getLineNo(factory: string) {
    let params = new HttpParams().set('factory', factory);
    return this.http.get<KeyValuePair[]>
      (`${this.apiUrl}BLRolloutReport/GetLineNo`, {params});
  }

  getLineType(factory: string) {
    let params = new HttpParams().set('factory', factory);
    return this.http.get<KeyValuePair[]>
      (`${this.apiUrl}BLRolloutReport/GetLineType`, {params});
  }

  getStage(factory: string) {
    let params = new HttpParams().set('factory', factory);
    return this.http.get<KeyValuePair[]>
      (`${this.apiUrl}BLRolloutReport/GetStage`, {params});
  }

}
