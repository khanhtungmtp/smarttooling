import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable } from "rxjs";
import { map } from "rxjs/operators";
import { environment } from "../../../../environments/environment";
import { MeasumentRFT } from "../../_models/smart-tool/measumentRFT";
import { PaginatedResult } from "../../_models/smart-tool/pagination";

@Injectable({
  providedIn: "root",
})
export class RftService {
  baseUrl = environment.apiUrl;
  rftSource = new BehaviorSubject<Object>({}); // record current data
  currentRFT = this.rftSource.asObservable();
  rftconditionSource = new BehaviorSubject<Object>({});
  paramSearchSource = new BehaviorSubject<any>(null);
  currentCondition = this.rftconditionSource.asObservable();

  constructor(private http: HttpClient) {}

  search(page?, modelNO?, stage?): Observable<PaginatedResult<MeasumentRFT[]>> {
    const paginatedResult: PaginatedResult<
      MeasumentRFT[]
    > = new PaginatedResult<MeasumentRFT[]>();
    let params = new HttpParams();

    params = params.append("pageNumber", page);
    params = params.append("pageSize", "10");
    params = params.append("modelNo", modelNO == "All" ? "" : modelNO);
    params = params.append("stage", stage == "All" ? "" : stage);

    return this.http
      .get<MeasumentRFT[]>(this.baseUrl + "RFT", {
        observe: "response",
        params,
      })
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

  // call API
  createMeasurementRFT(measurementrft: MeasumentRFT) {
    // console.log("data: ", measurementrft);
    return this.http.post(this.baseUrl + "RFT/create", measurementrft);
  }

  updateMeasurementRFT(measurementrft: MeasumentRFT) {
    // console.log("data: ", measurementrft);
    return this.http.post(this.baseUrl + "RFT/edit", measurementrft);
  }

  geDataModelNo = () => this.http.get<any>(this.baseUrl + "RFT/getallmodel");

  geDataStage = () => this.http.get<any>(this.baseUrl + "RFT/getallstage");

  getProcessType(model_no: string, stage_id: string): Observable<any> {
    return this.http.get<any>(this.baseUrl + "RFT/process-type", {
      params: {
        model_no: model_no,
        stage_id: stage_id,
      },
    });
  }

  getProcessNOperationForEdit(
    model_no: string,
    stage_id: string,
    operation_id: string
  ): Observable<any> {
    return this.http.get<any>(this.baseUrl + "RFT/processnoperationforedit", {
      params: {
        model_no: model_no,
        stage_id: stage_id,
        operation_id: operation_id,
      },
    });
  }

  getOperationName(
    model_no: string,
    stage_id: string,
    process_id: string
  ): Observable<any> {
    return this.http.get<any>(this.baseUrl + "RFT/getoptionname", {
      params: {
        model_no: model_no,
        stage_id: stage_id,
        process_id: process_id,
      },
    });
  }

  getDataDefectReason = () =>
    this.http.get<any>(this.baseUrl + "RFT/getalldefectreason");

  // for merge add & edit page
  // get list page rft condition
  getRFTCondition(condition: any = {}) {
    // console.log("condition", condition);
    this.rftconditionSource.next(condition);
  }
  // record current rft data (edit function only)
  changeRFT(rft: MeasumentRFT) {
    this.rftSource.next(rft);
  }
}
