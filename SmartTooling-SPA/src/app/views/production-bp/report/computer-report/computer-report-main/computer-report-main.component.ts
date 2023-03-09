import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { Select2OptionData } from "ng-select2";
import { NgxSpinnerService } from "ngx-spinner";
import { of } from "rxjs";
import { catchError } from "rxjs/operators";
import {
  CaptionConstants,
  MessageConstants,
} from "../../../../../_core/_constants/system.constants";
import { ComputerReportParam } from "../../../../../_core/_helpers/params/production-bp/computer-report-param";
import { Pagination } from "../../../../../_core/_models/best-line/pagination-best-line";
import { PbpComputerstitchingsetting } from "../../../../../_core/_models/production-bp/pbp-computerstitchingsetting";
import { NgSnotifyService } from "../../../../../_core/_services/ng-snotify.service";
import { ComputerReportService } from "../../../../../_core/_services/production-bp/computer-report.service";

@Component({
  selector: "app-computer-report-main",
  templateUrl: "./computer-report-main.component.html",
  styleUrls: ["./computer-report-main.component.css"],
})
export class ComputerReportMainComponent implements OnInit {
  factories: Array<Select2OptionData> = [];
  isSuccess: boolean = false;
  pagination: Pagination = {
    currentPage: 1,
    pageSize: 10,
    totalCount: 0,
    totalPage: 0,
  };
  factory: string = localStorage.getItem("factorySmartTooling");
  searchParam: ComputerReportParam = {
    factory: this.factory,
    dev_season: "",
    prod_season: "",
    model: "",
    production_adoption: "",
    changeLang: "0",
  } as ComputerReportParam;
  dataComputerstitchingsetting: PbpComputerstitchingsetting[] = [];
  message = MessageConstants;
  noImage: string = "../../../../../../assets/img/no-image.jpg";
  random: number = Math.random();
  constructor(
    private snotify: NgSnotifyService,
    private router: Router,
    private spinnerService: NgxSpinnerService,
    private computerReportService: ComputerReportService
  ) {}

  async ngOnInit() {
    await this.getAllFactory();
  }
  async getAllFactory() {
    let data = await this.computerReportService.getAllFactory().toPromise();
    this.factories = data.map((item) => ({ id: item.key, text: item.value }));
    this.factories.unshift({ id: "All", text: "All" });
  }
  async changeFactory() {
    this.reset();
    this.computerReportService.currentParamSearch
      .subscribe((res) => {
        if (res) {
          this.searchParam = res;
        }
      })
      .unsubscribe();
    this.computerReportService.currentPage
      .subscribe((res) => {
        if (res) {
          this.pagination = res;
        }
      })
      .unsubscribe();
    await this.getData();
  }
  async search() {
    this.pagination.currentPage = 1;
    await this.getData();
    if (this.isSuccess)
      this.snotify.success(
        MessageConstants.QUERY_SUCCESS,
        CaptionConstants.SUCCESS
      );
  }
  async getData() {
    this.spinnerService.show();
    await this.computerReportService.changeParamSearch(this.searchParam);
    await this.computerReportService.changePage(this.pagination);
    let res = await this.computerReportService
      .getData(this.searchParam, this.pagination)
      .pipe(
        catchError((e) => {
          this.snotify.error(
            MessageConstants.SYSTEM_ERROR_MSG,
            CaptionConstants.ERROR
          );
          return of(null);
        })
      )
      .toPromise();
    if (res) {
      this.dataComputerstitchingsetting = res.result;
      this.pagination = res.pagination;
      this.isSuccess = true;
    } else {
      this.isSuccess = false;
    }
    this.spinnerService.hide();
  }
  detail(item: PbpComputerstitchingsetting) {
    this.computerReportService.changeModel(item);
    this.computerReportService.changeParamSearch(this.searchParam);
    this.router.navigate(["production-bp/report/computer-report/detail"]);
  }
  async pageChanged(event) {
    this.pagination.currentPage = event.page;
    await this.getData();
  }
  reset() {
    this.searchParam.dev_season = "";
    this.searchParam.prod_season = "";
    this.searchParam.model = "";
    this.searchParam.production_adoption = "";
    this.searchParam.changeLang = "0";
    this.pagination = {
      currentPage: 1,
      pageSize: 10,
      totalCount: 0,
      totalPage: 0,
    };
  }
  async clear() {
    this.reset();
    this.searchParam.factory = this.factory;
    await this.getData();
    this.snotify.success(MessageConstants.CLEAR, CaptionConstants.SUCCESS);
  }
  exportExcel() {
    this.spinnerService.show();
    debugger;
    this.computerReportService
      .exportExcel(this.pagination, this.searchParam)
      .subscribe({
        next: (result: Blob) => {
          if (result.size === 0) {
            this.snotify.error("No Data");
            this.spinnerService.hide();
            return;
          } else {
            if (result.type !== "xlsx") {
              this.spinnerService.hide();
            }

            const blob = new Blob([result]);
            const url = window.URL.createObjectURL(blob);
            const link = document.createElement("a");
            const currentTime = new Date();
            const fileName =
              "Computer_Stitching_Setting_Report_" +
              currentTime.getFullYear().toString() +
              (currentTime.getMonth() + 1) +
              currentTime.getDate() +
              ".xlsx";
            link.href = url;
            link.setAttribute("download", fileName);
            document.body.appendChild(link);
            link.click();
          }
        },
        error: () => this.snotify.error("Error!"),
        complete: () => this.spinnerService.hide(),
      });
  }
}
