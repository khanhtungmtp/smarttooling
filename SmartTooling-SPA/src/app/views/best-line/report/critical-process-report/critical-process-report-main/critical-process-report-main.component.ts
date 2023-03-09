import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Select2OptionData } from 'ng-select2';
import { NgxSpinnerService } from 'ngx-spinner';
import { of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../../../../../../environments/environment';
import { CaptionConstants, MessageConstants } from '../../../../../_core/_constants/system.constants';
import { CriticalProcessReportDTO } from '../../../../../_core/_models/best-line/critical-process-report-dto';
import { CriticalProcessReportParam } from '../../../../../_core/_models/best-line/critical-process-report-param';
import { Pagination } from '../../../../../_core/_models/smart-tool/pagination';
import { CriticalProcessReportService } from '../../../../../_core/_services/best-line/critical-process-report.service';
import { NgSnotifyService } from '../../../../../_core/_services/ng-snotify.service';
import { FunctionUtility } from '../../../../../_core/_utility/function-utility';

@Component({
  selector: 'app-critical-process-report-main',
  templateUrl: './critical-process-report-main.component.html',
  styleUrls: ['./critical-process-report-main.component.css']
})
export class CriticalProcessReportMainComponent implements OnInit {
  message = MessageConstants;
  isSuccess: boolean = false;
  factory: string = localStorage.getItem('factorySmartTooling');
  searchParam: CriticalProcessReportParam = {
    factory_id: this.factory,
    line_no: '',
    line_type: '',
    model_no: '',
    stage: '',
    language: 'en',
    operation: ''
  }
  editModel: CriticalProcessReportParam = {
    factory_id: '',
    line_no: '',
    line_type: '',
    model_no: '',
    stage: '',
    language: '',
    operation: ''
  }

  models: CriticalProcessReportDTO[] = [];
  factories: Array<Select2OptionData> = [];
  listLineNo: Array<Select2OptionData> = [];
  listLineType: Array<Select2OptionData> = [];
  listStage: Array<Select2OptionData> = [];

  pagination: Pagination = {
    currentPage: 1,
    itemsPerPage: 10,
    totalItems: 0,
    totalPages: 0,
  };


  constructor(
    private snotify: NgSnotifyService,
    private router: Router,
    private spinnerService: NgxSpinnerService,
    private criticalReportService: CriticalProcessReportService,
  ) { }

  async ngOnInit() {
    await this.getAllFactory();
  }

  async search() {
    this.pagination.currentPage = 1;
    await this.getData();
    if (this.isSuccess)
      this.snotify.success(MessageConstants.QUERY_SUCCESS, CaptionConstants.SUCCESS);
  }

  async getData() {
    this.spinnerService.show();
    await this.criticalReportService.changeParamSearch(this.searchParam);
    let res = await this.criticalReportService.search(this.pagination, this.searchParam).pipe(catchError(e => {
      this.snotify.error(MessageConstants.SYSTEM_ERROR_MSG, CaptionConstants.ERROR);
      return of(null);
    })).toPromise()
    if (res) {
      this.models = res.result;
      this.pagination = res.pagination;
      this.isSuccess = true;
    } else {
      this.isSuccess = false;
    }
    this.spinnerService.hide();
  }

  async getAllFactory() {
    let data = await this.criticalReportService.getAllFactory().toPromise();
    this.factories = data.map(item => ({ id: item.key, text: item.value }));
  }

  async getLineNo() {
    let data = await this.criticalReportService.getLineNo(this.searchParam.factory_id).toPromise();
    this.listLineNo = data.map(item => ({ id: item.key, text: item.value }));
  }

  async getLineType() {
    let data = await this.criticalReportService.getLineType(this.searchParam.factory_id).toPromise();
    this.listLineType = data.map(item => ({ id: item.key, text: item.value }));
  }

  async getStage() {
    let data = await this.criticalReportService.getStage(this.searchParam.factory_id).toPromise();
    this.listStage = data.map(item => ({ id: item.key, text: item.value }));
  }

  async pageChanged(event: any) {
    this.pagination.currentPage = event.page;
    await this.getData();
  }

  async clear() {
    this.reset();
    this.searchParam.factory_id = this.factory;
    await this.getLineNo();
    await this.getLineType();
    await this.getStage();
    await this.getData();
    this.snotify.success(MessageConstants.CLEAR, CaptionConstants.SUCCESS);
  }
  goToDetail(data: CriticalProcessReportDTO) {
    this.editModel.factory_id = data.factory_id;
    this.editModel.line_no = data.line_id;
    this.editModel.line_type = data.line_type_id;
    this.editModel.model_no = data.model_no;
    this.editModel.stage = data.stage_id;
    this.editModel.operation = data.operation_id;
    this.editModel.language = this.searchParam.language;
    this.criticalReportService.changeModel(this.editModel);
    this.router.navigate(['/best-line/report/critical-process-report/detail']);
  }

  async changeFactory() {
    this.reset();
    this.criticalReportService.currentParamSearch.subscribe(param => {
      if (param) {
        this.searchParam = param;
      }
    }).unsubscribe();
    await this.getLineNo();
    await this.getLineType();
    await this.getStage();
    await this.getData();
  }

  reset() {
    this.searchParam.line_no = '';
    this.searchParam.line_type = '';
    this.searchParam.model_no = '';
    this.searchParam.stage = '';
    this.searchParam.language = 'en';
    this.searchParam.operation = '';
    this.pagination = {
      currentPage: 1,
      itemsPerPage: 10,
      totalItems: 0,
      totalPages: 0,
    };
  }
}
