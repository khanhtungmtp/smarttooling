import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { CriticalProcessParam } from '../../_helpers/params/best-line/critical-process-param';
import { SearchBLCriticalParam } from '../../_helpers/params/best-line/search-critical-Param';
import { BLCriticalProcess } from '../../_models/best-line/bl-critical-process';
import { BLCriticalProcessAnalysis } from '../../_models/best-line/bl-critical-process-analysis';
import { BLLineType } from '../../_models/best-line/bl-line-type';
import { BL_Lines } from '../../_models/best-line/bl-lines';
import { Pagination, PaginationResult } from '../../_models/best-line/pagination-best-line';
import { Stage } from '../../_models/best-line/stage';
import { Model } from '../../_models/smart-tool/model';
import { ModelOperation } from '../../_models/smart-tool/model-operation';
import { OperationResult } from '../../_models/smart-tool/operation-result';

@Injectable({
  providedIn: 'root'
})
export class CriticalProcessAnalysisService {

  idCritical = new BehaviorSubject<any>({
    line_type_name: '',
    line_name: '',
    factory_id: '',
    line_id: '',
    line_type_id: '',
    model_no: '',
    stage_id: '',
    operation_id: '',
    man_remarks: '',
    man_media_url: '',
    machine_remarks: '',
    machine_media_url: '',
    method_remarks: '',
    method_media_url: '',
    material_remarks: '',
    material_media_url: '',
    takt_time: '',
    best_practice_url: '',
    ct_before_sec: '',
    ct_after_sec: '',
    changeLang: '',
    editFlag: '',
    model: '',
    currentPage: 1,
    stageSearch: ''
  });
  currentIDCritical = this.idCritical.asObservable();
  flagSource = new BehaviorSubject<string>('0');
  currentFlag = this.flagSource.asObservable();

  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  // //get list data==================================================
  getAll(searchBLCriticalParam: SearchBLCriticalParam, pagination: Pagination)
    : Observable<PaginationResult<BLCriticalProcess>> {
    let params = new HttpParams();
    params = params.append('pageNumber', pagination.currentPage.toString());
    params = params.append('pageSize', pagination.pageSize.toString());
    return this.http.post<PaginationResult<BLCriticalProcess>>(this.baseUrl + 'BLCriticalProcessAnalysis/GetAll', searchBLCriticalParam, { params });
  }
  getLineNo() {
    return this.http.get<BL_Lines[]>(this.baseUrl + 'BLCriticalProcessAnalysis/GetLineNo', {});
  }
  getStage() {
    return this.http.get<Stage[]>(this.baseUrl + 'BLCriticalProcessAnalysis/GetStage', {});
  }
  getLineType(line_id: string) {
    let params = new HttpParams()
      .set('line_id', line_id)
    return this.http.get<BLLineType[]>(this.baseUrl + 'BLCriticalProcessAnalysis/GetLineType', { params });
  }

  //change critical
  changeIDCritical(params: any) {
    this.idCritical.next(params);
  }

  getModel() {
    return this.http.get<Model[]>(this.baseUrl + 'BLCriticalProcessAnalysis/GetModel', {});
  }

  getData(param: CriticalProcessParam) {
    let params = new HttpParams()
      .set('factory_id', param.factory_id)
      .set('line_id', param.line_id)
      .set('line_type_id', param.line_type_id)
      .set('model_no', param.model_no)
      .set('stage_id', param.stage_id)
      .set('operation_id', param.operation_id)

    return this.http.get<BLCriticalProcessAnalysis>(this.baseUrl + 'BLCriticalProcessAnalysis/GetData', { params });
  }

  getOperationName(model_no: string, stage: string) {
    let params = new HttpParams()
      .set('model_no', model_no)
      .set('stage', stage)
    return this.http.get<ModelOperation[]>(this.baseUrl + 'BLCriticalProcessAnalysis/GetOperationName', { params });
  }
  add(criticalProcessData: BLCriticalProcessAnalysis) {
    return this.http.post<OperationResult>(this.baseUrl + 'BLCriticalProcessAnalysis/Create', criticalProcessData);
  }
  upDate(model: BLCriticalProcessAnalysis) {
    return this.http.put<OperationResult>(this.baseUrl + 'BLCriticalProcessAnalysis/update', model);
  }

  changeFlag(flag: string) {
    this.flagSource.next(flag);
  }
}
