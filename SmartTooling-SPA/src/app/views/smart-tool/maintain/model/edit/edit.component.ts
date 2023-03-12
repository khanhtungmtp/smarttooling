import { Component, OnInit } from "@angular/core";
import { UntypedFormGroup, UntypedFormBuilder, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { Select2OptionData } from "ng-select2";
import { NgxSpinnerService } from "ngx-spinner";
import { environment } from "../../../../../../environments/environment";
import { CaptionConstants, MessageConstants } from "../../../../../_core/_constants/system.constants";
import { NgSnotifyService } from "../../../../../_core/_services/ng-snotify.service";
import { ModelService } from "../../../../../_core/_services/smart-tool/model.service";
import { Model} from '../../../../../_core/_models/smart-tool/model';


@Component({
  selector: "app-edit",
  templateUrl: "./edit.component.html",
  styleUrls: ["./edit.component.scss"],
})
export class EditComponent implements OnInit {
  editModelForm: UntypedFormGroup;
  url: string = environment.imageUrl;
  modelTypeList: Array<Select2OptionData>;
  constructor(
    private modelService: ModelService,
    private route: ActivatedRoute,
    private snotifyAlert: NgSnotifyService,
    private router: Router,
    private spinner: NgxSpinnerService,
    private fb: UntypedFormBuilder
  ) {}

  ngOnInit() {
    this.initForm();
    this.route.data.subscribe((data) => {
      this.spinner.hide();
      this.editModelForm.patchValue(data.model);
      this.url =
        this.url + this.editModelForm.value.model_picture + "?v=" + Math.random();
    });
    this.getAllModelType();
  }

  initForm() {
    this.editModelForm = this.fb.group({
      factory_id: "",
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
      create_by: "",
      create_time: "",
      update_by: "",
      update_time: "",
    });
  }

  changeToUppercase() {
    this.editModelForm.patchValue({
      model_no: this.editModelForm.value.model_no.toUpperCase(),
      model_name: this.editModelForm.value.model_name.toUpperCase(),
      model_family: this.editModelForm.value.model_family.toUpperCase(),
      dev_season: this.editModelForm.value.dev_season.toUpperCase(),
      prod_season: this.editModelForm.value.prod_season.toUpperCase()
    });
  }

  backList() {
    this.router.navigate(["/model/list"]);
  }

  btnSave() {
    this.changeToUppercase();
    let model: Model = this.editModelForm.value;
    model.pilot_line = String(model.pilot_line )==='true'? true:false;
    model.top_model = String(model.top_model) ==='true'?true:false;
    this.modelService.updateModel(model).subscribe(
      () => {
        this.router.navigate(["/model/list"]);
        this.snotifyAlert.success(MessageConstants.UPDATED_OK_MSG, CaptionConstants.SUCCESS);
      },
      (error) => {
        this.snotifyAlert.error(MessageConstants.UPDATED_ERROR_MSG, CaptionConstants.ERROR);
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
            this.url = event.target.result.toString();
            this.editModelForm.patchValue({
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
    this.backList();
  }
}
