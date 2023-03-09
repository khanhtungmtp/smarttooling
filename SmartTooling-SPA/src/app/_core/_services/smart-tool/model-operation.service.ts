import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { ModelOperationParam } from '../../_helpers/params/smart-tool/model-operation-param';
import { Pagination, PaginationResult } from '../../_models/best-line/pagination-best-line';
import { Model } from '../../_models/smart-tool/model';
import { ModelOperation } from '../../_models/smart-tool/model-operation';
import { ModelOperationEditParam } from '../../_models/smart-tool/model-operationEditParam';
import { OperationResult } from '../../_models/smart-tool/operation-result';

@Injectable({
  providedIn: 'root'
})
export class ModelOperationService {
  baseUrl = environment.apiUrl;

  paramSearchSource = new BehaviorSubject<ModelOperationParam>(<ModelOperationParam>{});
  constructor(private http: HttpClient) { }

  search(modelParam: ModelOperationParam, pagination: Pagination): Observable<PaginationResult<ModelOperation>> {
    let params: HttpParams = new HttpParams().appendAll({ 'PageNumber': pagination.currentPage, 'pageSize': pagination.pageSize });
    let url = this.baseUrl + 'modelOperation/All';
    return this.http.post<PaginationResult<ModelOperation>>(url, modelParam, { params });
  }

  getModelNo(): Observable<Model[]> {
    return this.http.get<Model[]>(this.baseUrl + 'modelOperation/model-no');
  }

  getStage() {
    return this.http.get<any>(this.baseUrl + 'modelOperation/stage', {});
  }

  getProcessType() {
    return this.http.get<any>(this.baseUrl + 'modelOperation/process-type', {});
  }

  getModelOperationEdit(modelOperation: ModelOperationEditParam) {
    return this.http.post<ModelOperation>(this.baseUrl + 'modelOperation/getModelOperation/', modelOperation);
  }

  createModelOperation(modelOperation: ModelOperation) {
    return this.http.post(this.baseUrl + 'modelOperation/create-operation/', modelOperation);
  }

  updateModelOperation(modelOperation: ModelOperation) {
    return this.http.post(this.baseUrl + "modelOperation/updateModelOperation/", modelOperation);
  }

  deleteModelOperation(modelOperation: ModelOperation) {
    return this.http.post(this.baseUrl + "modelOperation/deleteModelOperation/", modelOperation);
  }

  exportExcel(modelOperation: ModelOperationParam): Observable<Blob> {
    return this.http.post(this.baseUrl + "modelOperation/ExportExcel", modelOperation, { responseType: 'blob' });
  }

  uploadExcel(file: File) {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post<OperationResult>(this.baseUrl + 'modelOperation/UploadExcel', formData);
  }
}
