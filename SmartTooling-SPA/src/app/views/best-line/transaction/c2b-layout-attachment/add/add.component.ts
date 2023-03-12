import { ChangeDetectorRef, Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { Select2OptionData } from "ng-select2";
import { NgxSpinnerService } from "ngx-spinner";
import {
  ActionConstants,
  CaptionConstants,
  MessageConstants,
} from "../../../../../_core/_constants/system.constants";
import { C2bLayoutAttachmentAddParam } from "../../../../../_core/_helpers/params/best-line/c2b-layout-attachment-param";
import { C2BLayoutAttachmentService } from "../../../../../_core/_services/best-line/c2b-layout-attachment.service";
import { NgSnotifyService } from "../../../../../_core/_services/ng-snotify.service";

@Component({
  selector: "app-add",
  templateUrl: "./add.component.html",
  styleUrls: ["./add.component.scss"],
})
export class AddComponent implements OnInit {
  param: C2bLayoutAttachmentAddParam = <C2bLayoutAttachmentAddParam>{}
  lineNoList: Array<Select2OptionData> = [];
  lineTypeList: Array<Select2OptionData> = [];
  modelNoList: Array<Select2OptionData> = [];
  modelNoAndNameList: Array<Select2OptionData> = [];
  prodSeasonList: Array<Select2OptionData> = [];
  attachmentTypeList: Array<Select2OptionData> = [];
  file: File = null;

  constructor(
    private service: C2BLayoutAttachmentService,
    private router: Router,
    private spinnerService: NgxSpinnerService,
    private snotify: NgSnotifyService,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit() {
    this.getAllLineNoOfAdd();
    this.getAllAttachmentType();
  }

  ngAfterViewChecked() {
    this.cdr.detectChanges();
  }

  back() {
    this.router.navigate(["/best-line/transaction/layout-attachment/main"]);
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
        this.snotify.error(MessageConstants.SYSTEM_ERROR_MSG, CaptionConstants.ERROR);
      }
    })
  }

  getAllLineTypeOfAdd(event: any) {
    this.param.line_id = event
    if (this.param.line_id !== '') {
      this.service.getAllLineTypeOfAdd(this.param.line_id).subscribe({
        next: (res) => {
          this.lineTypeList = res.map(item => ({
            id: item.line_type_id,
            text: item.line_type_name
          }))
          this.spinnerService.hide();
        },
        error: () => {
          this.spinnerService.hide();
          this.snotify.error(MessageConstants.SYSTEM_ERROR_MSG, CaptionConstants.ERROR);
        }
      })
    }
  }

  getAllModelNoOfAdd(event: any) {
    this.param.line_type_id = event
    if (this.param.line_type_id !== '') {
      this.service.getAllModelNoOfAdd(this.param.line_id, this.param.line_type_id).subscribe({
        next: (res) => {
          this.modelNoList = res.map(item => ({
            id: item.model_no,
            text: item.model_no
          }))
          this.modelNoAndNameList = res.map(item => ({
            id: item.model_no,
            text: item.model_name
          }))
          this.spinnerService.hide();
        },
        error: () => {
          this.spinnerService.hide();
          this.snotify.error(MessageConstants.SYSTEM_ERROR_MSG, CaptionConstants.ERROR);
        }
      })
    }
  }

  changeModelName(event: any) {
    this.param.model_name = event ? this.modelNoAndNameList.find(x => x.id == event)?.text : ''
  }

  getAllProdSeasonOfAdd(event: any) {
    this.param.model_no = event
    if (this.param.model_no !== '') {
      this.service.getAllProdSeasonOfAdd(this.param.line_id, this.param.line_type_id, this.param.model_no).subscribe({
        next: (res) => {
          this.prodSeasonList = res.map(item => ({
            id: item.prod_season,
            text: item.prod_season
          }))
          this.spinnerService.hide();
        },
        error: () => {
          this.spinnerService.hide();
          this.snotify.error(MessageConstants.SYSTEM_ERROR_MSG, CaptionConstants.ERROR);
        }
      })
    }
  }

  save(type?: string) {
    this.spinnerService.show();
    if (this.file == null) {
      this.spinnerService.hide();
      return this.snotify.warning(
        MessageConstants.INVALID_FILE,
        CaptionConstants.WARNING
      );
    }
    this.service.create(this.param, this.file).subscribe({
      next: (res) => {
        if (res) {
          this.spinnerService.hide();
          this.snotify.success(
            MessageConstants.CREATED_OK_MSG,
            ActionConstants.CREATE
          );
          if (type == 'next')
            this.resetData()
          else
            this.back();
        } else {
          this.spinnerService.hide();
          return this.snotify.error(
            MessageConstants.CREATED_ERROR_MSG,
            CaptionConstants.ERROR
          );
        }
      },
      error: () => {
        this.spinnerService.hide();
        this.snotify.error(MessageConstants.SYSTEM_ERROR_MSG, CaptionConstants.ERROR);
      }
    })
  }

  getAllAttachmentType() {
    this.service.getAllAttachmentType().subscribe({
      next: (res) => {
        this.attachmentTypeList = res.map((item) => ({
          id: item.key,
          text: item.value,
        }));
      },
      error: () => {
        this.spinnerService.hide();
        this.snotify.error(MessageConstants.SYSTEM_ERROR_MSG, CaptionConstants.ERROR);
      }
    })
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
    } else
      this.file = null
  }

  resetData() {
    this.param.attachment_name = "";
    (<HTMLInputElement>document.getElementById("input_uploadFile")).value =
      null;
    this.file = null;
    this.param.attachment_type_id = "";
  }
}
