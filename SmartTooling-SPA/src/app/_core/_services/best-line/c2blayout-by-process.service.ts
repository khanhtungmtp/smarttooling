import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '../../../../environments/environment';
import { BL_Layout_Design_Process } from '../../_models/best-line/bL_Layout_Design_Process';
import { BL_Layout_Design_Process_Params } from '../../_models/best-line/bL_Layout_Design_Process_Params';
import { OperationResult } from '../../_models/smart-tool/operation-result';
import { PaginatedResult } from '../../_models/smart-tool/pagination';

@Injectable({
  providedIn: 'root'
})
export class C2blayoutByProcessService {
  baseUrl = environment.apiUrl;
  blProcess = new BehaviorSubject<any>(null);
  currentProcess = this.blProcess.asObservable();
  idProcessEdit = new BehaviorSubject<any>(null);
  currentProcessEdit = this.idProcessEdit.asObservable();


  constructor(private http: HttpClient) { }


  search(pageNumber: number, pageSize: number, param)
    : Observable<PaginatedResult<BL_Layout_Design_Process_Params[]>> {
    const paginatedResult: PaginatedResult<
      BL_Layout_Design_Process_Params[]
    > = new PaginatedResult<BL_Layout_Design_Process_Params[]>();
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString())
      .set('lineID', param.lineID)
      .set('lineTypeID', param.lineTypeID)
      .set('modelNo', param.modelNo)

    return this.http.get<BL_Layout_Design_Process_Params[]>(this.baseUrl + 'C2BLayoutByProcess/Search', { observe: "response", params })
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
      );;
  }

  changeIDProcess(params: any) {
    this.blProcess.next(params);
  }

  changeProcessEdit(params: any) {
    this.idProcessEdit.next(params);
  }

  getLineID() {
    return this.http.get<any>(this.baseUrl + 'C2BLayoutByProcess/GetLineID');
  }

  getLineTypeID(lineID: string) {
    return this.http.get<any>(this.baseUrl + 'C2BLayoutByProcess/GetLineTypeID', { params: { lineID: lineID } });
  }

  getmodelNo(lineID: string, lineTypeID: string) {
    return this.http.get<any>(this.baseUrl + 'C2BLayoutByProcess/GetModelNo', { params: { lineID: lineID, lineTypeID: lineTypeID } });
  }

  getProcessType() {
    return this.http.get<any>(this.baseUrl + 'C2BLayoutByProcess/GetProcessType');
  }

  add(lineID: BL_Layout_Design_Process) {
    return this.http.post<OperationResult>(this.baseUrl + 'C2BLayoutByProcess/Add', lineID);
  }

  edit(lineID: BL_Layout_Design_Process) {
    return this.http.put<OperationResult>(this.baseUrl + 'C2BLayoutByProcess/Edit', lineID);
  }

  getDataEdit(lineID: string, lineTypeID: string, modelNo: string, processNo: string) {
    let params = new HttpParams().set('lineID', lineID).set('lineTypeID', lineTypeID).set('modelNo', modelNo).set('processNo', processNo);
    return this.http.get<BL_Layout_Design_Process>(this.baseUrl + 'C2BLayoutByProcess/itemEdit', { params });
  }
}
