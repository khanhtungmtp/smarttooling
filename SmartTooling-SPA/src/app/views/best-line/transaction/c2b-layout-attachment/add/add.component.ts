import { ChangeDetectorRef, Component, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { Select2OptionData } from "ng-select2";
import { NgxSpinnerService } from "ngx-spinner";
import {
  ActionConstants,
  CaptionConstants,
  MessageConstants,
} from "../../../../../_core/_constants/system.constants";
import { BLAttachmentsDetail } from "../../../../../_core/_models/best-line/bl_attachments_detail";
import { C2bLayoutAttachmentResult } from "../../../../../_core/_models/best-line/c2b_layout_attachment_result";
import { C2BLayoutAttachmentService } from "../../../../../_core/_services/best-line/c2b-layout-attachment.service";
import { NgSnotifyService } from "../../../../../_core/_services/ng-snotify.service";

@Component({
  selector: "app-add",
  templateUrl: "./add.component.html",
  styleUrls: ["./add.component.scss"],
})
export class AddComponent implements OnInit {
  model_name: string = "";
  line_id: string = "";
  line_type_id: string = "";
  model_no: string = "";
  prod_season: string = "";
  attachment_type_id: string = "";
  lineNoList: Array<Select2OptionData> = [];
  lineTypeList: Array<Select2OptionData> = [];
  modelNoList: Array<Select2OptionData> = [];
  modelNoAndNameList: Array<Select2OptionData> = [];
  prodSeasonList: Array<Select2OptionData> = [];
  attachmentTypeList: Array<Select2OptionData> = [];
  data: BLAttachmentsDetail = {} as BLAttachmentsDetail;
  file: File = null;
  constructor(
    private service: C2BLayoutAttachmentService,
    private router: Router,
    private spinnerService: NgxSpinnerService,
    private snotify: NgSnotifyService,
    private route: ActivatedRoute,
    private cdr: ChangeDetectorRef
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
    this.getAllAttachmentType();
  }
  ngAfterViewChecked() {
    this.cdr.detectChanges();
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
  getAllLineTypeOfAdd(event: any) {
    this.line_id = event
    if (this.line_id !== '') {
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
  }
  getAllModelNoOfAdd(event: any) {
    this.line_type_id = event
    if (this.line_type_id !== '') {
      this.service.getAllModelNoOfAdd(this.line_id, this.line_type_id).subscribe({
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

        }
      })
    }
  }
  changeModelName(event: any) {
    this.data.model_name = event ? this.modelNoAndNameList.find(x => x.id == event)?.text : ''

  }
  getAllProdSeasonOfAdd(event: any) {
    this.model_no = event
    if (this.model_no !== '') {
      this.service.getAllProdSeasonOfAdd(this.line_id, this.line_type_id, this.model_no).subscribe({
        next: (res) => {
          this.prodSeasonList = res.map(item => ({
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
  }
  save() {
    this.spinnerService.show();
    if (this.file == null) {
      return this.snotify.warning(
        MessageConstants.INVALID_FILE,
        CaptionConstants.WARNING
      );
    }
    console.log(this.data);
    console.log(this.file);

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
    this.service.create(this.data, this.file).subscribe((res) => {
      if (res) {
        this.spinnerService.hide();
        this.snotify.success(
          MessageConstants.CREATED_OK_MSG,
          ActionConstants.CREATE
        );
        //   this.resetData();
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

  // resetData() {
  //   this.data.attachment_name = "";
  //   (<HTMLInputElement>document.getElementById("input_uploadFile")).value =
  //     null;
  //   this.file = null;
  //   this.data.attachment_type_id = "";
  // }
}
