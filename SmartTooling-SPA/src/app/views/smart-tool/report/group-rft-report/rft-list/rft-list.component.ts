import { Component, OnInit } from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";
import { NgxSpinnerService } from "ngx-spinner";
import { CaptionConstants } from "../../../../../_core/_constants/system.constants";
import { Factory } from "../../../../../_core/_models/smart-tool/factory";
import { Pagination, PaginatedResult } from "../../../../../_core/_models/smart-tool/pagination";
import { RFTReport } from "../../../../../_core/_models/smart-tool/rft-report";
import { NgSnotifyService } from "../../../../../_core/_services/ng-snotify.service";
import { RFTReportService } from "../../../../../_core/_services/smart-tool/rft-report.service";


@Component({
  selector: "app-rft-list",
  templateUrl: "./rft-list.component.html",
  styleUrls: ["./rft-list.component.css"],
})
export class RftListComponent implements OnInit {
  rftreports: RFTReport[] = [];
  // rftreport: any = {};
  // API helper(query condition)
  paramSearch: any = {};
  model_no: string = "";
  factories: Factory[];
  factory: string;
  pagination: Pagination = {
    currentPage: 1,
    itemsPerPage: 10,
    totalItems: 1,
    totalPages: 1,
  };
  // searchKey = false;
  constructor(
    private rftreportService: RFTReportService,
    private snotifyAlert: NgSnotifyService,
    private router: Router,
    private route: ActivatedRoute,
    private spinner: NgxSpinnerService
  ) {}

  ngOnInit() {
    this.rftreportService.paramSearchSource.asObservable().subscribe(res => {
      if(res !== null) {
        this.factory = res.factory_id;
        this.model_no = res.model_No;
        this.loadrftreports();
      }
    });
    this.getAllFactory();
  }

  getAllFactory() {
    this.rftreportService.getAllFactory().subscribe((res) => {
      this.factories = res;
    });
  }
  // load data, call api
  loadrftreports() {
    if (this.factory === undefined || this.factory === null) {
      this.snotifyAlert.error("Please option factory", CaptionConstants.ERROR);
    } else {
      this.paramSearch = {
        factory_id: this.factory,
        model_No: this.model_no.toUpperCase(),
      };
      this.spinner.show();
      // console.log("paramSearch: ", this.paramSearch);
      this.rftreportService
        .searchRFTReport(
          this.pagination.currentPage,
          this.pagination.itemsPerPage,
          this.paramSearch
        )
        .subscribe(
          (res: PaginatedResult<RFTReport[]>) => {
            this.rftreports = res.result;
            this.pagination = res.pagination;
            this.spinner.hide();
          },
          (error) => {
            this.snotifyAlert.error(error, CaptionConstants.ERROR);
          }
        );
    }
  }

  search() {
    this.spinner.show();
    this.pagination.currentPage = 1;
    // console.log("paramSearch: ", this.paramSearch);
    this.loadrftreports();
    this.spinner.hide();
  }

  // rft report detail
  detailRFTReport(model: RFTReport) {
    let paramSearch = {
      factory_id: this.factory,
      model_No: this.model_no,
    };
    this.rftreportService.paramSearchSource.next(paramSearch);
    this.rftreportService.sendmodel(model);
    this.router.navigate(["/report/group-rft-report/rft-detail"]);
  }

  // clear inputbox
  clearSearch() {
    this.rftreports.length = 0;
    this.model_no = "";
    this.factory = null;
    this.rftreportService.paramSearchSource.next(null);
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadrftreports();
  }
}
