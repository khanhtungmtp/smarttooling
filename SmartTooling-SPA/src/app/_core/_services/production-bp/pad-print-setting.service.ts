import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { SnotifyService } from 'ng-snotify';
import { NgxSpinnerService } from 'ngx-spinner';
import { BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '../../../../environments/environment';
import { CaptionConstants, MessageConstants } from '../../_constants/system.constants';
import { PadPrintSettingParam } from '../../_helpers/params/production-bp/pad-print-setting-param';
import { PbpPadPrintSetting } from '../../_models/production-bp/pbp-pad-print-setting';
import { PBP_Pad_Print_Setting_DTO } from '../../_models/production-bp/pbp-pad-print-setting-dto';
import { OperationResult } from '../../_models/smart-tool/operation-result';
import { PaginatedResult, Pagination } from '../../_models/smart-tool/pagination';
import { FunctionUtility } from '../../_utility/function-utility';
import { KeyValuePair } from '../../_utility/key-value-pair';

@Injectable({
  providedIn: 'root'
})
export class PadPrintSettingService {
  baseUrl = environment.apiUrl
  constructor(private http: HttpClient, private spiner: NgxSpinnerService, private snotify: SnotifyService, private functionUtil: FunctionUtility) { }
  modelSource = new BehaviorSubject<PBP_Pad_Print_Setting_DTO>(null);
  currentModel = this.modelSource.asObservable();
  searchModelSource = new BehaviorSubject<PadPrintSettingParam>(null);
  currentSearchModel = this.searchModelSource.asObservable();
  getAllModel() {
    return this.http.get<KeyValuePair[]>(this.baseUrl + 'PadPrintSetting/GetAllModel');
  }

  getAllPadShape() {
    return this.http.get<KeyValuePair[]>(this.baseUrl + 'PadPrintSetting/GetAllPadShape');
  }

  getAllMaterialType() {
    return this.http.get<KeyValuePair[]>(this.baseUrl + 'PadPrintSetting/GetAllMaterialType');
  }

  getAllMachineVendor() {
    return this.http.get<KeyValuePair[]>(this.baseUrl + 'PadPrintSetting/GetAllMachineVendor');
  }

  search(filterParam: PadPrintSettingParam, pagination: Pagination) {
    const paginatedResult: PaginatedResult<PBP_Pad_Print_Setting_DTO[]> = new PaginatedResult<PBP_Pad_Print_Setting_DTO[]>();
    let params = new HttpParams()
      .set('dev_season', filterParam.dev_season.toUpperCase())
      .set('production_season', filterParam.production_season.toUpperCase())
      .set('model_no', filterParam.model_no.toUpperCase())
      .set('pageNumber', pagination.currentPage.toString())
      .set('pageSize', pagination.itemsPerPage.toString());

    return this.http.get<PBP_Pad_Print_Setting_DTO[]>(this.baseUrl + 'PadPrintSetting/Search', { observe: 'response', params })
      .pipe(
        map(response => {
          paginatedResult.result = response.body;
          if (response.headers.get('Pagination') != null) {
            paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
          }
          return paginatedResult;
        })
      );
  }

  add(padPrintSetting: PbpPadPrintSetting) {
    return this.http.post<OperationResult>(this.baseUrl + 'PadPrintSetting/Create', padPrintSetting);
  }

  update(padPrintSetting: PbpPadPrintSetting) {
    return this.http.put<OperationResult>(this.baseUrl + 'PadPrintSetting/Update', padPrintSetting);
  }

  getDetail(param: PBP_Pad_Print_Setting_DTO) {
    let params = new HttpParams()
      .set('factory_id', param.factory_id)
      .set('model_no', param.model_no.toUpperCase())
      .set('component_name', param.component_name.toUpperCase())
      .set('material_type_id', param.material_type_id)
      .set('chemical_ink', param.chemical_ink);
    return this.http.get<PbpPadPrintSetting>(this.baseUrl + 'PadPrintSetting/GetDetail', { params });
  }

  changeModel(model: PBP_Pad_Print_Setting_DTO) {
    this.modelSource.next(model);
  }

  changeSearch(model: PadPrintSettingParam) {
    this.searchModelSource.next(model);
  }

  uploadExcel(file: File) {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post<OperationResult>(`${this.baseUrl}PadPrintSetting/UploadExcel`, formData);
  }
}
