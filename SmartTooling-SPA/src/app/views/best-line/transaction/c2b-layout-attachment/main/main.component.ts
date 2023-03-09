import { NgSnotifyService } from "./../../../../../_core/_services/ng-snotify.service";
import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { Select2OptionData } from "ng-select2";
import { NgxSpinnerService } from "ngx-spinner";
import { BLAttachments } from "../../../../../_core/_models/best-line/bl_attachments";
import { BLAttachmentsDetail } from "../../../../../_core/_models/best-line/bl_attachments_detail";
import { Pagination } from "../../../../../_core/_models/smart-tool/pagination";
import { C2BLayoutAttachmentService } from "../../../../../_core/_services/best-line/c2b-layout-attachment.service";
import { C2BLayoutAttachmentParam } from "../../../../../_core/_models/best-line/bl_attachments_param";
import {
  ActionConstants,
  CaptionConstants,
  MessageConstants,
} from "../../../../../_core/_constants/system.constants";
import { environment } from "../../../../../../environments/environment";
@Component({
  selector: "app-main",
  templateUrl: "./main.component.html",
  styleUrls: ["./main.component.css"],
})
export class MainComponent implements OnInit {
  urlFile = environment.imageUrl;
  data: BLAttachments[] = [];
  lineNoList: Array<Select2OptionData> = [];
  lineTypeList: Array<Select2OptionData> = [];
  modelNoList: Array<Select2OptionData> = [];
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
  optionsSelectModelNo = {
    placeholder: "Select Model No...",
    allowClear: true,
    width: "100%",
  };
  paramSearch: C2BLayoutAttachmentParam = {
    line_no: "",
    line_type: "",
    modelNo: "",
    modelName: "",
  };
  pagination: Pagination = {
    currentPage: 1,
    itemsPerPage: 10,
    totalItems: 0,
    totalPages: 0,
  };
  isLineTypeDisable: boolean = true;
  isModelNoDisable: boolean = true;
  checkNotifySearch: boolean = false;
  message = MessageConstants;
  constructor(
    private service: C2BLayoutAttachmentService,
    private router: Router,
    private spinnerService: NgxSpinnerService,
    private snotify: NgSnotifyService
  ) { }
  ngOnInit() {
    this.service.currentParamSearch.subscribe(res => {
      if (res != null) {
        this.paramSearch.line_no = this.lineIdTemp = res.lineID;
        this.paramSearch.line_type = this.lineTypeIdTemp = res.lineTypeID;
        this.paramSearch.modelNo = res.modelNo;
        this.paramSearch.modelName = res.modelName;
        this.search(1)
      }
      else {
        [this.paramSearch.line_no, this.paramSearch.line_type, this.paramSearch.modelNo, this.paramSearch.modelName] = ['', '', '', ''];
      }
    }).unsubscribe();
    this.getAllLineNo();
  }

  openFile(file) {
    if (file !== null && file !== '') {
      window.open(this.urlFile + file, '_blank');
    }
  }
  
  search(check: number) {
    if (
      this.paramSearch.line_no === "" ||
      this.paramSearch.line_type === "" ||
      this.paramSearch.modelNo === ""
    ) {
      return this.snotify.warning(MessageConstants.SELECT_ALL_QUERY_OPTION, CaptionConstants.WARNING);
    }
    this.checkNotifySearch = true;
    this.spinnerService.show();
    this.service.search(this.pagination, this.paramSearch).subscribe((res) => {
      this.spinnerService.hide();
      this.pagination = res.pagination;
      this.data = res.result;
      if (this.checkNotifySearch) {
        this.spinnerService.hide();
        if(check === 0){
          this.snotify.success(MessageConstants.QUERY_SUCCESS, CaptionConstants.SUCCESS);
        }
        this.checkNotifySearch = false;
      }
    }, (error) => {
      this.spinnerService.hide();
      this.snotify.error(MessageConstants.SYSTEM_ERROR_MSG, CaptionConstants.ERROR);
    }
    );
  }
  add() {
    this.service.changeSearchSource({
      lineID: this.paramSearch.line_no,
      lineTypeID: this.paramSearch.line_type,
      modelNo: this.paramSearch.modelNo,
      modelName: this.paramSearch.modelName
    });
    this.router.navigate([
      "/best-line/transaction/layout-attachment/add"
    ]);
  }
  clear() {
    this.paramSearch = {
      line_no: "",
      line_type: "",
      modelNo: "",
      modelName: "",
    };
    this.data = [];
    this.isLineTypeDisable = true;
    this.isModelNoDisable = true;
    this.snotify.success(MessageConstants.CLEAR, CaptionConstants.SUCCESS);
  }

  pageChanged(e: any): void {
    this.pagination.currentPage = e.page;
    this.search(1);
  }

  getAllLineNo() {
    this.service.getAllLineNo().subscribe((res) => {
      this.lineNoList = res.map((item) => ({ id: item.key, text: item.value }));
    });
  }

  getAllLineType(lineId) {
    this.service.getAllLineType(lineId).subscribe((res) => {
      this.lineTypeList = res.map((item) => ({
        id: item.key,
        text: item.value,
      }));
    });
  }

  getAllModelNo(lineId, lineTypeId) {
    this.service.getAllModelNo(lineId, lineTypeId).subscribe((res) => {
      this.modelNoList = res.map((item) => ({
        id: item.key,
        text: item.key,
        additional: item.value,
      }));
    });
  }

  delete(item: BLAttachmentsDetail) {
    this.snotify.confirm(
      MessageConstants.CONFIRM_DELETE,
      ActionConstants.DELETE,
      () => {
        this.service.delete(item).subscribe(
          () => {
            this.search(1);
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

  changeLineNo(e) {
    this.spinnerService.show();
    this.paramSearch.line_no = e;
    if(this.paramSearch.line_no != this.lineIdTemp){
      this.paramSearch.line_type = "";
      this.paramSearch.modelName = "";
      this.lineTypeList = [];
      this.modelNoList = [];
      this.lineIdTemp = this.paramSearch.line_no;
    }
    if (this.paramSearch.line_no === "") {
      this.isLineTypeDisable = true;
      this.isModelNoDisable = true;
    } else {
      this.getAllLineType(e);
      this.isLineTypeDisable = false;
    }
    this.spinnerService.hide();
  }

  changeLineType(e) {
    this.spinnerService.show();
    if (e === "") {
      this.isModelNoDisable = true;
    } else {
      this.paramSearch.line_type = e;
      if(this.paramSearch.line_type != this.lineTypeIdTemp){
        this.paramSearch.modelNo = '';
        this.paramSearch.modelName = '';
        this.lineTypeIdTemp = this.paramSearch.line_type
      }
      const lineTypeId = this.lineTypeList.find((x) => x.id === e).id;
      this.getAllModelNo(this.paramSearch.line_no, lineTypeId);
      this.isModelNoDisable = false;
    }
    this.spinnerService.hide();
  }
  changeModelNo(e) {
    this.spinnerService.show();
    this.paramSearch.modelName = "";
    if (e !== "") {
      this.paramSearch.modelName = this.modelNoList?.find(
        (x) => x.id === e
      )?.additional;
    }
    this.spinnerService.hide();
  }
}
