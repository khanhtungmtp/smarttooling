import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { Select2OptionData } from 'ng-select2';
import { NgxSpinnerService } from 'ngx-spinner';
import { environment } from '../../../../../../environments/environment';
import { CaptionConstants, MessageConstants } from '../../../../../_core/_constants/system.constants';
import { PbpPadPrintSetting } from '../../../../../_core/_models/production-bp/pbp-pad-print-setting';
import { ModelList } from '../../../../../_core/_models/smart-tool/modelList';
import { NgSnotifyService } from '../../../../../_core/_services/ng-snotify.service';
import { PadPrintSettingService } from '../../../../../_core/_services/production-bp/pad-print-setting.service';
import { SharedResourcesService } from '../../../../../_core/_services/shared-resources.service';
import { FunctionUtility } from '../../../../../_core/_utility/function-utility';
import { MediaUploadComponent } from '../../../../commons/media-upload/media-upload.component';

@Component({
  selector: 'app-pad-print-setting-add',
  templateUrl: './pad-print-setting-add.component.html',
  styleUrls: ['./pad-print-setting-add.component.css']
})
export class PadPrintSettingAddComponent implements OnInit {
  @ViewChild('mediaUploadVideo') mediaUploadVideo: MediaUploadComponent;
  @ViewChild('mediaUploadImage') mediaUploadImage: MediaUploadComponent;
  model: PbpPadPrintSetting = {
    article_no_is_general: false,
  } as PbpPadPrintSetting;
  modelName: string = '';
  isOk: boolean = false;
  listModelNo: Array<Select2OptionData> = [];
  modelList: ModelList[] = [];
  listPadShape: Array<Select2OptionData> = [];
  listMaterialType: Array<Select2OptionData> = [];
  listMachineVendor: Array<Select2OptionData> = [];
  baseUrl: string = environment.imageUrl;
  noImage: string = this.baseUrl + localStorage.getItem('factorySmartTooling') + "/no-image.jpg";
  constructor(
    private snotify: NgSnotifyService,
    private router: Router,
    private spinnerService: NgxSpinnerService,
    private service: PadPrintSettingService,
    private fu: FunctionUtility,
    private shareService: SharedResourcesService) { }

  ngOnInit() {
    this.getAllModel();
    this.getAllPadShape();
    this.getAllMaterialType();
    this.getAllMachineVendor();
  }

  getAllModel() {
    this.shareService.getAllModel().subscribe(res => {
      this.modelList = res;
      this.listModelNo = this.modelList.map(x => ({ id: x.model_No, text: x.model_No }));
    });
  }

  getAllPadShape() {
    this.service.getAllPadShape().subscribe(res => {
      this.listPadShape = res.map(x => ({ id: x.key, text: x.value }));
    });
  }

  getAllMaterialType() {
    this.service.getAllMaterialType().subscribe(res => {
      this.listMaterialType = res.map(x => ({ id: x.key, text: x.value }));
    });
  }

  getAllMachineVendor() {
    this.service.getAllMachineVendor().subscribe(res => {
      this.listMachineVendor = res.map(x => ({ id: x.key, text: x.value }));
    });
  }

  saveAndNext() {
    this.formValidation();
    if (!this.isOk)
      return;
    if (this.mediaUploadVideo.url.includes('base64'))
      this.model.operation_video_url_file = this.mediaUploadVideo.url;
    else
      this.model.operation_video_url_file = '';
    if (this.mediaUploadImage.url.includes('base64'))
      this.model.component_photo_url_file = this.mediaUploadImage.url;
    else
      this.model.component_photo_url_file = '';
    this.model.component_name = this.model.component_name.toUpperCase();
    this.spinnerService.show();
    this.service.add(this.model).subscribe(res => {
      if (res.success) {
        this.snotify.success(MessageConstants.CREATED_OK_MSG, CaptionConstants.SUCCESS);
        this.model = {
          article_no_is_general: false,
        } as PbpPadPrintSetting;
        this.modelName = '';
        this.isOk = false;
        this.spinnerService.hide();
      } else {
        this.snotify.error(MessageConstants.CREATED_ERROR_MSG, CaptionConstants.ERROR);
        console.log(res);
        this.spinnerService.hide();
      }
    }, err => console.log(err));
  }

