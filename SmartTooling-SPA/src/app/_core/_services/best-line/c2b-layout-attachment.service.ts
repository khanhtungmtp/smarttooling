import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs";
import { map } from "rxjs/operators";
import { environment } from "../../../../environments/environment";
import { C2bLayoutAttachmentAddParam, C2bLayoutAttachmentParam } from "../../_helpers/params/best-line/c2b-layout-attachment-param";
import { C2bLayoutAttachmentResult } from "../../_models/best-line/c2b_layout_attachment_result";
import { KeyValuePair } from "../../_models/key-value-pair";
import {
  PaginatedResult,
  Pagination
} from "../../_models/smart-tool/pagination";

@Injectable({
  providedIn: "root",
})
export class C2BLayoutAttachmentService {
  baseUrl = environment.apiUrl + 'C2BLayoutAttachment/';
  paramSearchSource = new BehaviorSubject<any>(null);
  currentParamSearch = this.paramSearchSource.asObservable();
  constructor(private http: HttpClient) { }

  changeSearchSource(params: any) {
    this.paramSearchSource.next(params);
  }
  getAllLineNo() {
    return this.http.get<any>(
      this.baseUrl + "GetAllLineNo"
    );
  }
  getAllLineType() {
    return this.http.get<any>(
      this.baseUrl + "GetAllLineType"
    );
  }
  getAllProdSeason() {
    return this.http.get<any>(this.baseUrl + 'getAllProdSeason')
  }
  getAllAttachmentType() {
    return this.http.get<KeyValuePair[]>(
      this.baseUrl + "getAllAttachmentType"
    );
  }
  getAllLineNoOfAdd() {
    return this.http.get<any>(this.baseUrl + 'GetAllLineNoOfAdd')
  }
  getAllLineTypeOfAdd(line_id: string) {
    let params = new HttpParams().set('line_id', line_id)
    return this.http.get<any>(this.baseUrl + 'GetAllLineTypeOfAdd', { params });
  }
  getAllModelNoOfAdd(line_id: string, line_type_id: string) {
    let params = new HttpParams()
      .set('line_id', line_id)
      .set('line_type_id', line_type_id)
    return this.http.get<any>(this.baseUrl + 'GetAllModelNoOfAdd', { params });
  }
  getAllProdSeasonOfAdd(line_id: string, line_type_id: string, model_no: string) {
    let params = new HttpParams()
      .set('line_id', line_id)
      .set('line_type_id', line_type_id)
      .set('model_no', model_no)
    return this.http.get<any>(this.baseUrl + 'GetAllProdSeasonOfAdd', { params });
  }
  search(pagination: Pagination, paramSearch: C2bLayoutAttachmentParam) {
    const paginatedResult: PaginatedResult<C2bLayoutAttachmentResult[]> =
      new PaginatedResult<C2bLayoutAttachmentResult[]>();
    const paginations = {
      'PageNumber': pagination.currentPage,
      'PageSize': pagination.itemsPerPage
    }
    const params = new HttpParams().appendAll({ ...paginations, ...paramSearch })
    return this.http
      .get<C2bLayoutAttachmentResult[]>(this.baseUrl + "Search", {
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
  create(model: C2bLayoutAttachmentAddParam, file: File) {
    const params = new HttpParams().appendAll({ ...model })
    const formData = new FormData();
    formData.append("file", file);

    return this.http.post(
      this.baseUrl + "create",
      formData,
      { params }
    );
  }
  delete(attachment_file_url: string) {
    const params = new HttpParams().append('attachment_file_url', attachment_file_url)
    return this.http.delete<any>(this.baseUrl + "delete", { params });
  }
}
