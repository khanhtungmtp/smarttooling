import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { Select2OptionData } from "ng-select2";
import { NgxSpinnerService } from "ngx-spinner";
import {
  ActionConstants,
  CaptionConstants,
  MessageConstants,
} from "../../../../../_core/_constants/system.constants";
import { BLAttachmentsDetail } from "../../../../../_core/_models/best-line/bl_attachments_detail";
import { C2BLayoutAttachmentService } from "../../../../../_core/_services/best-line/c2b-layout-attachment.service";
import { NgSnotifyService } from "../../../../../_core/_services/ng-snotify.service";

@Component({
  selector: "app-add",
  templateUrl: "./add.component.html",
  styleUrls: ["./add.component.scss"],
})
export class AddComponent implements OnInit {
  modelName: string = "";
  line_id: string = "";
  line_type_id: string = "";
  modelNo: string = "";
  lineNoList: Array<Select2OptionData> = [];
  lineTypeList: Array<Select2OptionData> = [];
  modelNoList: Array<Select2OptionData> = [];
  prodSeasonList: Array<Select2OptionData> = [];
  attachmentTypeList: Array<Select2OptionData> = [];
  data: BLAttachmentsDetail = {} as BLAttachmentsDetail;
  file: File = null;
  constructor(
    private service: C2BLayoutAttachmentService,
    private router: Router,
    private spinnerService: NgxSpinnerService,
    private snotify: NgSnotifyService,
    private route: ActivatedRoute
  ) { }
  ngOnInit() {
    // this.service.currentParamSearch.subscribe(res => {
    //   if (res != null) {
    //     this.lineNo = res.lineID;
    //     this.lineType = res.lineTypeID;
    //     this.modelNo = res.modelNo;
    //     this.modelName = res.modelName;
    //   }
    //   else {
    //     this.router.navigate(["/best-line/transaction/layout-attachment/main"]);
    //   }
    // }).unsubscribe();
    this.getAllLineNoOfAdd();
    // this.getAllAttachmentType();
  }

  getAllLineNoOfAdd() {
    this.service.getAllLineNoOfAdd().subscribe({
      next: (res) => {
        this.lineNoList = res.map(item => ({
          id: item.line_id,
          text: item.line_name
        }))
        this.spinnerService.hide();
      },
      error: () => {
        this.spinnerService.hide();

      }
    })
  }
  getAllLineTypeOfAdd() {
    this.service.getAllLineTypeOfAdd(this.line_id).subscribe({
      next: (res) => {
        this.lineTypeList = res.map(item => ({
          id: item.line_type_id,
          text: item.line_type_name
        }))
        this.spinnerService.hide();
      },
      error: () => {
        this.spinnerService.hide();

      }
    })
  }
  save() {
    this.spinnerService.show();
    if (this.file == null) {
      return this.snotify.warning(
        MessageConstants.INVALID_FILE,
        CaptionConstants.WARNING
      );
    }
    this.setParam();
    this.service.create(this.data, this.file).subscribe((res) => {
      if (res) {
        this.spinnerService.hide();
        this.snotify.success(
          MessageConstants.CREATED_OK_MSG,
          ActionConstants.CREATE
        );
        this.router.navigate(["/best-line/transaction/layout-attachment/main"]);
      } else {
        this.spinnerService.hide();
        return this.snotify.error(
          MessageConstants.CREATED_ERROR_MSG,
          CaptionConstants.ERROR
        );
      }
    });
  }
  saveAndNext() {
    this.spinnerService.show();
    if (this.file == null) {
      return this.snotify.warning(
        MessageConstants.INVALID_FILE,
        CaptionConstants.WARNING
      );
    }
    this.setParam();
    this.service.create(this.data, this.file).subscribe((res) => {
      if (res) {
        this.spinnerService.hide();
        this.snotify.success(
          MessageConstants.CREATED_OK_MSG,
          ActionConstants.CREATE
        );
        this.resetData();
      } else {
        this.spinnerService.hide();
        return this.snotify.error(
          MessageConstants.FILE_IMAGE_SIZE,
          CaptionConstants.ERROR
        );
      }
    });
  }
  getAllAttachmentType() {
    this.service.getAllAttachmentType().subscribe((res) => {
      this.attachmentTypeList = res.map((item) => ({
        id: item.key,
        text: item.value,
      }));
    });
  }

  setParam() {
    this.data.factory_id = "";
    this.data.line_id = this.lineNo;
    this.data.line_type_id = this.lineType;
    this.data.model_no = this.modelNo;
    this.data.attachment_type_id = this.attachmentTypeList.find(
      (x) => x.id === this.data.attachment_type_id
    ).id;
    this.data.attachment_name = "";
    this.data.attachment_file_url = "";
    this.data.update_by = "";
    this.data.update_time = "";
  }

  onSelectFile(event) {
    if (event.target.files && event.target.files[0]) {
      const reader = new FileReader();
      reader.readAsDataURL(event.target.files[0]); // read file as data url
      const files = event.target.files[0];
      // check file name extension
      const fileNameExtension = event.target.files[0].name.split(".").pop();
      if (
        fileNameExtension !== "xlsx" &&
        fileNameExtension !== "xls" &&
        fileNameExtension !== "pdf"
      ) {
        return this.snotify.warning(
          MessageConstants.INVALID_FILE + "'.xlsx', '.xls' or 'pdf'"
        );
      }
      this.file = files;
    }
  }

  resetData() {
    this.data.attachment_name = "";
    (<HTMLInputElement>document.getElementById("input_uploadFile")).value =
      null;
    this.file = null;
    this.data.attachment_type_id = "";
  }
}
