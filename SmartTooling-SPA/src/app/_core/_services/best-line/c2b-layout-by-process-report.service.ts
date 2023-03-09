import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '../../../../environments/environment';
import { BL_Layout_Design_ProcessDTO } from '../../_models/best-line/bl-layout-design-by-process-dto';
import { C2BLayoutByProcessDTO } from '../../_models/best-line/layout-by-process-dto';
import { C2BLayoutByProcessDetailDTO } from '../../_models/best-line/layout-by-process-report-detail';
import { C2BLayoutByProcessReportParam } from '../../_models/best-line/layout-by-process-report-param';
import { KeyValuePair } from '../../_models/key-value-pair';
import { PaginatedResult, Pagination } from '../../_models/smart-tool/pagination';

@Injectable({
  providedIn: 'root'
})
export class C2bLayoutByProcessReportService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }
  modelSource = new BehaviorSubject<C2BLayoutByProcessDTO>(null);
  currentModel = this.modelSource.asObservable();
  paramSearchSource = new BehaviorSubject<C2BLayoutByProcessReportParam>(null);
  currentParamSearch = this.paramSearchSource.asObservable();

  getAllFactory() {
    return this.http.get<KeyValuePair[]>(this.baseUrl + "C2BLayoutByProcessReport/GetFactory");
  }
  getLineNo(factory_id: string) {
    let params = new HttpParams().set('factory_id', factory_id);
    return this.http.get<KeyValuePair[]>(this.baseUrl + 'C2BLayoutByProcessReport/GetLine', { params });
  }

  getLineType(factory_id: string) {
    let params = new HttpParams().set('factory_id', factory_id);
    return this.http.get<KeyValuePair[]>(this.baseUrl + 'C2BLayoutByProcessReport/GetLineType', { params });
  }

  getProcessType(factory_id: string) {
    let params = new HttpParams().set('factory_id', factory_id);
    return this.http.get<KeyValuePair[]>(this.baseUrl + 'C2BLayoutByProcessReport/GetProcessType', { params });
  }
  changeModel(model: C2BLayoutByProcessDTO) {
    this.modelSource.next(model);
  }

  changeParamSearch(paramSearch: C2BLayoutByProcessReportParam) {
    this.paramSearchSource.next(paramSearch);
  }

  search(pagination: Pagination, searchParam: C2BLayoutByProcessReportParam) {
    const paginatedResult: PaginatedResult<C2BLayoutByProcessDTO[]> = new PaginatedResult<C2BLayoutByProcessDTO[]>();
    let params = new HttpParams()
      .set('factory', searchParam.factory)
      .set('line_no', searchParam.line_no)
      .set('line_type', searchParam.line_type)
      .set('model', searchParam.model.toUpperCase())
      .set('process_no', searchParam.process_no)
      .set('pageNumber', pagination.currentPage.toString())
      .set('pageSize', pagination.itemsPerPage.toString());
    return this.http.get<C2BLayoutByProcessDTO[]>(this.baseUrl + 'C2BLayoutByProcessReport/Search', { observe: 'response', params })
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

  GetLayoutDesignProcess(searchParam: C2BLayoutByProcessReportParam) {
    let params = new HttpParams()
      .set('factory', searchParam.factory)
      .set('line_no', searchParam.line_no)
      .set('line_type', searchParam.line_type)
      .set('process_no', searchParam.process_no);
    return this.http.get<BL_Layout_Design_ProcessDTO>(this.baseUrl + 'C2BLayoutByProcessReport/GetLayoutDesignProcess', { params });
  }

  GetLayoutDesignProcessData(searchParam: C2BLayoutByProcessReportParam) {
    let params = new HttpParams()
      .set('factory', searchParam.factory)
      .set('line_no', searchParam.line_no)
      .set('line_type', searchParam.line_type)
      .set('process_no', searchParam.process_no);
    return this.http.get<C2BLayoutByProcessDetailDTO>(`${this.baseUrl}C2BLayoutByProcessReport/GetGraphData`, { params });
  }
}
