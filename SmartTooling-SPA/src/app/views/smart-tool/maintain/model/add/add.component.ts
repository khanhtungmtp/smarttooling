import { Component, OnInit } from "@angular/core";
import { FormGroup, FormBuilder, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { Select2OptionData } from "ng-select2";
import { environment } from "../../../../../../environments/environment";
import { CaptionConstants, MessageConstants } from "../../../../../_core/_constants/system.constants";
import { Model } from "../../../../../_core/_models/smart-tool/model";
import { NgSnotifyService } from "../../../../../_core/_services/ng-snotify.service";
import { ModelService } from "../../../../../_core/_services/smart-tool/model.service";


@Component({
  selector: "app-add",
  templateUrl: "./add.component.html",
  styleUrls: ["./add.component.scss"],
})
export class AddComponent implements OnInit {
  addModelForm: FormGroup;
  baseUrl: string = environment.imageUrl;
  defaultImage: string =
    this.baseUrl + localStorage.getItem('factorySmartTooling') + "/Model/no-image.jpg";
  modelTypeList: Array<Select2OptionData>;

  constructor(
    private modelService: ModelService,
    private snotifyAlert: NgSnotifyService,
    private router: Router,
    private fb: FormBuilder
  ) { }

  ngOnInit() {
    this.getAllModelType();
    this.initForm();
  }

  backList() {
    this.router.navigate(["/model/list"]);
  }

  changeToUppercase() {
    this.addModelForm.patchValue({
      model_no: this.addModelForm.value.model_no.toUpperCase(),
      model_name: this.addModelForm.value.model_name.toUpperCase(),
      model_family: this.addModelForm.value.model_family.toUpperCase(),
      dev_season: this.addModelForm.value.dev_season.toUpperCase(),
      prod_season: this.addModelForm.value.prod_season.toUpperCase()
    });
  }

  saveAndNext() {
    this.changeToUppercase();
    this.modelService.createModel(this.addModelForm.value).subscribe(
      () => {
        this.snotifyAlert.success(MessageConstants.CREATED_OK_MSG, CaptionConstants.SUCCESS);
        this.resetForm();
        console.log(this.addModelForm.value);
      },
      (error) => {
        this.snotifyAlert.error(MessageConstants.CREATED_ERROR_MSG, CaptionConstants.ERROR);
      }
    );
  }

  save() {
    this.changeToUppercase();
    let model: Model = this.addModelForm.value;
    model.pilot_line = String(model.pilot_line )==='true'? true:false;
    model.top_model = String(model.top_model) ==='true'?true:false;
    this.modelService.createModel(model).subscribe(
      () => {
        this.snotifyAlert.success(MessageConstants.CREATED_OK_MSG, CaptionConstants.SUCCESS);
        this.router.navigate(["/model/list"]);
      },
      (error) => {
        this.snotifyAlert.error(MessageConstants.CREATED_ERROR_MSG, CaptionConstants.ERROR);
      }
    );
  }

  onSelectFile(event) {
    console.log(event);
    if (event.target.files && event.target.files[0]) {
      const reader = new FileReader();
      reader.readAsDataURL(event.target.files[0]);
      var file = event.target.files[0];
      var title = event.target.files[0].name.split(".").pop();
      var fileZise = event.target.files[0].size;
      if (
        title == "jpg" ||
        title == "jpeg" ||
        title == "png" ||
        title == "JPG" ||
        title == "JPEG" ||
        title == "PNG"
      ) {
        if (fileZise <= 5242880) {
          reader.onload = (event) => {
            this.addModelForm.patchValue({
              model_picture: event.target.result.toString(),
            });
          };
        } else {
          this.snotifyAlert.error(MessageConstants.FILE_IMAGE_SIZE, CaptionConstants.ERROR);
        }
      }
    }
  }

  getAllModelType() {
    this.modelService.getAllModelType().subscribe((res) => {
      this.modelTypeList = res.map((item) => {
        return { id: item.id, text: item.name };
      });
    });
  }

  cancel() {
    this.resetForm();
    console.log(this.addModelForm.value);
  }

  initForm() {
    this.addModelForm = this.fb.group({
      model_no: ["", Validators.compose([Validators.required])],
      upper_id: ["", Validators.compose([Validators.required, Validators.maxLength(6)])],
      model_name: ["", Validators.compose([Validators.required])],
      model_family: [""],
      model_type_id: ["", Validators.compose([Validators.required])],
      top_model:[null, Validators.compose([Validators.required])],
      pilot_line:[null, Validators.compose([Validators.required])],
      is_active: true,
      volume: [null, Validators.compose([Validators.min(0)])],
      volume_percent: [null, Validators.compose([Validators.min(0)])],
      dev_season: ["", Validators.compose([Validators.required])],
      prod_season: ["", Validators.compose([Validators.required])],
      remarks: [""],
      model_picture: "",
    });
  }

  resetForm() {
    this.addModelForm.setValue({
      model_no: "",
      upper_id: "",
      model_name: "",
      model_family: "",
      model_type_id: "",
      top_modal:null,
      piot_line:null,
      is_active: true,
      volume: null,
      volume_percent: null,
      dev_season: "",
      prod_season: "",
      remarks: "",
      model_picture: "",
    });
  }
}
