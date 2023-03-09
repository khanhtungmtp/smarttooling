import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs";
import { map } from "rxjs/operators";
import { environment } from "../../../../environments/environment";
import { BLLayoutDesignOverall } from "../../_models/best-line/bl-layout-design-overall";
import { BLLayoutDesignOverallDetail } from "../../_models/best-line/bl-layout-design-overall-detail";
import { BLLayoutDesignOverallParam } from "../../_models/best-line/bl-layout-design-overall-param";
import { OperationResult } from "../../_models/smart-tool/operation-result";
import { PaginatedResult, Pagination } from "../../_models/smart-tool/pagination";

@Injectable({
  providedIn: "root",
})
export class LayoutDesignOverallService {
  baseUrl = environment.apiUrl;

  paramSearchSource = new BehaviorSubject<BLLayoutDesignOverall>(null);
  currentParamSearch = this.paramSearchSource.asObservable();

  layoutSearchSource = new BehaviorSubject<BLLayoutDesignOverallParam>(null);
  currentModelSearch = this.layoutSearchSource.asObservable();


  constructor(private http: HttpClient) { }
  setEdit = (item: BLLayoutDesignOverall) => this.paramSearchSource.next(item);
  setParamSearch = (item: BLLayoutDesignOverallParam) => this.layoutSearchSource.next(item);

  getAllLineNo() {
    return this.http.get<any>(
      this.baseUrl + "LayoutDesignOverall/getAllLineNo"
    );
  }
  getAllLineType() {
    return this.http.get<any>(
      this.baseUrl + "LayoutDesignOverall/getAllLineType"
    );
  }

  getAllModelNo() {
    return this.http.get<any>(
      this.baseUrl + "LayoutDesignOverall/getAllModelNo"
    );
  }
  getAllProdSeason() {
    return this.http.get<any>(
      this.baseUrl + "LayoutDesignOverall/getAllProdSeason"
    );
  }
  search(pagination: Pagination, paramSearch: BLLayoutDesignOverallParam) {
    const paginatedResult: PaginatedResult<BLLayoutDesignOverall[]> = new PaginatedResult<BLLayoutDesignOverall[]>();
    const params = new HttpParams()
      .set("line_no", paramSearch.line_no)
      .set("line_type", paramSearch.line_type)
      .set("model", paramSearch.model)
      .set("prod_season", paramSearch.prodSeason)
      .set("pageNumber", pagination.currentPage.toString())
      .set("pageSize", pagination.itemsPerPage.toString());
    return this.http.get<BLLayoutDesignOverall[]>(this.baseUrl + "LayoutDesignOverall/search", { observe: "response", params })
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
    return this.http.post<OperationResult>(this.baseUrl + "LayoutDesignOverall/add", model);
  }
  update(model: BLLayoutDesignOverallDetail) {
    return this.http.put(this.baseUrl + "LayoutDesignOverall/update", model);
  }
}
