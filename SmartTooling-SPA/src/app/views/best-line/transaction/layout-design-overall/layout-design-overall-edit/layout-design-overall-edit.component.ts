import { Component, OnInit, ViewChild } from "@angular/core";
import { FormGroup, FormBuilder, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { Select2OptionData } from "ng-select2";
import { NgxSpinnerService } from "ngx-spinner";
import { environment } from "../../../../../../environments/environment";
import { LayoutDesignOverallService } from "../../../../../_core/_services/best-line/layout-design-overall.service";
import { NgSnotifyService } from "../../../../../_core/_services/ng-snotify.service";
import {
  ActionConstants,
  CaptionConstants,
  MessageConstants,
} from "../../../../../_core/_constants/system.constants";
import { MediaUploadComponent } from "../../../../commons/media-upload/media-upload.component";

@Component({
  selector: "app-layout-design-overall-edit",
  templateUrl: "./layout-design-overall-edit.component.html",
  styleUrls: ["./layout-design-overall-edit.component.scss"],
})
export class LayoutDesignOverallEditComponent implements OnInit {
  editForm: FormGroup;
  modelNoList: Array<Select2OptionData> = [];
  lineNoList: Array<Select2OptionData> = [];
  lineTypeList: Array<Select2OptionData> = [];
  modelName: string = "";
  url: string = environment.imageUrl;
  baseUrl: string = environment.imageUrl;
  defaultImage: string =
    this.baseUrl + localStorage.getItem('factorySmartTooling') + "/Model/no-image.jpg";
  lineId: string = "";
  lineTypeId: string = "";
  modelNo: string = "";
  random: number = Math.random();
  @ViewChild('image') image!: MediaUploadComponent;
  imageUrl: string = environment.imageUrl;
  constructor(
    private layoutDesignOverallService: LayoutDesignOverallService,
    private snotify: NgSnotifyService,
    private spinnerService: NgxSpinnerService,
    private router: Router,
    private fb: FormBuilder
  ) { }

  ngOnInit() {
    this.initForm();
    this.getAllModelNo();
    this.getAllLineNo();
    this.getAllLineType();
    this.loadData();
  }

  loadData() {
    this.spinnerService.show();
    this.layoutDesignOverallService.currentParamSearch
      .subscribe((res) => {
        this.spinnerService.hide();
        console.log(res);

        this.lineId = res.line_id;
        this.lineTypeId = res.line_type_id;
        this.modelNo = res.model_no;
        res.c2b_overall_image = this.url + res.c2b_overall_image;
        this.editForm.patchValue(res);
        this.url =
          this.url +
          this.editForm.value.c2b_overall_image +
          "?" +
          Math.random();
      })
      .unsubscribe();
  }

  getAllModelNo() {
    this.layoutDesignOverallService.getAllModelNo().subscribe((res) => {
      this.modelNoList = res.map((item) => {
        return {
          id: item.model_no,
          text: item.model_no,
        };
      });
    });
  }
  getAllLineNo() {
    this.layoutDesignOverallService.getAllLineNo().subscribe((res) => {
      this.lineNoList = res.map((item) => {
        return { id: item.line_id, text: item.line_name };
      });
    });
  }
  getAllLineType() {
    this.layoutDesignOverallService.getAllLineType().subscribe((res) => {
      this.lineTypeList = res.map((item) => {
        return {
          id: item.line_id,
          text: item.line_type_id,
        };
      });
    });
  }

  save() {
    this.spinnerService.show();
    this.editForm.value.line_id = this.lineId;
    this.editForm.value.line_type_id = this.lineTypeId;
    this.editForm.value.model_no = this.modelNo;
    this.editForm.value.c2b_overall_image =
      this.image.url.includes('base64')
        ? this.image.url
        : "";

    this.layoutDesignOverallService.update(this.editForm.value).subscribe(
      () => {
        this.spinnerService.hide();
        this.router.navigate([
          "/best-line/transaction/layout-design-overall/main",
        ]);
        this.snotify.success(
          MessageConstants.UPDATED_OK_MSG,
          ActionConstants.EDIT
        );
      },
      (error) => {
        this.spinnerService.hide();
        this.snotify.error(
          MessageConstants.UPDATED_ERROR_MSG,
          CaptionConstants.ERROR
        );
      }
    );
  }
  initForm() {
    this.editForm = this.fb.group({
      line_id: [
        { value: "" },
        Validators.compose([Validators.required]),
      ],
      line_type_id: [
        { value: "" },
        Validators.compose([Validators.required]),
      ],
      model_no: [
        { value: "" },
        Validators.compose([Validators.required]),
      ],
      model_name: [
        { value: "", disabled: true },
        Validators.compose([Validators.required]),
      ],
      no_of_process_before: null,
      no_of_process_after: null,
      tct_before: ["", Validators.compose([Validators.required])],
      tct_after: ["", Validators.compose([Validators.required])],
      cps_mp_before: ["", Validators.compose([Validators.required])],
      cps_mp_after: ["", Validators.compose([Validators.required])],
      assembly_mp_before: ["", Validators.compose([Validators.required])],
      assembly_mp_after: ["", Validators.compose([Validators.required])],
      eolr_before: ["", Validators.compose([Validators.required])],
      eolr_after: ["", Validators.compose([Validators.required])],
      ller_before_percent: ["", Validators.compose([Validators.required])],
      ller_after_percent: ["", Validators.compose([Validators.required])],
      tentative_pph_before: ["", Validators.compose([Validators.required])],
      tentative_pph_after: ["", Validators.compose([Validators.required])],
      tentative_efficiency_before_percent: null,
      tentative_efficiency_after_percent: null,
      c2b_overall_image: "",
      prod_season: ["", Validators.compose([Validators.required, Validators.maxLength(4)])]
    });
  }
}
