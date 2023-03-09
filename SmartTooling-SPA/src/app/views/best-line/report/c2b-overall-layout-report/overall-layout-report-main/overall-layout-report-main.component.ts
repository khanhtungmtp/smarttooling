import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Select2OptionData } from 'ng-select2';
import { NgxSpinnerService } from 'ngx-spinner';
import { of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../../../../../../environments/environment';
import { CaptionConstants, MessageConstants } from '../../../../../_core/_constants/system.constants';
import { C2BOverallLayoutReport } from '../../../../../_core/_models/best-line/overall-layout-report';
import { C2BOverallLayoutReportParam } from '../../../../../_core/_models/best-line/overall-layout-report-param';
import { Pagination } from '../../../../../_core/_models/smart-tool/pagination';
import { C2bOverallLayoutReportService } from '../../../../../_core/_services/best-line/c2b-overall-layout-report.service';
import { NgSnotifyService } from '../../../../../_core/_services/ng-snotify.service';
@Component({
  selector: 'app-overall-layout-report-main',
  templateUrl: './overall-layout-report-main.component.html',
  styleUrls: ['./overall-layout-report-main.component.scss']
})
export class OverallLayoutReportMainComponent implements OnInit {
  message = MessageConstants;
  factory: string = localStorage.getItem('factorySmartTooling');
  isSuccess: boolean = false;
  searchParam: C2BOverallLayoutReportParam = {
    factory: this.factory,
    line_no: '',
    line_type: '',
    model: ''
  };
  dataModel: C2BOverallLayoutReport[] = [];
  factories: Array<Select2OptionData> = [];
  listLineNo: Array<Select2OptionData> = [];
  listLineType: Array<Select2OptionData> = [];
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
    private overallLayoutReportService: C2bOverallLayoutReportService,
  ) { }

  async ngOnInit() {
    await this.getAllFactory();
  }

  async getAllFactory() {
    let data = await this.overallLayoutReportService.getAllFactory().toPromise();
    this.factories = data.map(item => ({ id: item.key, text: item.value }));
  }

  async getLineNo() {
    let data = await this.overallLayoutReportService.getLineNo(this.searchParam.factory).toPromise();
    this.listLineNo = data.map(item => ({ id: item.key, text: item.value }));
  }

  async getLineType() {
    let data = await this.overallLayoutReportService.getLineType(this.searchParam.factory).toPromise();
    this.listLineType = data.map(item => ({ id: item.key, text: item.value }));
  }

  async clear() {
    this.reset();
    this.searchParam.factory = this.factory;
    await this.getLineNo();
    await this.getLineType();
    await this.getData();
    this.snotify.success(MessageConstants.CLEAR, CaptionConstants.SUCCESS);
  }

  async search() {
    this.pagination.currentPage = 1;
    await this.getData();
    if (this.isSuccess)
      this.snotify.success(MessageConstants.QUERY_SUCCESS, CaptionConstants.SUCCESS);

  }

  async getData() {
    this.spinnerService.show();
    await this.overallLayoutReportService.changeParamSearch(this.searchParam);
    let res = await this.overallLayoutReportService.search(this.pagination, this.searchParam).pipe(catchError(e => {
      this.snotify.error(MessageConstants.SYSTEM_ERROR_MSG, CaptionConstants.ERROR);
      return of(null);
    })).toPromise();
    if (res) {
      this.pagination = res.pagination;
      this.dataModel = res.result;
      this.isSuccess = true;
    } else {
      this.isSuccess = false;
    }
    this.spinnerService.hide();
  }

  async pageChanged(event: any) {
    this.pagination.currentPage = event.page;
    await this.getData();
  }


  goToDetail(model: C2BOverallLayoutReport) {
    this.overallLayoutReportService.changeModel(model);
    this.router.navigate(["/best-line/report/overall-layout-report/detail"]);
  }

  async changeFactory() {
    this.reset();
    this.overallLayoutReportService.currentParamSearch.subscribe(res => {
      if (res) {
        this.searchParam = res;
      }
    }).unsubscribe();
    await this.getLineNo();
    await this.getLineType();
    await this.getData();
  }

  reset() {
    this.searchParam.line_no = "";
    this.searchParam.line_type = "";
    this.searchParam.model = "";
    this.pagination = {
      currentPage: 1,
      itemsPerPage: 10,
      totalItems: 0,
      totalPages: 0,
    };
  }
}
