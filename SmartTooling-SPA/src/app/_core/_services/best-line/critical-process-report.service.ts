import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '../../../../environments/environment';
import { CriticalProcessReportDetailDTO } from '../../_models/best-line/critical-process-report-detail';
import { CriticalProcessReportDTO } from '../../_models/best-line/critical-process-report-dto';
import { CriticalProcessReportKaizenDTO } from '../../_models/best-line/critical-process-report-kaizen';
import { CriticalProcessReportParam } from '../../_models/best-line/critical-process-report-param';
import { KeyValuePair } from '../../_models/key-value-pair';
import { PaginatedResult, Pagination } from '../../_models/smart-tool/pagination';

@Injectable({
  providedIn: 'root'
})
export class CriticalProcessReportService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }
  modelSource = new BehaviorSubject<CriticalProcessReportParam>(null);
  currentModel = this.modelSource.asObservable();
  paramSearchSource = new BehaviorSubject<CriticalProcessReportParam>(null);
  currentParamSearch = this.paramSearchSource.asObservable();
  kaizenSource = new BehaviorSubject<CriticalProcessReportKaizenDTO>(null);
  currentKaizen = this.kaizenSource.asObservable();
  getAllFactory() {
    return this.http.get<KeyValuePair[]>(this.baseUrl + "CriticalProcessReport/GetFactory");
  }

  getLineNo(factory_id: string) {
    let params = new HttpParams().set('factory_id', factory_id);
    return this.http.get<KeyValuePair[]>(this.baseUrl + 'CriticalProcessReport/GetLine', { params });
  }

  getLineType(factory_id: string) {
    let params = new HttpParams().set('factory_id', factory_id);
    return this.http.get<KeyValuePair[]>(this.baseUrl + 'CriticalProcessReport/GetLineType', { params });
  }

  getStage(factory_id: string) {
    let params = new HttpParams().set('factory_id', factory_id);
    return this.http.get<KeyValuePair[]>(this.baseUrl + 'CriticalProcessReport/GetStage', { params });
  }
  changeModel(model: CriticalProcessReportParam) {
    this.modelSource.next(model);
  }

  changeParamSearch(paramSearch: CriticalProcessReportParam) {
    this.paramSearchSource.next(paramSearch);
  }

  changeKaizen(kaizen: CriticalProcessReportKaizenDTO) {
    this.kaizenSource.next(kaizen);
  }

  search(pagination: Pagination, searchParam: CriticalProcessReportParam) {
    const paginatedResult: PaginatedResult<CriticalProcessReportDTO[]> = new PaginatedResult<CriticalProcessReportDTO[]>();
    let params = new HttpParams()
      .set('factory_id', searchParam.factory_id)
      .set('line_no', searchParam.line_no)
      .set('line_type', searchParam.line_type)
      .set('model_no', searchParam.model_no.toUpperCase())
      .set('stage', searchParam.stage)
      .set('pageNumber', pagination.currentPage.toString())
      .set('pageSize', pagination.itemsPerPage.toString());
    return this.http.get<CriticalProcessReportDTO[]>(this.baseUrl + 'CriticalProcessReport/Search', { observe: 'response', params })
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

  getDetail(searchParam: CriticalProcessReportParam) {
    return this.http.post<CriticalProcessReportDetailDTO>(this.baseUrl + "CriticalProcessReport/GetDetail", searchParam );
  }

  getKaizen(pagination: Pagination, searchParam: CriticalProcessReportParam) {
    const paginatedResult: PaginatedResult<CriticalProcessReportKaizenDTO[]> = new PaginatedResult<CriticalProcessReportKaizenDTO[]>();
    let params = new HttpParams()
      .set('pageNumber', pagination.currentPage.toString())
      .set('pageSize', pagination.itemsPerPage.toString());

    return this.http.post<CriticalProcessReportKaizenDTO[]>(this.baseUrl + "CriticalProcessReport/GetKaizen", searchParam , { observe: 'response', params })
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


  getKaizenDetail(factory_id: string, model_no: string, serial_no: string) {
    return this.http.get<any>(this.baseUrl + "CriticalProcessReport/getKaizenDetail",
      { params: { factory_id: factory_id, model_no: model_no, serial_no: serial_no } });
  }


}
