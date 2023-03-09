import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { BLLineType } from '../../_models/best-line/bl-line-type';
import { BL_Lines } from '../../_models/best-line/bl-lines';
import { BL_Rollout_Progress } from '../../_models/best-line/bl-rollout-progress';
import { BL_RolloutHourlyPPH } from '../../_models/best-line/bl-rollouthourlypph';
import { ModelOperationRollout } from '../../_models/best-line/model-operation-rollout';
import { ModelRollout } from '../../_models/best-line/model-rollout';
import { PaginationResult } from '../../_models/best-line/pagination-best-line';
import { Params_Rollout_Progress } from '../../_models/best-line/params-rollout-progress';
import { StageRollout } from '../../_models/best-line/stage-rollout';
import { OperationResult } from '../../_models/smart-tool/operation-result';

@Injectable({
  providedIn: 'root'
})
export class RolloutProgressService {

  baseUrl = environment.apiUrl;
  //change lineNo and lineType when edit and add
  idRollout = new BehaviorSubject<any>({
    //id
    line_id: '',
    line_type_id: '',
    model_no: '',
    stage_id: '',
    operation_id: '',
    rollout_line_id: '',
    hour: null,
    //model
    model: '',
    editFlag: '',
    currentPage: null,
    //name
    lineNoName: '',
    lineTypeName: '',
    rolloutLineName: '',
    changeLang: '',
  });
  currentIDRollout = this.idRollout.asObservable();

  constructor(private http: HttpClient) { }

  //get list data==================================================
  getAll(pageNumber: number = 1, lineNo: string, lineType: string, modelNo: string)
    : Observable<PaginationResult<BL_Rollout_Progress>> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('lineNo', lineNo)
      .set('lineType', lineType)
      .set('model', modelNo);
    return this.http.get<PaginationResult<BL_Rollout_Progress>>(this.baseUrl + 'RolloutProgress/search', { params });
  }
  getLineNo() {
    return this.http.get<BL_Lines[]>(this.baseUrl + 'RolloutProgress/getlineno', {});
  }
  getLineType(lineType: string) {
    let params = new HttpParams()
      .set('lineType', lineType)
    return this.http.get<BLLineType[]>(this.baseUrl + 'RolloutProgress/getlinetype', { params });
  }
  //add and edit==================================================
  changeIDRollout(params: any) {
    this.idRollout.next(params);
  }
  getModelNo(param: Params_Rollout_Progress) {
    let params = new HttpParams()
      .set('line_id', param.line_id)
      .set('line_type_id', param.line_type_id)
    return this.http.get<ModelRollout[]>(this.baseUrl + 'RolloutProgress/getmodelno', { params });
  }
  getStage(param: Params_Rollout_Progress) {
    let params = new HttpParams()
      .set('line_id', param.line_id)
      .set('line_type_id', param.line_type_id)
      .set('model_no', param.model_no)
    return this.http.get<StageRollout[]>(this.baseUrl + 'RolloutProgress/getstage', { params });
  }
  getOperaName(param: Params_Rollout_Progress) {
    let params = new HttpParams()
      .set('line_id', param.line_id)
      .set('line_type_id', param.line_type_id)
      .set('model_no', param.model_no)
      .set('stage_id', param.stage_id)
    return this.http.get<ModelOperationRollout[]>(this.baseUrl + 'RolloutProgress/getoperaname', { params });
  }
  getRolloutLine() {
    return this.http.get<BL_Lines[]>(this.baseUrl + 'RolloutProgress/getrolloutline', {});
  }
  getCTAfter(model: Params_Rollout_Progress) {
    return this.http.post<number>(this.baseUrl + 'RolloutProgress/getctafter', model, {});
  }
  getHourlyPPH(model: Params_Rollout_Progress) {
    return this.http.post<BL_RolloutHourlyPPH>(this.baseUrl + 'RolloutProgress/gethourlypph', model, {});
  }
  getBLRollout(model: Params_Rollout_Progress) {
    return this.http.post<BL_Rollout_Progress>(this.baseUrl + 'RolloutProgress/getblrollout', model, {});
  }
  addNew(model: BL_Rollout_Progress) {
    return this.http.post<OperationResult>(this.baseUrl + 'RolloutProgress/addnew', model);
  }
  upDate(model: BL_Rollout_Progress) {
    return this.http.put<OperationResult>(this.baseUrl + 'RolloutProgress/update', model, {});
  }
}
