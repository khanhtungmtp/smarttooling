import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs";
import { map } from "rxjs/operators";
import { environment } from "../../../../environments/environment";
import { BLLayoutDesignOverall } from "../../_models/best-line/bl-layout-design-overall";
import { BLLayoutDesignOverallDetail } from "../../_models/best-line/bl-layout-design-overall-detail";
import { BLLayoutDesignOverallParam } from "../../_helpers/params/best-line/bl-layout-design-overall-param";
import { OperationResult } from "../../_models/smart-tool/operation-result";
import { PaginatedResult, Pagination } from "../../_models/smart-tool/pagination";

@Injectable({
  providedIn: "root",
})
export class LayoutDesignOverallService {
  baseUrl = environment.apiUrl + 'LayoutDesignOverall/';
  paramSearchSource = new BehaviorSubject<BLLayoutDesignOverallDetail>(null);
  currentParamSearch = this.paramSearchSource.asObservable();
  // layoutSearchSource = new BehaviorSubject<BLLayoutDesignOverallParam>(null);
  // currentModelSearch = this.layoutSearchSource.asObservable();

  constructor(private http: HttpClient) { }
  setEdit = (item: BLLayoutDesignOverallDetail) => this.paramSearchSource.next(item);
  //setParamSearch = (item: BLLayoutDesignOverallParam) => this.layoutSearchSource.next(item);
  getLineNoOfMain() {
    return this.http.get<any>(
      this.baseUrl + "getLineNoOfMain"
    );
  }
  getLineTypeOfMain() {
    return this.http.get<any>(
      this.baseUrl + "getLineTypeOfMain"
    );
  }
  getAllLineNo() {
    return this.http.get<any>(
      this.baseUrl + "getAllLineNo"
    );
  }
  getAllLineType() {
    return this.http.get<any>(
      this.baseUrl + "getAllLineType"
    );
  }
  getAllModelNo() {
    return this.http.get<any>(
      this.baseUrl + "getAllModelNo"
    );
  }
  getAllProdSeason() {
    return this.http.get<any>(
      this.baseUrl + "getAllProdSeason"
    );
  }
  search(pagination: Pagination, paramSearch: BLLayoutDesignOverallParam) {
    const paginatedResult: PaginatedResult<BLLayoutDesignOverall[]> = new PaginatedResult<BLLayoutDesignOverall[]>();
    const paginations = {
      'PageNumber': pagination.currentPage,
      'PageSize': pagination.itemsPerPage
    }
    const params = new HttpParams().appendAll({ ...paginations, ...paramSearch })
    return this.http.get<BLLayoutDesignOverall[]>(this.baseUrl + "search", { observe: "response", params })
      .pipe(
        map(response => {
          paginatedResult.result = response.body;
          if (response.headers.get("Pagination") != null) {
            paginatedResult.pagination = JSON.parse(response.headers.get("Pagination"));
          }
          return paginatedResult;
        })
      );
  }
  create(model: BLLayoutDesignOverallDetail) {
    return this.http.post<OperationResult>(this.baseUrl + "add", model);
  }
  update(model: BLLayoutDesignOverallDetail) {
    return this.http.put<OperationResult>(this.baseUrl + "update", model);
  }
}
