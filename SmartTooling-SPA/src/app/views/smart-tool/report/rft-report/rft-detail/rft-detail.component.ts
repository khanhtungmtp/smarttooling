import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { NgxSpinnerService } from "ngx-spinner";
import { RFTReportDetail } from "../../../../../_core/_models/smart-tool/rft-report-detail";
import { AuthService } from "../../../../../_core/_services/auth.service";
import { NgSnotifyService } from "../../../../../_core/_services/ng-snotify.service";
import { RFTReportService } from "../../../../../_core/_services/smart-tool/rft-report.service";


@Component({
  selector: "app-rft-detail",
  templateUrl: "./rft-detail.component.html",
  styleUrls: ["./rft-detail.component.css"],
})
export class RftDetailComponent implements OnInit {
  rftreportdetails: RFTReportDetail[];
  model: any = {};
  avgdata: any = null;
  cr2: string = null;
  sms: string = null;
  cs1: string = null;
  cs2: string = null;
  cs3: string = null;
  prod1: string = null;
  prod2: string = null;
  mp1: string = null;
  mp2: string = null;
  mp3: string = null;
  totalavg: string = null;
  // modelno: string;
  // pagination: Pagination = {
  //   currentPage: 1,
  //   itemsPerPage: 999, // 調整
  //   totalItems: 1,
  //   totalPages: 1,
  // };
  constructor(
    private rftReportService: RFTReportService,
    private snotifyAlert: NgSnotifyService,
    private authService: AuthService,
    private router: Router,
    private spinner: NgxSpinnerService
  ) {}

  ngOnInit() {
    this.spinner.show();
    // call by value(from model)
    this.rftReportService.currentModel.subscribe(
      (models) => (this.model = models)
    );
    console.log(this.model);
    if (this.model !== undefined || this.model !== null) {
      // console.log(this.model.model_no);
      this.getRFTReportDetail();
      this.getAVG();
    }
    this.spinner.hide();
  }

  // with page
  // getRFTReportDetail() {
  //   this.rftReportService
  //     .searchRFTReportDetail(
  //       this.pagination.currentPage,
  //       this.pagination.itemsPerPage,
  //       this.model
  //     )
  //     .subscribe(
  //       (res: PaginatedResult<RFTReportDetail[]>) => {
  //         this.rftreportdetails = res.result;
  //         // console.log(this.rftreportdetails);
  //         this.pagination = res.pagination;
  //         this.spinner.hide();
  //       },
  //       (error) => {
  //         this.alertify.error(error);
  //       }
  //     );
  // }

  getRFTReportDetail() {
    this.rftReportService.searchRFTReportDetail(this.model).subscribe((res) => {
      this.rftreportdetails = res;
      // console.log(res);
    });
  }

  getAVG() {
    this.rftReportService
      .getAverage(this.model.factory_id, this.model.model_no)
      .subscribe((res) => {
        this.avgdata = res;
        // console.log("AVG :", this.avgdata);
        // console.log("totalavg :", this.avgdata.totalavg);
        this.cr2 = this.avgdata.rftavg.cR2;
        this.sms = this.avgdata.rftavg.sms;
        this.cs1 = this.avgdata.rftavg.cS1;
        this.cs2 = this.avgdata.rftavg.cS2;
        this.cs3 = this.avgdata.rftavg.cS3;
        this.prod1 = this.avgdata.rftavg.proD1;
        this.prod2 = this.avgdata.rftavg.proD2;
        this.mp1 = this.avgdata.rftavg.mP1;
        this.mp2 = this.avgdata.rftavg.mP2;
        this.mp3 = this.avgdata.rftavg.mP3;
        this.totalavg = this.avgdata.totalavg.toFixed(2);
      });
  }

  backList() {
    this.router.navigate(["./report/rft-report/main"]);
  }
}
