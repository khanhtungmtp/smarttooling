import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { BL_Layout_Design_Process_Data_Params } from '../../_models/best-line/bL_Layout_Design_Process_Data_ParamsDTO';
import { C2BLayoutByProcessDetailDTO } from '../../_models/best-line/c2BLayoutByProcessDetailDTO';
import { OperationResult } from '../../_models/smart-tool/operation-result';

@Injectable({
  providedIn: 'root'
})
export class LineBalancingService {
  baseUrl = environment.apiUrl;
  lineBalancing = new BehaviorSubject<any>(null);
  currentLineBalancing = this.lineBalancing.asObservable();

  constructor(private http: HttpClient) { }

  changeIDProcess(params: any) {
    this.lineBalancing.next(params);
  }

  getLineID() {
    return this.http.get<any>(this.baseUrl + 'LineBalancing/GetLineID');
  }

  getLineTypeID(lineID: string) {
    return this.http.get<any>(this.baseUrl + 'LineBalancing/GetLineTypeID', { params: { lineID: lineID } });
  }

  getmodelNo(lineID: string, lineTypeID: string) {
    return this.http.get<any>(this.baseUrl + 'LineBalancing/GetModelNo', { params: { lineID: lineID, lineTypeID: lineTypeID } });
  }

  getProcessType(lineID: string, lineTypeID: string, modelNo: string) {
    return this.http.get<any>(this.baseUrl + 'LineBalancing/GetProcessType', { params: { lineID: lineID, lineTypeID: lineTypeID, modelNo: modelNo } });
  }

  getData(line: BL_Layout_Design_Process_Data_Params) {
    let params = new HttpParams()
      .set('line_id', line.line_id)
      .set('line_type_id', line.line_type_id)
      .set('model_no', line.model_no)
      .set('process_type_id', line.process_type_id)
    return this.http.get<any>(this.baseUrl + 'LineBalancing/GetData', { params });
  }

  GetLayoutDesignProcessData(line: BL_Layout_Design_Process_Data_Params) {
    let params = new HttpParams()
      .set('line_id', line.line_id)
      .set('line_type_id', line.line_type_id)
      .set('model_no', line.model_no)
      .set('process_type_id', line.process_type_id)
      .set('before_or_after', line.before_or_after);
    return this.http.get<C2BLayoutByProcessDetailDTO>(this.baseUrl + 'LineBalancing/GetGraphData', { params });
  }

  getExportExcel(line: BL_Layout_Design_Process_Data_Params) {
    let params = new HttpParams()
      .set('line_id', line.line_id)
      .set('line_type_id', line.line_type_id)
      .set('model_no', line.model_no)
      .set('process_type_id', line.process_type_id)
      .set('before_or_after', line.before_or_after);
    return this.http.get(this.baseUrl + 'LineBalancing/exportExcelAspose', { responseType: 'blob', params })
      .subscribe((result: Blob) => {
        const blob = new Blob([result]);
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        const currentTime = new Date();

        let filename = "LineBalancing_" + localStorage.getItem('factorySmartTooling') + "_" + line.line_id.trim() + "_" + line.line_type_id + "_"
          + line.model_no + "_" + line.process_type_id + "_"
          + currentTime.getFullYear().toString() +
          (currentTime.getMonth() + 1) + currentTime.getDate() +
          currentTime.toLocaleTimeString().replace(/[ ]|[,]|[:]/g, '').trim() + '.xlsx';

        link.href = url;
        link.setAttribute('download', filename);
        document.body.appendChild(link);
        link.click();
      });
  }

  importExcel(line_id: string, line_type_id: string, model_no: string,
    process_type_id: string, model_name: string, file: File) {
    const formData = new FormData();
    formData.append('line_id', line_id);
    formData.append('line_type_id', line_type_id);
    formData.append('model_no', model_no);
    formData.append('process_type_id', process_type_id);
    formData.append('model_name', model_name);
    formData.append('file', file);
    return this.http.post<OperationResult>(this.baseUrl + 'LineBalancing/importExcel', formData);
  }
}
