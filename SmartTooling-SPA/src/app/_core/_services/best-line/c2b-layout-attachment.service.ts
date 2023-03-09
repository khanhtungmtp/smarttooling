import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs";
import { map } from "rxjs/operators";
import { environment } from "../../../../environments/environment";
import { BLAttachments } from "../../_models/best-line/bl_attachments";
import { BLAttachmentsDetail } from "../../_models/best-line/bl_attachments_detail";
import { C2BLayoutAttachmentParam } from "../../_models/best-line/bl_attachments_param";
import { KeyValuePair } from "../../_models/key-value-pair";
import {
  PaginatedResult,
  Pagination
} from "../../_models/smart-tool/pagination";

@Injectable({
  providedIn: "root",
})
export class C2BLayoutAttachmentService {
  baseUrl = environment.apiUrl;
  paramSearchSource = new BehaviorSubject<any>(null);
  currentParamSearch = this.paramSearchSource.asObservable();
  constructor(private http: HttpClient) { }

  changeSearchSource(params: any) {
    this.paramSearchSource.next(params);
  }

  getAllLineNo() {
    return this.http.get<KeyValuePair[]>(
      this.baseUrl + "C2BLayoutAttachment/GetAllLineNo"
    );
  }
  getAllLineType(lindId: string) {
    return this.http.get<KeyValuePair[]>(
      this.baseUrl + "C2BLayoutAttachment/GetAllLineType?lineID=" + lindId
    );
  }
  getAllModelNo(lindId: string, lineTypeId: string) {
    return this.http.get<KeyValuePair[]>(
      this.baseUrl +
      `C2BLayoutAttachment/GetAllModelNo?lineId=${lindId}&lineTypeId=${lineTypeId}`
    );
  }
  getAllAttachmentType() {
    return this.http.get<KeyValuePair[]>(
      this.baseUrl + "C2BLayoutAttachment/getAllAttachmentType"
    );
  }
  // changeParamSearch(paramSearch: C2BLayoutAttachmentParam) {
  //   this.paramSearchSource.next(paramSearch);
  // }
  search(pagination: Pagination, paramSearch: C2BLayoutAttachmentParam) {
    const paginatedResult: PaginatedResult<BLAttachments[]> =
      new PaginatedResult<BLAttachments[]>();
    const params = new HttpParams()
      .set("line_id", paramSearch.line_no)
      .set("line_type_id", paramSearch.line_type)
      .set("model", paramSearch.modelNo)
      .set("pageNumber", pagination.currentPage.toString())
      .set("pageSize", pagination.itemsPerPage.toString());
    return this.http
      .get<BLAttachments[]>(this.baseUrl + "C2BLayoutAttachment/Search", {
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

  create(model: BLAttachmentsDetail, file: File) {
    const params = new HttpParams()
      .set("factory_id", model.factory_id)
      .set("line_id", model.line_id)
      .set("line_type_id", model.line_type_id)
      .set("model_no", model.model_no)
      .set("attachment_type_id", model.attachment_type_id)
      .set("attachment_name", model.attachment_name)
      .set("attachment_file_url", model.attachment_file_url)
      .set("update_by", model.update_by)
      .set("update_time", model.update_time);
    const formData = new FormData();
    formData.append("file", file);

    return this.http.post(
      this.baseUrl + "C2BLayoutAttachment/create",
      formData,
      { params }
    );
  }
  delete(model: BLAttachmentsDetail) {
    const params = new HttpParams()
      .set("factory_id", model.factory_id)
      .set("line_id", model.line_id)
      .set("line_type_id", model.line_type_id)
      .set("model_no", model.model_no)
      .set("attachment_type_id", model.attachment_type_id)
      .set("attachment_name", model.attachment_name)
      .set("attachment_file_url", model.attachment_file_url);
    return this.http.delete<any>(this.baseUrl + "C2BLayoutAttachment/delete", { params });
  }
}
