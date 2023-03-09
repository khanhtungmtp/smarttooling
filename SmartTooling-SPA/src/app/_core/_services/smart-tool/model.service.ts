import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '../../../../environments/environment';
import { Model } from '../../_models/smart-tool/model';
import { ModelParam } from '../../_models/smart-tool/modelParam';
import { OperationResult } from '../../_models/smart-tool/operation-result';
import { PaginatedResult, Pagination } from '../../_models/smart-tool/pagination';

@Injectable({
  providedIn: 'root'
})
export class ModelService {
  baseUrl = environment.apiUrl;
  modelSearchSource = new BehaviorSubject<any>(null);
  currentModelSearch = this.modelSearchSource.asObservable();
  constructor(private http: HttpClient) { }

  search(page?, itemsPerPage?, modelParam?: object): Observable<PaginatedResult<Model[]>> {
    const paginatedResult: PaginatedResult<Model[]> = new PaginatedResult<Model[]>();
    let params = new HttpParams();

    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }
    let url = this.baseUrl + 'model/model-list';
    return this.http.post<any>(url, modelParam, { observe: 'response', params })
      .pipe(
        map(response => {
          paginatedResult.result = response.body;
          if (response.headers.get('Pagination') != null) {
            paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
          }
          return paginatedResult;
        }),
      );
  }
  changeParamModelSearch(param: any) {
    this.modelSearchSource.next(param);
  }
  getAllModelType() {
    return this.http.get<any>(this.baseUrl + 'model/model-type', {});
  }

  createModel(model: Model) {
    console.log("Service: ", model);
    return this.http.post(this.baseUrl + 'model/createModel/', model);
  }

  updateModel(model: Model) {
    return this.http.post(this.baseUrl + "model/updateModel/", model);
  }


  getModelNoEdit(modelNo: string) {
    return this.http.get<Model>(this.baseUrl + 'model/edit/' + modelNo);
  }

  exportExcel(pagination: Pagination, modelParam?: ModelParam): Observable<Blob> {
    let params = new HttpParams()
      .set("model_search", modelParam.model_search.toString())
      .set("active", modelParam.active)
      .set("pageNumber", pagination.currentPage.toString())
      .set("pageSize", pagination.totalPages.toString());

    return this.http.get(`${this.baseUrl}Model/ExportExcel`, { params, responseType: "blob", });
  }

  uploadExcel(file: File): Observable<OperationResult> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post<OperationResult>(`${this.baseUrl}Model/UploadExcel`, formData)
  }
}
