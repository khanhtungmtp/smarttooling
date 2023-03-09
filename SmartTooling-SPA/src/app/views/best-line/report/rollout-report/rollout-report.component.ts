import { MessageConstants, CaptionConstants } from './../../../../_core/_constants/system.constants';
import { environment } from './../../../../../environments/environment';
import { Select2OptionData } from 'ng-select2';
import { BLRolloutReportModel } from './../../../../_core/_models/best-line/bl-rollout-report.model';
import { Component, OnInit } from '@angular/core';
import { Pagination } from '../../../../_core/_models/smart-tool/pagination';
import { BlRolloutReportService } from '../../../../_core/_services/best-line/bl-rollout-report.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { NgSnotifyService } from '../../../../_core/_services/ng-snotify.service';
import { BLSearchRolloutReportParam } from '../../../../_core/_helpers/params/best-line/bl-search-rollout-report.param';

@Component({
  selector: 'app-rollout-report',
  templateUrl: './rollout-report.component.html',
  styleUrls: ['./rollout-report.component.scss']
})
export class RolloutReportComponent implements OnInit {
  rollouts: BLRolloutReportModel[] = [];
  factories: Array<Select2OptionData> = [];
  lineNos: Array<Select2OptionData> = [];
  lineTypes: Array<Select2OptionData> = [];
  stages: Array<Select2OptionData> = [];
  lang: string = 'en';
  snotifySearch: boolean = false;
  random: number = Math.random();
  search: BLSearchRolloutReportParam = {
    factory: localStorage.getItem('factorySmartTooling'),
    lineID: '',
    lineTypeID: '',
    modelNo: '',
    stageID: ''
  } as BLSearchRolloutReportParam;

  pagination: Pagination = {
    currentPage: 1,
    itemsPerPage: 10
  } as Pagination;
  message = MessageConstants;
  countRolloutTotal: number = 0;
  countBeingAudit: number = 0;
  constructor(
    private rolloutService: BlRolloutReportService,
    private spinner: NgxSpinnerService,
    private snotify: NgSnotifyService,
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
      this.rolloutService.getAllRollout(this.search, this.pagination)
        .subscribe(res => {
          this.rollouts = res.result;
          this.pagination = res.pagination;
          if(this.rollouts.length > 0) {
            this.countBeingAudit = this.rollouts[0].countBeingAudit;
            this.countRolloutTotal = this.rollouts[0].criticalOperationsTotal;
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
    });
  }

  getLineNo() {
    this.rolloutService.getLineNo(this.search.factory).subscribe(res => {
      this.lineNos = res.map(item => {
        return { id: item.key, text: item.value };
      });
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
    this.getLineNo();
    this.getLineType();
    this.getStage();
    this.search.lineID = '';
    this.search.lineTypeID = '';
    this.search.modelNo = '';
    this.search.stageID = '';
  }

  searchQuery() {
    this.pagination.currentPage = 1;
    this.snotifySearch = true;
    this.loadData();
  }

  clear() {
    this.search = {
      factory: localStorage.getItem('factorySmartTooling'),
      lineID: '',
      lineTypeID: '',
      modelNo: '',
      stageID: ''
    } as BLSearchRolloutReportParam;
    this.pagination = {
      currentPage: 1,
      itemsPerPage: 10
    } as Pagination;
    this.rollouts = [];
    this.getLineNo();
    this.getLineType();
    this.getStage();
    this.countBeingAudit = 0;
    this.countRolloutTotal = 0;
    // this.snotify.success(MessageConstants.CLEAR, CaptionConstants.SUCCESS);
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadData();
  }

}
