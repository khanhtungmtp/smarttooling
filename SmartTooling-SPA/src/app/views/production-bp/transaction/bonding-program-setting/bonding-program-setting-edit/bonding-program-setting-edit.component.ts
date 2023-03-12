import { Component, OnInit } from '@angular/core';
import {
  ActionConstants,
  CaptionConstants,
  MessageConstants,
} from "../../../../../_core/_constants/system.constants";
import { environment } from "../../../../../../environments/environment";
import { Select2OptionData } from 'ng-select2';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { NgSnotifyService } from '../../../../../_core/_services/ng-snotify.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ActivatedRoute, Router } from '@angular/router';
import { BondingProgramSettingService } from '../../../../../_core/_services/production-bp/bonding-program-setting.service';
import * as moment from 'moment';

@Component({
  selector: 'app-bonding-program-setting-edit',
  templateUrl: './bonding-program-setting-edit.component.html',
  styleUrls: ['./bonding-program-setting-edit.component.css']
})
export class BondingProgramSettingEditComponent implements OnInit {
  initialValues: string;
  editForm: UntypedFormGroup;
  baseUrl: string = environment.imageUrl;
  defaultImage: string =
    this.baseUrl + localStorage.getItem('factorySmartTooling') + "/Model/no-image.jpg";
  random: number = Math.random();

  autoTechList: Array<Select2OptionData> = [];
  chemicalProcessTypeList: Array<Select2OptionData> = [];
  modelNoList: Array<Select2OptionData> = [];
  mainUpperMaterialTypeList: Array<Select2OptionData> = [];
  mainBottomMaterialTypeList: Array<Select2OptionData> = [];
  adoptionComponentTypeList: Array<Select2OptionData> = [];
  processAdoptionScopeTypeList: Array<Select2OptionData> = [];
  chemicalSupplierTypeList: Array<Select2OptionData> = [];

  optionsSelectModelNo = {
    placeholder: "Select Model No...",
    allowClear: true,
    width: "100%",
  };

  optionsSelectAutoTech = {
    placeholder: "Select Auto Tech...",
    allowClear: true,
    width: "100%",
  };

  optionsSelectChemicalProcessType = {
    placeholder: "Select Chemical Process Type...",
    allowClear: true,
    width: "100%",
  };

  optionsSelectMainUpperMaterialType = {
    placeholder: "Select Main Upper Material Type...",
    allowClear: true,
    width: "100%",
  };

  optionsSelectMainBottomMaterialType = {
    placeholder: "Select Main Bottom Material Type...",
    allowClear: true,
    width: "100%",
  };

  optionsSelectAdoptionComponentType = {
    placeholder: "Select Adoption Component Type...",
    allowClear: true,
    width: "100%",
  };

  optionsSelectProcessAdoptionScopeType = {
    placeholder: "Select Process Adoption Scope Type...",
    allowClear: true,
    width: "100%",
  };

  optionsSelectChecmicalSupplierType = {
    placeholder: "Select Chemical Supplier Type...",
    allowClear: true,
    width: "100%",
  };

  modelName: string = "";

  constructor(
    private bondingProgramSettingService: BondingProgramSettingService,
    private snotify: NgSnotifyService,
    private spinnerService: NgxSpinnerService,
    private route: ActivatedRoute,
    private router: Router,
    private fb: UntypedFormBuilder
  ) { }

  ngOnInit(): void {
    this.spinnerService.show();
    this.initForm();
    this.getAllAutoTech();
    this.getAllLineType();
    this.getAllModelNo();
    this.getAllAdoptionComponentType();
    this.getAllProcessAdoptionScopeType();
    this.getAllMainUpperMaterialType();
    this.getAllMainBottomMaterialType();
    this.getAllChemicalSupplierType();

    this.editForm.get("model_no").valueChanges.subscribe((value) => {
      if (value && this.modelNoList.length > 0) {
        const modelName = this.modelNoList.find(
          (x) => x.id === value
        ).additional;
        this.editForm.get("model_name").patchValue(modelName);
      }
    });

    this.editForm.get("article_no_is_general").valueChanges.subscribe((value) => {
      if (value) {
        this.editForm.get("article_no_remarks").disable();
      }
      else {
        this.editForm.get("article_no_remarks").enable();
      }
    });

    this.initialValues = this.editForm.value;
    this.loadData();
    this.spinnerService.hide();

    this.articleNoIsGeneralChanged();
  }

  loadData() {
    this.spinnerService.show();
    this.bondingProgramSettingService.currentParamSearch
      .subscribe((res) => {
        this.spinnerService.hide();
        // this.lineId = res.line_id;
        // this.lineTypeId = res.line_type_id;
        // this.modelNo = res.model_no;
        res.shoes_photo_url = this.baseUrl + res.shoes_photo_url;
        this.editForm.patchValue(res);
        this.baseUrl =
          this.baseUrl +
          this.editForm.value.shoes_photo_url +
          "?" +
          Math.random();
      })
      .unsubscribe();
  }

