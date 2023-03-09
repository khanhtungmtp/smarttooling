import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { Select2OptionData } from 'ng-select2';
import { NgxSpinnerService } from 'ngx-spinner';
import { environment } from '../../../../../../environments/environment';
import { CaptionConstants, MessageConstants } from '../../../../../_core/_constants/system.constants';
import { PbpPadPrintSetting } from '../../../../../_core/_models/production-bp/pbp-pad-print-setting';
import { PBP_Pad_Print_Setting_DTO } from '../../../../../_core/_models/production-bp/pbp-pad-print-setting-dto';
import { NgSnotifyService } from '../../../../../_core/_services/ng-snotify.service';
import { PadPrintSettingService } from '../../../../../_core/_services/production-bp/pad-print-setting.service';
import { FunctionUtility } from '../../../../../_core/_utility/function-utility';
import { MediaUploadComponent } from '../../../../commons/media-upload/media-upload.component';

@Component({
  selector: 'app-pad-print-setting-edit',
  templateUrl: './pad-print-setting-edit.component.html',
  styleUrls: ['./pad-print-setting-edit.component.css']
})
export class PadPrintSettingEditComponent implements OnInit {
  @ViewChild('mediaUploadVideo') mediaUploadVideo: MediaUploadComponent;
  @ViewChild('mediaUploadImage') mediaUploadImage: MediaUploadComponent;
  baseUrl: string = environment.imageUrl;
  noImage: string = "../../../../../../assets/img/no-image.jpg";
  model: PbpPadPrintSetting = {
    article_no_is_general: false,
  } as PbpPadPrintSetting;
  param: PBP_Pad_Print_Setting_DTO = {} as PBP_Pad_Print_Setting_DTO;
  listMachineVendor: Array<Select2OptionData> = [];
  listPadShape: Array<Select2OptionData> = [];
  isOk: boolean = false;
  random: number = Math.random();
  constructor(
    private snotify: NgSnotifyService,
    private router: Router,
    private spinnerService: NgxSpinnerService,
    private service: PadPrintSettingService,
    private fu: FunctionUtility
  ) { }

  ngOnInit() {
    this.service.currentModel.subscribe(data => {
      if (data) {
        this.param = data;
        this.getAllPadShape();
        this.getAllMachineVendor();
        this.getDetail();
      } else {
        this.back();
      }
    })

  }

  getAllMachineVendor() {
    this.service.getAllMachineVendor().subscribe(res => {
      this.listMachineVendor = res.map(x => ({ id: x.key, text: x.value }));
    });
  }

  getAllPadShape() {
    this.service.getAllPadShape().subscribe(res => {
      this.listPadShape = res.map(x => ({ id: x.key, text: x.value }));
    });
  }

  getDetail() {
    this.spinnerService.show();
    this.service.getDetail(this.param).subscribe(res => {
      this.spinnerService.hide();
      this.model = res;
    });
  }

  back() {
    this.router.navigate(['/production-bp/transaction/pad-print-setting/main']);
  }

  save() {
    this.formValidation();
    if (!this.isOk)
      return;
    if (this.mediaUploadVideo.url.includes('base64'))
      this.model.operation_video_url_file = this.mediaUploadVideo.url;
    else
      this.model.operation_video_url = '';
    if (this.mediaUploadImage.url.includes('base64'))
      this.model.component_photo_url_file = this.mediaUploadImage.url;
    else
      this.model.component_photo_url = '';
    this.service.update(this.model).subscribe(res => {
      if (res.success) {
        this.snotify.success(MessageConstants.UPDATED_OK_MSG, CaptionConstants.SUCCESS);
        this.back();
      } else {
        this.snotify.error(MessageConstants.UPDATED_ERROR_MSG, CaptionConstants.ERROR);
        console.log(res);
      }
    }, err => console.log(err));
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
    if (this.fu.isEmpty(this.model.material_description)) {
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
