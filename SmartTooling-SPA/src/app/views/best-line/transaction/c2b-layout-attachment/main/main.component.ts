import { NgSnotifyService } from "./../../../../../_core/_services/ng-snotify.service";
import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { Select2OptionData } from "ng-select2";
import { NgxSpinnerService } from "ngx-spinner";
import { Pagination } from "../../../../../_core/_models/smart-tool/pagination";
import { C2BLayoutAttachmentService } from "../../../../../_core/_services/best-line/c2b-layout-attachment.service";
import {
  ActionConstants,
  CaptionConstants,
  MessageConstants,
} from "../../../../../_core/_constants/system.constants";
import { environment } from "../../../../../../environments/environment";
import { C2bLayoutAttachmentParam } from "../../../../../_core/_helpers/params/best-line/c2b-layout-attachment-param";
import { C2bLayoutAttachmentResult } from "../../../../../_core/_models/best-line/c2b_layout_attachment_result";
@Component({
  selector: "app-main",
  templateUrl: "./main.component.html",
  styleUrls: ["./main.component.css"],
})
export class MainComponent implements OnInit {
  urlFile = environment.imageUrl;
  dataMain: C2bLayoutAttachmentResult[] = [];
  lineNoList: Array<Select2OptionData> = [];
  lineTypeList: Array<Select2OptionData> = [];
  prodSeasonList: Array<Select2OptionData> = [];
  lineIdTemp: string = '';
  lineTypeIdTemp: string = '';
  optionsSelectLineNo = {
    placeholder: "Select Line No...",
    allowClear: true,
    width: "100%",
  };
  optionsSelectLineType = {
    placeholder: "Select Line Type...",
    allowClear: true,
    width: "100%",
  };
  optionsSelectprodSeason = {
    allowClear: true,
    placeholder: "Select Prod Season...",
    width: "100%",
  };
  paramSearch: C2bLayoutAttachmentParam = {
    line_name: '',
    line_type_name: '',
    model: '',
    prod_season: ''
  };
  pagination: Pagination = <Pagination>{
    currentPage: 1,
    itemsPerPage: 10,
  };
  message = MessageConstants;
  constructor(
    private service: C2BLayoutAttachmentService,
    private router: Router,
    private spinnerService: NgxSpinnerService,
    private snotify: NgSnotifyService
  ) { }
  ngOnInit() {
    this.getAllLineNo();
    this.getAllLineType();
    this.getAllProdSeason();
    this.getData();
  }

  openFile(file) {
    if (file !== null && file !== '') {
      window.open(this.urlFile + file, '_blank');
    }
  }

  search() {
    this.pagination.currentPage == 1 ? this.getData() : this.pagination.currentPage = 1;
  }

  getData() {
    this.service.search(this.pagination, this.paramSearch).subscribe({
      next: (res) => {
        this.pagination = res.pagination
        this.dataMain = res.result
        this.spinnerService.hide();
      },
      error: () => {
        this.spinnerService.hide();
        this.snotify.error(MessageConstants.SYSTEM_ERROR_MSG, CaptionConstants.ERROR);
      }
    })
  }

  add() {
    this.router.navigate([
      "/best-line/transaction/layout-attachment/add"
    ]);
  }

  clear() {
    this.paramSearch = {
      line_name: '',
      line_type_name: '',
      model: '',
      prod_season: ''
    };
    this.dataMain = [];
    this.getData();
  }

  pageChanged(e: any): void {
    this.pagination.currentPage = e.page;
    this.getData();
  }

  getAllLineNo() {
    this.service.getAllLineNo().subscribe((res) => {
      this.lineNoList = res.map((item) => ({
        id: item.line_name,
        text: item.line_name
      }));
    });
  }

  getAllLineType() {
    this.service.getAllLineType().subscribe((res) => {
      this.lineTypeList = res.map((item) => ({
        id: item.line_type_name,
        text: item.line_type_name,
      }));
    });
  }

  getAllProdSeason() {
    this.service.getAllProdSeason().subscribe({
      next: (res) => {
        this.prodSeasonList = res.map((item) => ({
          id: item.prod_season,
          text: item.prod_season
        }))
        this.spinnerService.hide();
      },
      error: () => {
        this.spinnerService.hide();
      }
    })
  }

  delete(attachment_file_url: string) {
    this.snotify.confirm(
      MessageConstants.CONFIRM_DELETE,
      ActionConstants.DELETE,
      () => {
        this.service.delete(attachment_file_url).subscribe(
          () => {
            this.search();
            return this.snotify.success(
              MessageConstants.DELETED_OK_MSG,
              ActionConstants.DELETE
            );
          },
          (error) => {
            this.snotify.error(
              MessageConstants.DELETED_ERROR_MSG,
              ActionConstants.DELETE
            );
          }
        );
      }
    );
  }

}
