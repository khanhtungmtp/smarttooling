import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Select2OptionData } from 'ng-select2';
import { NgxSpinnerService } from 'ngx-spinner';
import { of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../../../../../../environments/environment';
import { CaptionConstants, MessageConstants } from '../../../../../_core/_constants/system.constants';
import { C2BLayoutByProcessDTO } from '../../../../../_core/_models/best-line/layout-by-process-dto';
import { C2BLayoutByProcessReportParam } from '../../../../../_core/_models/best-line/layout-by-process-report-param';
import { Pagination } from '../../../../../_core/_models/smart-tool/pagination';
import { C2bLayoutByProcessReportService } from '../../../../../_core/_services/best-line/c2b-layout-by-process-report.service';
import { NgSnotifyService } from '../../../../../_core/_services/ng-snotify.service';

@Component({
  selector: 'app-c2b-layout-by-process-report-main',
  templateUrl: './c2b-layout-by-process-report-main.component.html',
  styleUrls: ['./c2b-layout-by-process-report-main.component.css']
})
export class C2bLayoutByProcessReportMainComponent implements OnInit {
  message = MessageConstants;
  isSuccess: boolean = false;
  factory: string = localStorage.getItem('factorySmartTooling');
  searchParam: C2BLayoutByProcessReportParam = {
    factory: this.factory,
    line_no: '',
    line_type: '',
    model: '',
    process_no: ''
  }
  dataModel: C2BLayoutByProcessDTO[] = [];
  factories: Array<Select2OptionData> = [];
  listLineNo: Array<Select2OptionData> = [];
  listLineType: Array<Select2OptionData> = [];
  listProcessType: Array<Select2OptionData> = [];

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
    private layoutByProcessReportService: C2bLayoutByProcessReportService
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
    await this.layoutByProcessReportService.changeParamSearch(this.searchParam);
    let res = await this.layoutByProcessReportService.search(this.pagination, this.searchParam).pipe(catchError(e => {
      this.snotify.error(MessageConstants.SYSTEM_ERROR_MSG, CaptionConstants.ERROR);
      return of(null);
    })).toPromise();
    if (res) {
      this.dataModel = res.result;
      this.pagination = res.pagination;
      this.isSuccess = true;
    } else {
      this.isSuccess = false;
    }
    this.spinnerService.hide();
  }

  async pageChanged(event) {
    this.pagination.currentPage = event.page;
    await this.getData();
  }

  async clear() {
    this.reset();
    this.searchParam.factory = this.factory;
    await this.getLineNo();
    await this.getLineType();
    await this.getProcessNo();
    await this.getData();
    this.snotify.success(MessageConstants.CLEAR, CaptionConstants.SUCCESS);
  }

  async getAllFactory() {
    let data = await this.layoutByProcessReportService.getAllFactory().toPromise();
    this.factories = data.map(item => ({ id: item.key, text: item.value }));
  }

  async getLineNo() {
    let data = await this.layoutByProcessReportService.getLineNo(this.searchParam.factory).toPromise();
    this.listLineNo = data.map(item => ({ id: item.key, text: item.value }));
  }

  async getLineType() {
    let data = await this.layoutByProcessReportService.getLineType(this.searchParam.factory).toPromise();
    this.listLineType = data.map(item => ({ id: item.key, text: item.value }));
  }

  async getProcessNo() {
    let data = await this.layoutByProcessReportService.getProcessType(this.searchParam.factory).toPromise();
    this.listProcessType = data.map(item => ({ id: item.key, text: item.value }));
  }

  goToDetail(model: C2BLayoutByProcessDTO) {
    this.layoutByProcessReportService.changeModel(model);
    this.router.navigate(['/best-line/report/layout-by-process-report/detail']);
  }

  async changeFactory() {
    this.reset();
    this.layoutByProcessReportService.currentParamSearch.subscribe(res => {
      if (res) {
        this.searchParam = res;
      }
    }).unsubscribe();
    await this.getLineNo();
    await this.getLineType();
    await this.getProcessNo();
    await this.getData();
  }

  reset() {
    this.searchParam.line_no = '';
    this.searchParam.line_type = '';
    this.searchParam.model = '';
    this.searchParam.process_no = '';
    this.pagination = {
      currentPage: 1,
      itemsPerPage: 10,
      totalItems: 0,
      totalPages: 0,
    }
  }
}
