import { Component, OnInit } from "@angular/core";
import { FormGroup, FormBuilder, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { Select2OptionData } from "ng-select2";
import { NgxSpinnerService } from "ngx-spinner";
import { LayoutDesignOverallService } from "../../../../../_core/_services/best-line/layout-design-overall.service";
import { NgSnotifyService } from "../../../../../_core/_services/ng-snotify.service";
import {
  ActionConstants,
  CaptionConstants,
  MessageConstants,
} from "../../../../../_core/_constants/system.constants";
import { environment } from "../../../../../../environments/environment";

@Component({
  selector: "app-layout-design-overall-add",
  templateUrl: "./layout-design-overall-add.component.html",
  styleUrls: ["./layout-design-overall-add.component.scss"],
})
export class LayoutDesignOverallAddComponent implements OnInit {
  addForm: FormGroup;
  baseUrl: string = environment.imageUrl;
  defaultImage: string =
    this.baseUrl + localStorage.getItem('factorySmartTooling') + "/Model/no-image.jpg";
  modelNoList: Array<Select2OptionData> = [];
  lineNoList: Array<Select2OptionData> = [];
  lineTypeList: Array<Select2OptionData> = [];
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
  modelName: string = "";
  constructor(
    private layoutDesignOverallService: LayoutDesignOverallService,
    private snotify: NgSnotifyService,
    private spinnerService: NgxSpinnerService,
    private route: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder
  ) { }

  ngOnInit() {
    this.spinnerService.show();
    this.initForm();
    this.getAllLineNo();
    this.getAllLineType();
    this.getAllModelNo();
    this.addForm.get("model_no").valueChanges.subscribe((value) => {
      if (value) {
        const modelName = this.modelNoList.find(
          (x) => x.id === value
        ).additional;
        this.addForm.get("model_name").patchValue(modelName);
      }
    });
    this.spinnerService.hide();
  }
  save() {
    this.spinnerService.show();
    this.layoutDesignOverallService.create(this.addForm.value).subscribe(
      res => {
        this.spinnerService.hide();
        if (res.success) {
          this.snotify.success(MessageConstants.CREATED_OK_MSG, ActionConstants.CREATE);
          this.router.navigate([
            "/best-line/transaction/layout-design-overall/main",
          ]);
        }
        else {
          if (!res.success && res.caption != null) {
            this.snotify.error(MessageConstants.CREATED_ERROR_MSG, CaptionConstants.ERROR);
          }
          else {
            this.snotify.error(res.message, CaptionConstants.ERROR);
          }
        }
      });
  }
  initForm() {
    this.addForm = this.fb.group({
      line_id: ["", Validators.compose([Validators.required])],
      line_type_id: ["", Validators.compose([Validators.required])],
      model_no: ["", Validators.compose([Validators.required])],
      model_name: [
        { value: this.modelName, disabled: true },
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
      prod_season: ["", Validators.compose([Validators.required, Validators.maxLength(4)])],
      c2b_overall_image: "",
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
          id: item.line_type_id,
          text: item.line_type_name,
        };
      });
    });
  }
  getAllModelNo() {
    this.layoutDesignOverallService.getAllModelNo().subscribe((res) => {
      this.modelNoList = res.map((item) => {
        return {
          id: item.model_no,
          text: item.model_no,
          additional: item.model_name,
        };
      });
    });
  }
  saveAndNext() {
    this.spinnerService.show();
    this.layoutDesignOverallService.create(this.addForm.value).subscribe(
      res => {
        this.spinnerService.hide();
        if (res.success) {
          this.snotify.success(MessageConstants.CREATED_OK_MSG, ActionConstants.CREATE);
          this.resetForm();
        }
        else {
          if (!res.success && res.caption != null) {
            this.snotify.error(MessageConstants.CREATED_ERROR_MSG, CaptionConstants.ERROR);
          }
          else {
            this.snotify.error(res.message, CaptionConstants.ERROR);
          }
        }
      }
    );
  }
  changeToUppercase() {
    this.addForm.patchValue({
      line_id: this.addForm.value.model_no.toUpperCase(),
      line_type_id: this.addForm.value.line_type_id.toUpperCase(),
      model_no: this.addForm.value.model_no.toUpperCase(),
      model_name: this.addForm.value.model_name.toUpperCase(),
    });
  }
  resetForm() {
    this.addForm.setValue({
      line_id: "",
      line_type_id: "",
      model_no: "",
      model_name: "",
      no_of_process_before: "",
      no_of_process_after: "",
      tct_before: "",
      tct_after: "",
      cps_mp_before: "",
      cps_mp_after: "",
      assembly_mp_before: "",
      assembly_mp_after: "",
      eolr_before: "",
      eolr_after: "",
      ller_before_percent: "",
      ller_after_percent: "",
      tentative_pph_before: "",
      tentative_pph_after: "",
      tentative_efficiency_before_percent: "",
      tentative_efficiency_after_percent: "",
      c2b_overall_image: "",
    });
  }
  onSelectFile(event) {
    if (event.target.files && event.target.files[0]) {
      const reader = new FileReader();
      reader.readAsDataURL(event.target.files[0]);
      const file = event.target.files[0];
      const title = event.target.files[0].name.split(".").pop();
      const fileZise = event.target.files[0].size;
      if (
        title === "jpg" ||
        title === "jpeg" ||
        title === "png" ||
        title === "JPG" ||
        title === "JPEG" ||
        title === "PNG"
      ) {
        if (fileZise <= 5242880) {
          reader.onload = (e) => {
            this.addForm.patchValue({
              c2b_overall_image: e.target.result.toString(),
            });
          };
        } else {
          this.snotify.error(
            MessageConstants.FILE_IMAGE_SIZE,
            CaptionConstants.ERROR
          );
        }
      }
    }
  }
}
