import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { Select2OptionData } from "ng-select2";
import { NgxSpinnerService } from "ngx-spinner";
import { CaptionConstants } from "../../../../../_core/_constants/system.constants";
import { DefectReason } from "../../../../../_core/_models/smart-tool/defect-reason";
import { Pagination, PaginatedResult } from "../../../../../_core/_models/smart-tool/pagination";
import { NgSnotifyService } from "../../../../../_core/_services/ng-snotify.service";
import { DefectReasonService } from "../../../../../_core/_services/smart-tool/defect-reason.service";


@Component({
  selector: "app-defect-reason-list",
  templateUrl: "./defect-reason-list.component.html",
  styleUrls: ["./defect-reason-list.component.scss"],
})
export class DefectReasonListComponent implements OnInit {
  defectreasons: DefectReason[];
  defectreason: any = {};
  activeList: Array<Select2OptionData>;
  paramSearch: any = {}; // API helper(query condition)
  pagination: Pagination = {
    currentPage: 1,
    itemsPerPage: 10,
    totalItems: 1,
    totalPages: 1,
  };
  // searchKey = false;
  constructor(
    private defectreasonService: DefectReasonService,
    private snotifyAlert: NgSnotifyService,
    private router: Router,
    private spinner: NgxSpinnerService
  ) {}

  ngOnInit() {
    this.spinner.show();
    this.defectreasonService.currentDefectReasonSearch.subscribe(res => {
      if(res === null) {
        this.paramSearch.active = "all";
        this.paramSearch.defect_Reason = "";
      } else {
        this.paramSearch = res;
      }
    })
    this.loadDefectReasons();
    this.spinner.hide();
  }

  // load data
  loadDefectReasons() {
    this.defectreasonService.changeDefectReasonSearch(this.paramSearch);
    this.defectreasonService
      .search(
        this.pagination.currentPage,
        this.pagination.itemsPerPage,
        this.paramSearch
      )
      .subscribe(
        (res: PaginatedResult<DefectReason[]>) => {
          this.defectreasons = res.result;
          this.pagination = res.pagination;
          this.spinner.hide();
        },
        (error) => {
          this.snotifyAlert.error(error, CaptionConstants.ERROR);
        }
      );
  }

  search() {
    this.spinner.show();
    this.pagination.currentPage = 1;
    // console.log("defect search", this.paramSearch.defect_Reason_ID);
    // console.log("active", this.paramSearch.active);
    this.loadDefectReasons();
    this.spinner.hide();
  }

  // open add page
  addDefectReason() {
    this.defectreason = {};
    this.defectreasonService.changeDefectReason(this.defectreason);
    this.defectreasonService.changeFlag("0");
    // console.log(this.defectreasonService.flagSource.value);
    this.router.navigate(["/defect-reason/add"]);
  }

  // open edit page
  changeToEdit(defactreason: DefectReason) {
    this.defectreasonService.changeDefectReason(defactreason);
    this.defectreasonService.changeFlag("1");
    // console.log(this.defectreasonService.flagSource.value);
    this.router.navigate(["/defect-reason/edit"]);
  }

  // clear inputbox
  clearSearch() {
    // bind API helper
    this.paramSearch.active = "all";
    this.paramSearch.defect_Reason = "";
    this.loadDefectReasons();
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadDefectReasons();
  }
}
