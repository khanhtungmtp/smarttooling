import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable } from "rxjs";
import { map } from "rxjs/operators";
import { environment } from "../../../../environments/environment";
import { Efficiency } from "../../_models/smart-tool/efficiency";
import { ModelKaizenReport } from "../../_models/smart-tool/model-kaizen-report";
import { PaginatedResult } from "../../_models/smart-tool/pagination";

@Injectable({
  providedIn: "root"
})
export class KaizenReportService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }
  paramSearchSource = new BehaviorSubject<any>(null);
  modelSource = new BehaviorSubject<ModelKaizenReport>(null);
  currentModel = this.modelSource.asObservable();
  kaizenSource = new BehaviorSubject<object>(null);
  currentKaizen = this.kaizenSource.asObservable();
  search(page?, itemsPerPage?, text?: any): Observable<PaginatedResult<ModelKaizenReport[]>> {
    const paginatedResult: PaginatedResult<ModelKaizenReport[]> = new PaginatedResult<ModelKaizenReport[]>();
    let params = new HttpParams();
    if (page != null && itemsPerPage != null) {
      params = params.append("pageNumber", page);
      params = params.append("pageSize", itemsPerPage);
    }
    return this.http.post<any>(this.baseUrl + "kaizenReport/search/", text, { observe: "response", params })
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
  exportExcel(param: any) {
    return this.http.post(this.baseUrl + "kaizenreport/exportExcel", param, {responseType: "blob" })
      .subscribe((result: Blob) => {
        if (result.type !== "application/xlsx") {
          alert(result.type);
        }
        const blob = new Blob([result]);
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement("a");
        const currentTime = new Date();
        const filename = "Excel_"+ "kaizenReport" + currentTime.getFullYear().toString() +
          (currentTime.getMonth() + 1) + currentTime.getDate() +
          currentTime.toLocaleTimeString().replace(/[ ]|[,]|[:]/g, "").trim() + ".xlsx";
        link.href = url;
        link.setAttribute("download", filename);
        document.body.appendChild(link);
        link.click();
      });
  }
  changeModel(model: ModelKaizenReport) {
    this.modelSource.next(model);
  }
  changeKaizen(model: object) {
    this.kaizenSource.next(model);
  }
  getSeasonByUpper(upperId: string): Observable<string[]> {
    return this.http.get<string[]>(this.baseUrl + "kaizenreport/getSeason/" + upperId, {});
  }
  getDataChart(upper_id: string, season: string): Observable<Efficiency[]> {
    return this.http.get<Efficiency[]>(this.baseUrl + "kaizenreport/getEfficiencys",
                                      {params: {upper_id: upper_id, season: season}});
  }

  getKaizens(page?, itemsPerPage?, text?: any): Observable<PaginatedResult<any[]>> {
    const paginatedResult: PaginatedResult<any[]> = new PaginatedResult<any[]>();
    let params = new HttpParams();
    if (page != null && itemsPerPage != null) {
      params = params.append("pageNumber", page);
      params = params.append("pageSize", itemsPerPage);
    }
    return this.http.get<any>(this.baseUrl + "kaizenReport/getKaizens/" + text, { observe: "response", params })
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

  updateClickTimes(data: any) {
    return this.http.post(this.baseUrl + "kaizenReport/updateClickTimes", data, {} );
  }

  getKaizenDetail(model_no: string, serial_no: string): Observable<any> {
    return this.http.get<any>(this.baseUrl + "kaizenReport/getKaizenDetail",
                                  {params: {model_no: model_no, serial_no: serial_no}});
  }
}