  initForm() {
    this.editForm = this.fb.group({
      model_no: [
        { value: "", disabled: true },
        Validators.compose([Validators.required]),
      ],
      model_name: [
        { value: "", disabled: true },
        Validators.compose([Validators.required]),
      ],
      chemical_process_type_id: [
        { value: "", disabled: true },
        Validators.compose([Validators.required]),
      ],
      chemical_name: [
        { value: "", disabled: true },
        Validators.compose([Validators.required]),
      ],
      // chemical_process_type_name: [
      //   { value: "", disabled: true },
      //   Validators.compose([Validators.required]),
      // ],
      auto_tech_id: [
        { value: "", disabled: true },
        Validators.compose([Validators.required]),
      ],
      adoption_component_id: [
        { value: "", disabled: true },
        Validators.compose([Validators.required]),
      ],
      // auto_tech_name: [
      //   { value: "", disabled: true },
      //   Validators.compose([Validators.required]),
      // ]
      dev_season: [
        { value: "", disabled: false },
        Validators.compose([Validators.required, Validators.maxLength(4)]),
      ],
      production_season: [
        { value: "", disabled: false },
        Validators.compose([Validators.required, Validators.maxLength(4)]),
      ],
      chemical_supplier_id: [
        { value: "", disabled: false },
        Validators.compose([Validators.required]),
      ],
      article_no_is_general: [
        { value: "", disabled: false },
        Validators.compose([]),
      ],
      process_adoption_scope_id: [
        { value: "", disabled: false },
        Validators.compose([Validators.required]),
      ],
      article_no_remarks: [
        { value: "", disabled: false },
        Validators.compose([]),
      ],
      main_upper_material_type_id: [
        { value: "", disabled: false },
        Validators.compose([Validators.required]),
      ],
      main_bottom_material_type_id: [
        { value: "", disabled: false },
        Validators.compose([Validators.required]),
      ],
      first_month_of_production_adoption: [
        { value: "", disabled: false },
        Validators.compose([Validators.required]),
      ],
      shoes_photo_url: [
        { value: "", disabled: false },
        Validators.compose([]),
      ]
    });
  }

  // changeToUppercase() {
  //   this.editForm.patchValue({
  //     chemical_name: this.editForm.value.chemical_name.toUpperCase(),
  //     dev_season: this.editForm.value.dev_season.toUpperCase(),
  //     chemical_supplier: this.editForm.value.chemical_supplier.toUpperCase(),
  //     production_season: this.editForm.value.production_season.toUpperCase(),
  //   });
  // }

  getAllAutoTech() {
    this.bondingProgramSettingService.getAllAutoTech().subscribe((res) => {
      this.autoTechList = res.map((item) => {
        return {
          id: item.key,
          text: item.value,
        };
      });
    });
  }

  getAllLineType() {
    this.bondingProgramSettingService.getAllChemicalProcessType().subscribe((res) => {
      this.chemicalProcessTypeList = res.map((item) => {
        return {
          id: item.key,
          text: item.value,
        };
      });
    });
  }

  getAllModelNo() {
    this.bondingProgramSettingService.getAllModelNo().subscribe((res) => {
      this.modelNoList = res.map((item) => {
        return {
          id: item.key,
          text: item.key,
          additional: item.value,
        };
      });
    });
  }

  getAllAdoptionComponentType() {
    this.bondingProgramSettingService.getAllAdoptionComponentType().subscribe((res) => {
      this.adoptionComponentTypeList = res.map((item) => {
        return {
          id: item.key,
          text: item.value
        };
      });
    });
  }


  getAllProcessAdoptionScopeType() {
    this.bondingProgramSettingService.getAllProcessAdoptionScopeType().subscribe((res) => {
      this.processAdoptionScopeTypeList = res.map((item) => {
        return {
          id: item.key,
          text: item.value
        };
      });
    });
  }


  getAllMainUpperMaterialType() {
    this.bondingProgramSettingService.getAllMainUpperMaterialType().subscribe((res) => {
      this.mainUpperMaterialTypeList = res.map((item) => {
        return {
          id: item.key,
          text: item.value
        };
      });
    });
  }

  getAllMainBottomMaterialType() {
    this.bondingProgramSettingService.getAllMainBottomMaterialType().subscribe((res) => {
      this.mainBottomMaterialTypeList = res.map((item) => {
        return {
          id: item.key,
          text: item.value
        };
      });
    });
  }

  getAllChemicalSupplierType() {
    this.bondingProgramSettingService.getAllChemicalSupplierType().subscribe((res) => {
      this.chemicalSupplierTypeList = res.map((item) => {
        return {
          id: item.key,
          text: item.value
        };
      });
    });
  }

  save() {
    this.spinnerService.show();

    var value = this.editForm.value;
    var rawValue = this.editForm.getRawValue();
    value.model_no = rawValue.model_no;
    value.chemical_process_type_id = rawValue.chemical_process_type_id;
    value.auto_tech_id = rawValue.auto_tech_id;
    value.chemical_name = rawValue.chemical_name;
    value.adoption_component_id = rawValue.adoption_component_id;
    value.first_month_of_production_adoption = moment(value.first_month_of_production_adoption).format('YYYY-MM') + '-01';

    this.bondingProgramSettingService.update(value).subscribe(
      () => {
        this.spinnerService.hide();
        this.snotify.success(
          MessageConstants.UPDATED_OK_MSG,
          ActionConstants.EDIT
        );
        this.router.navigate([
          "/production-bp/transaction/bonding-program-setting/main",
        ]);
      },
      (error) => {
        this.spinnerService.hide();
        return this.snotify.error(
          MessageConstants.UPDATED_ERROR_MSG,
          CaptionConstants.ERROR
        );
      }
    );
  }

  onOpenCalendar(container) {
    container.monthSelectHandler = (event: any): void => {
      container._store.dispatch(container._actions.select(event.date));
    };
    container.setViewMode('month');
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
            this.editForm.patchValue({
              shoes_photo_url: e.target.result.toString(),
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

  articleNoIsGeneralChanged() {
    this.editForm.controls.article_no_is_general.valueChanges.subscribe((res: boolean) => {
      if (res) {
        this.editForm.controls.article_no_remarks.setValue(null);
      }
    });
  }


  findInvalidControls() {
    const invalid = [];
    const controls = this.editForm.controls;
    for (const name in controls) {
      if (controls[name].invalid) {
        invalid.push(name);
      }
    }
    console.log(invalid);
  }

}
