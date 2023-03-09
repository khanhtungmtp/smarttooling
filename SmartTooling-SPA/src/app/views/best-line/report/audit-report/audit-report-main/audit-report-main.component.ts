import { } from '@angular/core/testing';
import { BlAuditReportService } from "./../../../../../_core/_services/best-line/bl-audit-report.service";
import { Component, OnInit } from "@angular/core";
import { Select2OptionData } from "ng-select2";
import { environment } from "../../../../../../environments/environment";
import { BLSearchAuditReportParam } from "../../../../../_core/_helpers/params/best-line/bl-search-audit-report.param";
import { BLAuditReportDTO } from "../../../../../_core/_models/best-line/bl-audit-report.model";
import { Pagination } from "../../../../../_core/_models/smart-tool/pagination";
import { NgxSpinnerService } from "ngx-spinner";
import { BlRolloutReportService } from "../../../../../_core/_services/best-line/bl-rollout-report.service";
import { Router } from "@angular/router";
import { NgSnotifyService } from "../../../../../_core/_services/ng-snotify.service";
import { CaptionConstants, MessageConstants } from '../../../../../_core/_constants/system.constants';

@Component({
  selector: "app-audit-report-main",
  templateUrl: "./audit-report-main.component.html",
  styleUrls: ["./audit-report-main.component.scss"]
})
export class AuditReportMainComponent implements OnInit {
  auditReports: BLAuditReportDTO[] = [];
  factories: Array<Select2OptionData> = [];
  lineNos: Array<Select2OptionData> = [];
  lineTypes: Array<Select2OptionData> = [];
  stages: Array<Select2OptionData> = [];
  lang: string = "en";
  snotifySearch: boolean = false;
  countRolloutTotal: number = 0;
  countBeingAudit: number = 0;
  search: BLSearchAuditReportParam = {
    factory: localStorage.getItem('factorySmartTooling'),
    lineID: '',
    rolloutLineID: '',
    lineTypeID: '',
    modelNo: '',
    stageID: '',
    operationID: ''
  } as BLSearchAuditReportParam;

  detailParam: BLSearchAuditReportParam = {
    factory: '',
    lineID: '',
    rolloutLineID: '',
    lineTypeID: '',
    modelNo: '',
    stageID: '',
    operationID: ''
  } as BLSearchAuditReportParam;

  pagination: Pagination = {
    currentPage: 1,
    itemsPerPage: 10
  } as Pagination;
  message = MessageConstants;
  constructor(
    private auditService: BlAuditReportService,
    private rolloutService: BlRolloutReportService,
    private spinner: NgxSpinnerService,
    private snotify: NgSnotifyService,
    private router: Router,
  ) { }

  ngOnInit() {
    this.getFactories();
    this.getLineNo();
    this.getLineType();
    this.getStage();
  }

  loadData() {
    if (!this.search.lineID || !this.search.lineTypeID)
      this.snotify.warning(MessageConstants.SELECT_QUERY_OPTIONS, CaptionConstants.WARNING);
    else {
      this.spinner.show();
      this.auditService.getAllAudit(this.search, this.pagination).subscribe(res => {
        this.auditReports = res.result;
        this.pagination = res.pagination;
        this.countRolloutTotal = this.pagination.totalItems;
        if(this.auditReports.length > 0) {
          this.countBeingAudit = this.auditReports[0].countBeingAudit;
        } else {
          this.countRolloutTotal = 0;
          this.countBeingAudit = 0;
        }
        if (this.snotifySearch) {
          this.snotify.success(MessageConstants.QUERY_SUCCESS, CaptionConstants.SUCCESS);
          this.snotifySearch = false;
        }
      }
      );
      this.spinner.hide();
    }
  }


  getFactories() {
    this.rolloutService.getFactory().subscribe(res => {
      this.factories = res.map(item => {
        return { id: item.key, text: item.value };
      });
      this.search.factory = localStorage.getItem('factorySmartTooling');
    });
  }

  getLineNo() {
    this.auditService.getLineNo(this.search.factory).subscribe(res => {
      this.lineNos = res.map(item => {
        return { id: item.key, text: item.value };
      });
      //   this.search.lineID = this.lineNos[0].id;
    });
  }

  getLineType() {
    this.rolloutService.getLineType(this.search.factory).subscribe(res => {
      this.lineTypes = res.map(item => {
        return { id: item.key, text: item.value };
      });
    });
  }

  getStage() {
    this.rolloutService.getStage(this.search.factory).subscribe(res => {
      this.stages = res.map(item => {
        return { id: item.key, text: item.value };
      });
    });
  }

  changeFactory() {
    this.search.lineID = '';
    this.search.lineTypeID = '';
    this.search.modelNo = '';
    this.search.stageID = '';

    this.auditService.currentParamSearch.subscribe(res => {
      if (res != null) {
        this.search = res;
        this.pagination.currentPage = res.currentPage;
        this.loadData();
        this.countRolloutTotal = res.countRolloutTotal;
        this.countBeingAudit = res.countBeingAudit;
      }
    }).unsubscribe();
    this.getLineNo();
    this.getLineType();
    this.getStage();
  }

  searchQuery() {
    this.pagination.currentPage = 1;
    this.snotifySearch = true;
    this.loadData();
  }

  btnDetail(item: BLAuditReportDTO) {
    this.search.countRolloutTotal = this.countRolloutTotal;
    this.search.countBeingAudit = this.countBeingAudit;
    this.search.currentPage = this.pagination.currentPage;
    this.auditService.setParamSearch(this.search);
    this.detailParam = {
      factory: item.factory_id,
      lineID: item.line_id,
      rolloutLineID: item.rollout_line_id,
      lineTypeID: item.line_type_id,
      modelNo: item.model_no,
      stageID: item.stage_id,
      operationID: item.operation_id,
      lang: this.lang
    } as BLSearchAuditReportParam;
    this.auditService.setDetail(this.detailParam);
    this.router.navigate(['/best-line/report/audit-report/audit-report-detail']);
  }

  clear() {
    this.search = {
      factory: localStorage.getItem('factorySmartTooling'),
      lineID: '',
      rolloutLineID: '',
      lineTypeID: '',
      modelNo: '',
      stageID: '',
      operationID: ''
    } as BLSearchAuditReportParam;
    this.pagination = {
      currentPage: 1,
      itemsPerPage: 10
    } as Pagination;
    this.auditReports = [];
    this.getLineNo();
    this.getLineType();
    this.getStage();
    this.countRolloutTotal = 0
    this.countBeingAudit = 0;
    // this.snotify.success(MessageConstants.CLEAR, CaptionConstants.SUCCESS);
  }

  pageChanged(event: any) {
    this.pagination.currentPage = event.page;
    this.loadData();
  }

}
