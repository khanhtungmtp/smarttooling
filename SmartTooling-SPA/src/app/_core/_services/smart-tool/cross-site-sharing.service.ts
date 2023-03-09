import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable } from "rxjs";
import { map } from "rxjs/operators";
import { environment } from "../../../../environments/environment";
import { Factory } from "../../_models/smart-tool/factory";
import { ModelCrossSiteSharing, ModelCrossSiteSharingEdit } from "../../_models/smart-tool/model-cross-site-sharing";
import { OperationResult } from "../../_models/smart-tool/operation-result";
import { PaginatedResult } from "../../_models/smart-tool/pagination";

@Injectable({
  providedIn: "root"
})
export class CrossSiteSharingService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }
  modelSource = new BehaviorSubject<ModelCrossSiteSharing[]>(null);
  currentModel = this.modelSource.asObservable();
  paramSearchSource = new BehaviorSubject<any>(null);
  getAllFactory(): Observable<Factory[]> {
    return this.http.get<Factory[]>(this.baseUrl + "groupKaizenReport/getAllFactory/", {});
  }
  search(page?, itemsPerPage?, text?: any): Observable<PaginatedResult<ModelCrossSiteSharing[]>> {
    const paginatedResult: PaginatedResult<ModelCrossSiteSharing[]> = new PaginatedResult<ModelCrossSiteSharing[]>();
    let params = new HttpParams();
    if (page != null && itemsPerPage != null) {
      params = params.append("pageNumber", page);
      params = params.append("pageSize", itemsPerPage);
    }
    return this.http.post<any>(this.baseUrl + "CrossSiteSharing/search/", text, { observe: "response", params })
      .pipe(
        map(response => {
          paginatedResult.result = response.body;
          if (response.headers.get("Pagination") != null) {
            paginatedResult.pagination = JSON.parse(response.headers.get("Pagination"));
          }
          return paginatedResult;
        }),
      );
  }
  getCrossSiteSharingEdit(factory: string, modelNO: string, serialNo: string) {
    return this.http.get<ModelCrossSiteSharingEdit>(this.baseUrl + "CrossSiteSharing/getCrossSiteSharingEdit", {params : {factory: factory, modelNo: modelNO, serialNo: serialNo}});
  }
  UpdateCrossSiteSharing(model: ModelCrossSiteSharing) {
    return this.http.post<OperationResult>(this.baseUrl + "CrossSiteSharing/update", model);
  }
  getCrossSiteSharingPDF(model: ModelCrossSiteSharing) {
    return this.http.post<ModelCrossSiteSharingEdit[]>(this.baseUrl + "CrossSiteSharing/getCrossSiteSharingPDF", model);
  }
  changeCrossSiteSharing(models: ModelCrossSiteSharing[]) {
    this.modelSource.next(models);
  }
}