  save() {
    this.formValidation();
    if (!this.isOk)
      return;
    if (this.mediaUploadVideo.url.includes('base64'))
      this.model.operation_video_url_file = this.mediaUploadVideo.url;
    else
      this.model.operation_video_url_file = '';
    if (this.mediaUploadImage.url.includes('base64'))
      this.model.component_photo_url_file = this.mediaUploadImage.url;
    else
      this.model.component_photo_url_file = '';
    this.model.component_name = this.model.component_name.toUpperCase();
    this.service.add(this.model).subscribe(res => {
      if (res.success) {
        this.snotify.success(MessageConstants.CREATED_OK_MSG, CaptionConstants.SUCCESS);
        this.back();
      } else {
        this.snotify.error(MessageConstants.CREATED_ERROR_MSG, CaptionConstants.ERROR);
        console.log(res);
      }
    }, err => console.log(err));
  }

  back() {
    this.router.navigate(['/production-bp/transaction/pad-print-setting/main']);
  }

  changeModel(event) {
    this.model.model_no = event;
    let model = this.modelList.find(x => x.model_No == event);
    this.modelName = model.model_Name;
    this.model.dev_season = model.dev_Season;
    this.model.production_season = model.prod_Season;
  }

  changeMaterialType(event) {
    this.model.material_type_id = event;
  }

  changePadShape(event) {
    this.model.pad_shape_id = event;
  }

  changeMachineVendor(event) {
    this.model.machine_vendor_id = event;
  }

  changeArticleRemark() {
    if (this.model.article_no_is_general)
      this.model.article_no_remarks = '';
  }

  formValidation() {
    if (this.fu.isEmpty(this.model.model_no)) {
      this.snotify.warning("Model No is required", CaptionConstants.WARNING);
    }
    else if (this.fu.isEmpty(this.model.component_name)) {
      this.snotify.warning("Component Name is required", CaptionConstants.WARNING);
    }
    else if (this.fu.isEmpty(this.model.material_type_id)) {
      this.snotify.warning("Material Type is required", CaptionConstants.WARNING);
    }
    else if (this.fu.isEmpty(this.model.chemical_ink)) {
      this.snotify.warning("Chemical - Ink is required", CaptionConstants.WARNING);
    }
    else if (this.fu.isEmpty(this.model.material_description)) {
      this.snotify.warning("Material Description is required", CaptionConstants.WARNING);
    }
    else if (this.fu.isEmpty(this.model.pad_shape_id)) {
      this.snotify.warning("Pad Shape is required", CaptionConstants.WARNING);
    }
    else if (this.model.number_of_pad_hits == 0) {
      this.snotify.warning("# of Pad Hits is required", CaptionConstants.WARNING);
    }
    else if (this.fu.isEmpty(this.model.machine_vendor_id)) {
      this.snotify.warning("Machine Vendor is required", CaptionConstants.WARNING);
    }
    else if (this.fu.isEmpty(this.model.machine_model)) {
      this.snotify.warning("Machine Model is required", CaptionConstants.WARNING);
    }
    else if (this.fu.isEmpty(this.model.chemical_hardener)) {
      this.snotify.warning("Chemical - Hardener is required", CaptionConstants.WARNING);
    }
    else if (this.fu.isEmpty(this.model.chemical_additive)) {
      this.snotify.warning("Chemical - Additive is required", CaptionConstants.WARNING);
    }
    else if (this.fu.isEmpty(this.model.chemical_primer)) {
      this.snotify.warning("Chemical - Primer is required", CaptionConstants.WARNING);
    }
    else if (this.fu.isEmpty(this.model.chemical_others)) {
      this.snotify.warning("Chemical - Others is required", CaptionConstants.WARNING);
    }
    else if (this.model.is_rotary_table_used == null) {
      this.snotify.warning("Rotary Table Used is required", CaptionConstants.WARNING);
    }
    else if (this.fu.isEmpty(this.model.article_no_remarks) && this.model.article_no_is_general === false) {
      this.snotify.warning("Article No is required", CaptionConstants.WARNING);
    }
    else
      return this.isOk = true;
  }

}
