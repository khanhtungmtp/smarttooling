import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Select2OptionData } from 'ng-select2';
import { NgxSpinnerService } from 'ngx-spinner';
import { of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { CaptionConstants, MessageConstants } from '../../../../../_core/_constants/system.constants';
import { BondingReportParam } from '../../../../../_core/_helpers/params/production-bp/bonding-report-param';
import { Pagination } from '../../../../../_core/_models/best-line/pagination-best-line';
import { PBPBondingProgramSetting } from '../../../../../_core/_models/production-bp/pbp-bonding-program-setting';
import { NgSnotifyService } from '../../../../../_core/_services/ng-snotify.service';
import { BondingReportService } from '../../../../../_core/_services/production-bp/bonding-report.service';

@Component({
  selector: 'app-bonding-report-main',
  templateUrl: './bonding-report-main.component.html',
  styleUrls: ['./bonding-report-main.component.css']
})
export class BondingReportMainComponent implements OnInit {

  factories: Array<Select2OptionData> = [];
  processTypeList: Array<Select2OptionData> = [];
  autoTechList: Array<Select2OptionData> = [];
  isSuccess: boolean = false;
  pagination: Pagination = {
    currentPage: 1,
    pageSize: 10,
    totalCount: 0,
    totalPage: 0,
  };
  factory: string = localStorage.getItem('factorySmartTooling');
  searchParam: BondingReportParam = <BondingReportParam>{
    factory: this.factory,
    prod_season: '',
    model: '',
    chemical_process_type_id: '',
    auto_tech_id: ''
  }
  noImage: string = "../../../../../../assets/img/no-image.jpg";
  dataBondingProgramSetting: PBPBondingProgramSetting[] = [];
  message = MessageConstants;
  random: number = Math.random();
  constructor(
    private snotify: NgSnotifyService,
    private router: Router,
    private spinnerService: NgxSpinnerService,
    private bondingReportService: BondingReportService,
  ) { }

  async ngOnInit() {
    await this.getAllFactory();
  }
  async getAllFactory() {
    let data = await this.bondingReportService.getAllFactory().toPromise();
    this.factories = data.map(item => ({ id: item.key, text: item.value }));
    this.factories.unshift({ id: " ", text: "All" });
  }
  async getProcessType() {
    let data = await this.bondingReportService.getProcessType(this.searchParam.factory).toPromise();
    this.processTypeList = data.map(item => ({ id: item.key, text: item.value }));
  }
  async GetAutoTech() {
    let data = await this.bondingReportService.getAutoTech(this.searchParam.factory).toPromise();
    this.autoTechList = data.map(item => ({ id: item.key, text: item.value }));
  }
  async search() {
    this.pagination.currentPage = 1;
    await this.getData();
    if (this.isSuccess)
      this.snotify.success(MessageConstants.QUERY_SUCCESS, CaptionConstants.SUCCESS);
  }
  async getData() {
    this.spinnerService.show();
    await this.bondingReportService.changeParamSearch(this.searchParam);
    await this.bondingReportService.changePage(this.pagination);
    let res = await this.bondingReportService.getData(this.searchParam, this.pagination).pipe(catchError(e => {
      this.snotify.error(MessageConstants.SYSTEM_ERROR_MSG, CaptionConstants.ERROR);
      return of(null);
    })).toPromise();
    if (res) {
      this.dataBondingProgramSetting = res.result;
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
  detail(item: PBPBondingProgramSetting) {
    this.bondingReportService.changeModel(item);
    this.router.navigate(['production-bp/report/bonding-program-report/detail']);
  }
  async changeFactory() {
    this.reset();
    this.bondingReportService.currentParamSearch.subscribe(res => {
      if (res) {
        this.searchParam = res;
      }
    }).unsubscribe();
    this.bondingReportService.currentPage.subscribe(res => {
      if (res) {
        this.pagination = res;
      }
    }).unsubscribe();
    await this.getProcessType();
    await this.GetAutoTech();
    await this.getData();
  }
  async clear() {
    this.reset();
    this.searchParam.factory = this.factory;
    await this.getProcessType();
    await this.GetAutoTech();
    await this.getData();
    this.snotify.success(MessageConstants.CLEAR, CaptionConstants.SUCCESS);
  }
  reset() {
    this.searchParam.auto_tech_id = '';
    this.searchParam.chemical_process_type_id = '';
    this.searchParam.model = '';
    this.searchParam.prod_season = '';
    this.pagination = {
      currentPage: 1,
      pageSize: 10,
      totalCount: 0,
      totalPage: 0,
    }
  }

  exportExcel() {
    this.spinnerService.show();
    this.bondingReportService.exportExcel(this.searchParam, this.pagination).subscribe({
      next: (result: Blob) => {
        if (result.size === 0) {
          this.spinnerService.hide();
          return this.snotify.error(MessageConstants.NO_DATA, CaptionConstants.ERROR);
        } else if (result.type !== "application/xlsx") {
          this.spinnerService.hide();
          return this.snotify.error(MessageConstants.SYSTEM_ERROR_MSG, CaptionConstants.ERROR);
        }
        const blob = new Blob([result]);
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement("a");
        const currentTime = new Date();
        const fileName =
          "Bonding_Program_Setting_Report_" +
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
    });
  }
}
