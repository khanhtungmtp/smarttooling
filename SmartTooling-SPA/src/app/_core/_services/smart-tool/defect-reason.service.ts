import { HttpHeaders, HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable } from "rxjs";
import { map } from "rxjs/operators";
import { environment } from "../../../../environments/environment";
import { DefectReason } from "../../_models/smart-tool/defect-reason";
import { PaginatedResult } from "../../_models/smart-tool/pagination";

// const httpOptions = {
//   headers: new HttpHeaders({
//     Authorization: "Bearer " + localStorage.getItem("token"),
//   }),
// };

@Injectable({
  providedIn: "root",
})
export class DefectReasonService {
  baseUrl = environment.apiUrl;
  defectreasonSource = new BehaviorSubject<Object>({});
  currentdefectreason = this.defectreasonSource.asObservable();
  flagSource = new BehaviorSubject<string>("0");
  currentFlag = this.flagSource.asObservable();
  defectReasonSearchSource = new BehaviorSubject<object>(null);
  currentDefectReasonSearch = this.defectReasonSearchSource.asObservable();
  constructor(private http: HttpClient) {}

  search(
    page?,
    itemsPerPage?,
    defectreasonParam?: object
  ): Observable<PaginatedResult<DefectReason[]>> {
    const paginatedResult: PaginatedResult<
      DefectReason[]
    > = new PaginatedResult<DefectReason[]>();
    let params = new HttpParams();
    if (page != null && itemsPerPage != null) {
      params = params.append("pageNumber", page);
      params = params.append("pageSize", itemsPerPage);
    }
    let url = this.baseUrl + "defectreason/search";
    return this.http
      .post<any>(url, defectreasonParam, { observe: "response", params })
      .pipe(
        map((response) => {
          paginatedResult.result = response.body;
          if (response.headers.get("Pagination") != null) {
            paginatedResult.pagination = JSON.parse(
              response.headers.get("Pagination")
            );
          }
          return paginatedResult;
        })
      );
  }
  changeDefectReasonSearch(param: object) {
    this.defectReasonSearchSource.next(param);
  }
  // call API
  createDefectReason(defectreason: DefectReason) {
    // console.log(httpOptions);
    // console.log("data: ", defectreason);
    return this.http.post(this.baseUrl + "defectreason/create/", defectreason);
  }

  updateDefectReason(defectreason: DefectReason) {
    // console.log("data: ", defectreason);
    return this.http.post(this.baseUrl + "defectreason/edit/", defectreason);
  }

  // getAllDefectReason() {
  //   return this.http.get<DefectReason[]>(this.baseUrl + "defectreason/all", {});
  // }

  changeDefectReason(defectraeson: DefectReason) {
    this.defectreasonSource.next(defectraeson);
  }

  changeFlag(flag: string) {
    this.flagSource.next(flag);
  }
}
