import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { Select2OptionData } from 'ng-select2';
import { NgxSpinnerService } from 'ngx-spinner';
import { environment } from '../../../../../../environments/environment';
import { MessageConstants, CaptionConstants } from '../../../../../_core/_constants/system.constants';
import { ComputerStitchingSettingView } from '../../../../../_core/_models/production-bp/pbp-computer-stitching-setting-view';
import { NgSnotifyService } from '../../../../../_core/_services/ng-snotify.service';
import { PbpComputerStitchingSettingService } from '../../../../../_core/_services/production-bp/pbp-computer-stitching-setting.service';
import { MediaUploadComponent } from '../../../../commons/media-upload/media-upload.component';

@Component({
  selector: 'app-computer-stitching-setting-edit',
  templateUrl: './computer-stitching-setting-edit.component.html',
  styleUrls: ['./computer-stitching-setting-edit.component.scss']
})
export class ComputerStitchingSettingEditComponent implements OnInit {
  @ViewChild('mediaUploadImage') mediaUploadImage: MediaUploadComponent;
  @ViewChild('mediaUploadVideo') mediaUploadVideo: MediaUploadComponent;
  imageUrl: string = environment.imageUrl;
  noImage: string = "../../../../../../assets/img/no-image.jpg";
  itemEdit: ComputerStitchingSettingView = {
    model_no: '',
    model_name: '',
    stage_id: '',
    stage_name: '',
    operation_id: '',
    operation_name_en: '',
    operation_name_local: '',
    operation_name_zh: '',
    cs_type_id: '',
    cs_type_name: '',
    cs_machine_type_id: '',
    cs_machine_type_name: '',
    main_upper_material_type_id: '',
    jig_design_id: '',
    jig_design_name: '',
    jig_photo_url: '',
    cs_video_url: '',
    article_no_is_general: false
  } as ComputerStitchingSettingView;
  mainUpperMaterialTypes: Array<Select2OptionData> = [];
  jigDesigns: Array<Select2OptionData> = [];
  lang: string;
  yesNo: Array<Select2OptionData> = [
    { id: 'true', text: 'Yes' },
    { id: 'false', text: 'No' }
  ];
  random: number = Math.random();
  constructor(
    private computerService: PbpComputerStitchingSettingService,
    private router: Router,
    private snotify: NgSnotifyService,
    private spinner: NgxSpinnerService
  ) { }

  async ngOnInit() {
    await this.loadData();
    await this.getMainUpperMaterialType();
    await this.getJigDesign();
  }

  loadData() {
    this.computerService.currentModel.subscribe(res => {
      if (res) {
        this.itemEdit = res;
      } else {
        this.back();
      }
    }).unsubscribe();
    this.computerService.currentParamSearch.subscribe(res => {
      if (res)
        this.lang = res.lang;
    }).unsubscribe();
  }

  async getMainUpperMaterialType() {
    let res = await this.computerService.getMainUpperMaterialType().toPromise();
    if (res) {
      this.mainUpperMaterialTypes = res.map(item => {
        return { id: item.key, text: item.value };
      });
    }
  }

  async getJigDesign() {
    let res = await this.computerService.getJigDesign().toPromise();
    if (res)
      this.jigDesigns = res.map(item => {
        return { id: item.key, text: item.value };
      });
  }

  changeArticleNo() {
    if (this.itemEdit.article_no_is_general)
      this.itemEdit.article_no_remarks = '';
  }

  btnSave(): void {
    this.spinner.show();
    this.itemEdit.cs_machine_model = this.itemEdit.cs_machine_model.toUpperCase();
    this.itemEdit.jig_photo_url = this.mediaUploadImage.url.includes("base64") ? this.mediaUploadImage.url : '';
    this.itemEdit.cs_video_url = this.mediaUploadVideo.url.includes("base64") ? this.mediaUploadVideo.url : '';
    this.itemEdit.sop_setup = String(this.itemEdit.sop_setup) == 'true';
    this.itemEdit.is_critical_process = String(this.itemEdit.is_critical_process) == 'true';
    this.itemEdit.production_adoption = String(this.itemEdit.production_adoption) == 'true';

    this.computerService.updateComputerStitchingSetting(this.itemEdit).subscribe(res => {
      this.spinner.hide();
      if (res.success) {
        this.snotify.success(MessageConstants.UPDATED_OK_MSG, CaptionConstants.SUCCESS);
        this.back();
      } else {
        this.snotify.error(res.caption, CaptionConstants.ERROR);
        console.log(res.caption);
      }
    }, error => {
      this.spinner.hide();
      this.snotify.error(MessageConstants.UPDATED_ERROR_MSG, CaptionConstants.ERROR);
      console.log(error)
    });
  }

  back() {
    this.router.navigate(['/production-bp/transaction/computer-stitching-setting']);
  }

}
