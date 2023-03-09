import { environment } from './../../../../environments/environment';
import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable } from "rxjs";
import { map } from "rxjs/operators";
import { BLLayoutDesignOverallDetail } from "../../_models/best-line/bl-layout-design-overall-detail";
import { KeyValuePair } from "../../_models/key-value-pair";
import { PBPBondingProgramSetting } from "../../_models/production-bp/pbp-bonding-program-setting";
import { PBPBondingProgramSettingPram } from "../../_models/production-bp/pbp-bonding-program-setting-pram";
import { OperationResult } from "../../_models/smart-tool/operation-result";
import { PaginatedResult, Pagination } from "../../_models/smart-tool/pagination";

@Injectable({
  providedIn: "root",
})
export class BondingProgramSettingService {
  baseUrl = environment.apiUrl;
  urlExcel = environment.baseUrl;

  paramSearchSource = new BehaviorSubject<PBPBondingProgramSetting>(null);
  currentParamSearch = this.paramSearchSource.asObservable();

  layoutSearchSource = new BehaviorSubject<PBPBondingProgramSettingPram>(null);
  currentModelSearch = this.layoutSearchSource.asObservable();


  constructor(private http: HttpClient) { }

  setEdit = (item: PBPBondingProgramSetting) => this.paramSearchSource.next(item);
  setParamSearch = (item: PBPBondingProgramSettingPram) => this.layoutSearchSource.next(item);

  getAllAutoTech() {
    return this.http.get<KeyValuePair[]>(
      this.baseUrl + "BondingProgramSetting/getAllAutoTech"
    );
  }

  getAllChemicalProcessType() {
    return this.http.get<KeyValuePair[]>(
      this.baseUrl + "BondingProgramSetting/getAllChemicalProcessType"
    );
  }

  getAllModelNo() {
    return this.http.get<KeyValuePair[]>(
      this.baseUrl + "BondingProgramSetting/getAllModelNo"
    );
  }


  getAllAdoptionComponentType() {
    return this.http.get<KeyValuePair[]>(
      this.baseUrl + "BondingProgramSetting/getAllAdoptionComponentType"
    );
  }


  getAllProcessAdoptionScopeType() {
    return this.http.get<KeyValuePair[]>(
      this.baseUrl + "BondingProgramSetting/getAllProcessAdoptionScopeType"
    );
  }


  getAllMainUpperMaterialType() {
    return this.http.get<KeyValuePair[]>(
      this.baseUrl + "BondingProgramSetting/getAllMainUpperMaterialType"
    );
  }

  getAllMainBottomMaterialType() {
    return this.http.get<KeyValuePair[]>(
      this.baseUrl + "BondingProgramSetting/getAllMainBottomMaterialType"
    );
  }

  getAllChemicalSupplierType() {
    return this.http.get<KeyValuePair[]>(
      this.baseUrl + "BondingProgramSetting/getAllChemicalSupplier"
    );
  }

  search(pagination: Pagination, paramSearch: PBPBondingProgramSettingPram) {
    const paginatedResult: PaginatedResult<PBPBondingProgramSetting[]> = new PaginatedResult<PBPBondingProgramSetting[]>();
    const params = new HttpParams()
      .set("auto_tech_id", paramSearch.auto_tech_id)
      .set("chemical_process_type_id", paramSearch.chemical_process_type_id)
      .set("model", paramSearch.model)
      .set("production_season", paramSearch.production_season)
      .set("pageNumber", pagination.currentPage.toString())
      .set("pageSize", pagination.itemsPerPage.toString());
    return this.http.get<PBPBondingProgramSetting[]>(this.baseUrl + "BondingProgramSetting/search", { observe: "response", params })
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

  create(model: PBPBondingProgramSetting) {
    return this.http.post(this.baseUrl + "BondingProgramSetting/add", model);
  }

  update(model: BLLayoutDesignOverallDetail) {
    return this.http.put(this.baseUrl + "BondingProgramSetting/update", model);
  }

  uploadExcel(file: File): Observable<OperationResult> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post<OperationResult>(`${this.baseUrl}BondingProgramSetting/uploadExcel`, formData)
  }
}
