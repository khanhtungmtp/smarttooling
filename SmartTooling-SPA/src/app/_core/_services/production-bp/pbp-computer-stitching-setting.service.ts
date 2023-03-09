import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs";
import { map } from "rxjs/operators";
import { environment } from "../../../../environments/environment";
import { CaptionConstants, MessageConstants } from "../../_constants/system.constants";
import { PBPSearchComputerStitchingSetting } from "../../_helpers/params/production-bp/pbp-search-computer-stitching-setting";
import { ComputerStitchingSettingView } from "../../_models/production-bp/pbp-computer-stitching-setting-view";
import { ModelOperation } from "../../_models/smart-tool/model-operation";
import { OperationResult } from "../../_models/smart-tool/operation-result";
import {
  PaginatedResult,
  Pagination,
} from "../../_models/smart-tool/pagination";
import { KeyValuePair } from "../../_utility/key-value-pair";
import { NgSnotifyService } from "../ng-snotify.service";

@Injectable({
  providedIn: "root",
})
export class PbpComputerStitchingSettingService {
  apiUrl = environment.apiUrl;
  modelSource = new BehaviorSubject<ComputerStitchingSettingView>(null);
  currentModel = this.modelSource.asObservable();
  paramSearchSource = new BehaviorSubject<PBPSearchComputerStitchingSetting>(
    null
  );
  currentParamSearch = this.paramSearchSource.asObservable();
  constructor(private http: HttpClient, private snotify: NgSnotifyService,) { }

  setModel = (item: ComputerStitchingSettingView) =>
    this.modelSource.next(item);
  setParamSearch = (item: PBPSearchComputerStitchingSetting) =>
    this.paramSearchSource.next(item);

  getAllComputerStitchingSetting(
    searchParam: PBPSearchComputerStitchingSetting,
    pagination: Pagination
  ) {
    const paginatedResult: PaginatedResult<ComputerStitchingSettingView[]> =
      new PaginatedResult<ComputerStitchingSettingView[]>();
    const params = new HttpParams()
      .set("DevSeason", searchParam.devSeason)
      .set("ProductionSeason", searchParam.productionSeason)
      .set("Model", searchParam.model)
      .set("ProductionAdoption", searchParam.productionAdoption)
      .set("pageNumber", pagination.currentPage.toString())
      .set("pageSize", pagination.itemsPerPage.toString());
    return this.http
      .get<PaginatedResult<ComputerStitchingSettingView[]>>(
        `${this.apiUrl}BPComputerStitchingSetting/GetAllComputerStitchingSetting`,
        { observe: "response", params }
      )
      .pipe(
        map((response) => {
          paginatedResult.result = <any>response.body;
          if (response.headers.get("Pagination") != null) {
            paginatedResult.pagination = JSON.parse(
              response.headers.get("Pagination")
            );
          }
          return paginatedResult;
        })
      );
  }

  createComputerStitchingSetting(model: ComputerStitchingSettingView) {
    model.cs_machine_model.toUpperCase();
    return this.http.post<OperationResult>(
      `${this.apiUrl}BPComputerStitchingSetting/CreateComputerStitchingSetting`,
      model
    );
  }

  updateComputerStitchingSetting(model: ComputerStitchingSettingView) {
    model.cs_machine_model.toUpperCase();
    return this.http.put<OperationResult>(
      `${this.apiUrl}BPComputerStitchingSetting/UpdateComputerStitchingSetting`,
      model
    );
  }

  getModel() {
    return this.http.get<KeyValuePair[]>(
      `${this.apiUrl}BPComputerStitchingSetting/GetModel`
    );
  }

  getStage() {
    return this.http.get<KeyValuePair[]>(
      `${this.apiUrl}BPComputerStitchingSetting/GetStage`
    );
  }

  getCSOperation(modelNo: string, stageID: string) {
    const params = new HttpParams()
      .set("model_no", modelNo)
      .set("stage_id", stageID);
    return this.http.get<ModelOperation[]>(
      `${this.apiUrl}BPComputerStitchingSetting/GetCSOperation`,
      { params }
    );
  }

  getCSType() {
    return this.http.get<KeyValuePair[]>(
      `${this.apiUrl}BPComputerStitchingSetting/GetCSType`
    );
  }

  getCSMachineType() {
    return this.http.get<KeyValuePair[]>(
      `${this.apiUrl}BPComputerStitchingSetting/GetCSMachineType`
    );
  }

  getMainUpperMaterialType() {
    return this.http.get<KeyValuePair[]>(
      `${this.apiUrl}BPComputerStitchingSetting/GetMainUpperMaterialType`
    );
  }

  getJigDesign() {
    return this.http.get<KeyValuePair[]>(
      `${this.apiUrl}BPComputerStitchingSetting/GetJigDesign`
    );
  }

  uploadExcel(formData: FormData) {
    return this.http.post<OperationResult>(`${this.apiUrl}BPComputerStitchingSetting/UploadExcel`, formData);
  }
}
