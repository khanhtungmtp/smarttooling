import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { Select2OptionData } from 'ng-select2';
import { NgxSpinnerService } from 'ngx-spinner';
import { CaptionConstants, MessageConstants } from '../../../../../_core/_constants/system.constants';
import { PBPPadPrintSettingParams } from '../../../../../_core/_helpers/params/production-bp/pbp-pad-print-setting-params';
import { Pagination } from '../../../../../_core/_models/production-bp/pagination-production-bp';
import { PbpPadPrintSetting } from '../../../../../_core/_models/production-bp/pbp-pad-print-setting';
import { NgSnotifyService } from '../../../../../_core/_services/ng-snotify.service';
import { PadPrintSettingReportService } from '../../../../../_core/_services/production-bp/pad-print-setting-report.service';


@UntilDestroy()
@Component({
  selector: 'app-pad-print-setting-report-main',
  templateUrl: './pad-print-setting-report-main.component.html',
  styleUrls: ['./pad-print-setting-report-main.component.scss']
})
export class PadPrintSettingReportMainComponent implements OnInit {

  factory: string = localStorage.getItem('factorySmartTooling');
  isSuccess: number = 0;
  pbpPadPrintSettingData: PbpPadPrintSetting[] = [];
  searchParam: PBPPadPrintSettingParams = <PBPPadPrintSettingParams>{
    factory_id: this.factory,
    dev_season: '',
    model: '',
    production_season: ''
  };
  pagination: Pagination = {
    currentPage: 1,
    pageSize: 10,
    totalCount: 0,
    totalPage: 0,
  };

  factories: Array<Select2OptionData> = [];
  message = MessageConstants;
  constructor(
    private PadPrintService: PadPrintSettingReportService,
    private snotify: NgSnotifyService,
    private router: Router,
    private spinnerService: NgxSpinnerService,
  ) { }

  async ngOnInit() {
    await this.getFactory();
    //await this.getData();
  }

  async search() {
    this.pagination.currentPage = 1;
    await this.getData();
    if (this.isSuccess == 0) {
      this.snotify.success(MessageConstants.QUERY_SUCCESS, CaptionConstants.SUCCESS);
    }
  }

  detail(item: PbpPadPrintSetting) {
    this.PadPrintService.changePBPPadPrintSetting(item)
    this.router.navigate(['production-bp/report/pad-print-setting-report/detail']);
  }

  async getFactory() {
    let data = await this.PadPrintService.getAllFactory().toPromise();
    this.factories = data.map(item => ({ id: item.key, text: item.value }));
    this.factories.unshift({ id: " ", text: "All" });
  }

  async changeFactory() {
    this.reset();
    this.PadPrintService.currentParamSearch.subscribe(res => {
      if (res) {
        this.searchParam = res;
      }
    }).unsubscribe();
    this.PadPrintService.currentPage.subscribe(res => {
      if (res) {
        this.pagination = res;
      }
    }).unsubscribe();
    await this.getData();
  }

  async getData() {
    this.spinnerService.show();
    await this.PadPrintService.changeParamSearch(this.searchParam);
    await this.PadPrintService.changePage(this.pagination);
    await this.PadPrintService.search(this.pagination, this.searchParam).pipe(untilDestroyed(this)).subscribe(res => {
      if (res) {
        this.pbpPadPrintSettingData = res.result;
        this.pagination = res.pagination;
        this.isSuccess = 0;
      }
      else {
        this.isSuccess = 1;
      }
      this.spinnerService.hide();
    })

  }

  async pageChanged(event: any) {
    this.pagination.currentPage = event.page;
    await this.getData();
  }

  async clear() {
    this.reset();
    this.searchParam.factory_id = this.factory;;
    await this.getData();
    this.snotify.success(MessageConstants.CLEAR, CaptionConstants.SUCCESS);
  }

  reset() {
    this.searchParam.dev_season = '';
    this.searchParam.production_season = '';
    this.searchParam.model = '';
    this.pagination = {
      currentPage: 1,
      pageSize: 10,
      totalCount: 0,
      totalPage: 0,
    };
  }

  exportExcel() {
    this.spinnerService.show();
    this.PadPrintService.exportExcel(this.pagination, this.searchParam).subscribe({
      next: (result: Blob) => {
        if (result.size === 0) {
          this.spinnerService.hide();
          return this.snotify.error(MessageConstants.NO_DATA, CaptionConstants.ERROR);
        } else if (result.type !== "application/xlsx") {
          this.spinnerService.hide();
          return this.snotify.error(MessageConstants.SYSTEM_ERROR_MSG, CaptionConstants.ERROR);
        }
        let blob = new Blob([result]);
        let url = window.URL.createObjectURL(blob);
        let link = document.createElement("a");
        let currentTime = new Date();
        const fileName =
          "Pad_Print_Setting_Report_" +
          currentTime.getFullYear().toString() +
          (currentTime.getMonth() + 1) +
          currentTime.getDate() +
          ".xlsx";
        link.href = url;
        link.setAttribute("download", fileName);
        document.body.appendChild(link);
        link.click();
      },
      error: () => {
        this.spinnerService.hide(),
        this.snotify.error(MessageConstants.UN_KNOWN_ERROR, CaptionConstants.ERROR)
      },
      complete: () => this.spinnerService.hide()
    })
  }
}
