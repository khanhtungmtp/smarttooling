import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { BLLineType } from '../../_models/best-line/bl-line-type';
import { BL_Lines } from '../../_models/best-line/bl-lines';
import { BL_Rollout_Audit } from '../../_models/best-line/bl-rollout-audit';
import { BL_Critical_Process_Analysis } from '../../_models/best-line/bl_critical_process_analysis_audit';
import { ModelOperationRollout } from '../../_models/best-line/model-operation-rollout';
import { ModelRollout } from '../../_models/best-line/model-rollout';
import { PaginationResult } from '../../_models/best-line/pagination-best-line';
import { Params_Rollout_Progress } from '../../_models/best-line/params-rollout-progress';
import { StageRollout } from '../../_models/best-line/stage-rollout';
import { OperationResult } from '../../_models/smart-tool/operation-result';

@Injectable({
  providedIn: 'root'
})
export class RolloutAuditService {

  baseUrl = environment.apiUrl;

  //change rollout_line_no and lineType when view and add
  idRollout = new BehaviorSubject<any>({
    //id
    rollout_line_id: '',
    line_type_id: '',
    changeLang: '',
    model_no: '',
    stage_id: '',
    operation_id: '',
    audit_count: 0,
    //model
    model: '',
    editFlag: '',
    currentPage: null,
    //text
    line_id: '',
    line_type_name: '',
  });
  currentIDRollout = this.idRollout.asObservable();

  constructor(private http: HttpClient) { }
  //get list data==================================================
  getAll(pageNumber: number = 1, rollout_line_id: string, line_type_id: string, text: string)
    : Observable<PaginationResult<BL_Rollout_Audit>> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('rollout_line_id', rollout_line_id)
      .set('line_type_id', line_type_id)
      .set('text', text);
    return this.http.get<PaginationResult<BL_Rollout_Audit>>(this.baseUrl + 'RolloutAudit/search', { params });
  }
  getRolloutLineNo() {
    return this.http.get<BL_Lines[]>(this.baseUrl + 'RolloutAudit/getrolloutlineno', {});
  }
  getLineType(line_type_id: string) {
    let params = new HttpParams()
      .set('line_type_id', line_type_id);
    return this.http.get<BLLineType[]>(this.baseUrl + 'RolloutAudit/getlinetype', { params });
  }
  //add and==================================================
  changeIDRollout(params: any) {
    this.idRollout.next(params);
  }
  getModelNo(param: Params_Rollout_Progress) {
    let params = new HttpParams()
      .set('rollout_line_id', param.rollout_line_id)
      .set('line_type_id', param.line_type_id)
    return this.http.get<ModelRollout[]>(this.baseUrl + 'RolloutAudit/getmodelno', { params });
  }
  getStage(param: Params_Rollout_Progress) {
    let params = new HttpParams()
      .set('rollout_line_id', param.rollout_line_id)
      .set('line_type_id', param.line_type_id)
      .set('model_no', param.model_no)
    return this.http.get<StageRollout[]>(this.baseUrl + 'RolloutAudit/getstage', { params });
  }
  getOperaName(param: Params_Rollout_Progress) {
    let params = new HttpParams()
      .set('rollout_line_id', param.rollout_line_id)
      .set('line_type_id', param.line_type_id)
      .set('model_no', param.model_no)
      .set('stage_id', param.stage_id)
    return this.http.get<ModelOperationRollout[]>(this.baseUrl + 'RolloutAudit/getoperaname', { params });
  }
  getAuditCount(model: Params_Rollout_Progress) {
    return this.http.post<number>(this.baseUrl + 'RolloutAudit/auditcount', model, {});
  }
  getRemarks(model: Params_Rollout_Progress) {
    return this.http.post<BL_Critical_Process_Analysis>(this.baseUrl + 'RolloutAudit/getremarks', model, {});
  }
  addNew(model: BL_Rollout_Audit) {
    return this.http.post<OperationResult>(this.baseUrl + 'RolloutAudit/add', model);
  }
  getAudit(model: Params_Rollout_Progress, audit_count: number) {
    let params = new HttpParams()
      .set('audit_count', audit_count.toString())
    return this.http.post<BL_Rollout_Audit>(this.baseUrl + 'RolloutAudit/getaudit', model, { params });
  }
}
