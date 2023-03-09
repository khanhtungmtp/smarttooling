import { PbpComputerStitchingSettingService } from './../../../../../_core/_services/production-bp/pbp-computer-stitching-setting.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { Select2OptionData } from 'ng-select2';
import { NgxSpinnerService } from 'ngx-spinner';
import { ModelOperation } from '../../../../../_core/_models/smart-tool/model-operation';
import { NgSnotifyService } from '../../../../../_core/_services/ng-snotify.service';
import { ComputerStitchingSettingView } from '../../../../../_core/_models/production-bp/pbp-computer-stitching-setting-view';
import { MediaUploadComponent } from '../../../../commons/media-upload/media-upload.component';
import { CaptionConstants, MessageConstants } from '../../../../../_core/_constants/system.constants';
import { SharedResourcesService } from '../../../../../_core/_services/shared-resources.service';
import { ModelList } from '../../../../../_core/_models/smart-tool/modelList';

@Component({
  selector: 'app-computer-stitching-setting-add',
  templateUrl: './computer-stitching-setting-add.component.html',
  styleUrls: ['./computer-stitching-setting-add.component.scss']
})
export class ComputerStitchingSettingAddComponent implements OnInit {
  @ViewChild('mediaUploadImage') mediaUploadImage: MediaUploadComponent;
  @ViewChild('mediaUploadVideo') mediaUploadVideo: MediaUploadComponent;
  noImage: string = "../../../../../../assets/img/no-image.jpg";
  itemAdd: ComputerStitchingSettingView = {
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
  models: Array<Select2OptionData> = [];
  modelList : ModelList[] = [];
  stages: Array<Select2OptionData> = [];
  csOperations: Array<Select2OptionData> = [];
  resultCSOperation: ModelOperation[] = [];
  csTypes: Array<Select2OptionData> = [];
  csMachineTypes: Array<Select2OptionData> = [];
  mainUpperMaterialTypes: Array<Select2OptionData> = [];
  jigDesigns: Array<Select2OptionData> = [];
  lang: string;
  yesNo: Array<Select2OptionData> = [
    { id: 'true', text: 'Yes' },
    { id: 'false', text: 'No' }
  ];
  constructor(
    private computerService: PbpComputerStitchingSettingService,
    private router: Router,
    private spinner: NgxSpinnerService,
    private snotify: NgSnotifyService,
    private shareService : SharedResourcesService
  ) { }

  async ngOnInit() {
    this.spinner.show();
    await this.getLang();
    await this.getModel();
    await this.getStage();
    await this.getCSType()
    await this.getCSMachineType();
    await this.getMainUpperMaterialType();
    await this.getJigDesign();
    this.spinner.hide();
  }

  getLang() {
    this.computerService.currentParamSearch.subscribe(res => {
      // if (res)
      res ? this.lang = res.lang : this.lang = 'en';
    }).unsubscribe();
  }

  async getModel() {
    let res = await this.shareService.getAllModel().toPromise();
    if (res) {
      this.modelList = res;
      this.models = res.map(item => {
        return { id: item.model_No, text: item.model_No };
      });
    }
  }

  async getStage() {
    let res = await this.computerService.getStage().toPromise();
    if (res)
      this.stages = res.map(item => {
        return { id: item.key, text: item.value };
      });
  }

  async getCSOperation() {
    if (this.itemAdd.model_no !== '' && this.itemAdd.stage_id !== '') {
      let res = await this.computerService.getCSOperation(this.itemAdd.model_no, this.itemAdd.stage_id).toPromise();
      if (res) {
        this.csOperations = res.map(item => {
          return {
            id: item.operation_id,
            text: this.lang != 'en' ?
              (this.lang == 'vi' ? item.operation_name_local : item.operation_name_zh) : item.operation_name_en
          };
        });
        this.resultCSOperation = res;
      }
    }
  }

  async getCSType() {
    let res = await this.computerService.getCSType().toPromise();
    if (res)
      this.csTypes = res.map(item => {
        return { id: item.key, text: item.value };
      });
  }

  async getCSMachineType() {
    let res = await this.computerService.getCSMachineType().toPromise();
    if (res)
      this.csMachineTypes = res.map(item => {
        return { id: item.key, text: item.value };
      });
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

  async changeModel(event: string): Promise<void> {
    if (event) {
      this.itemAdd.model_no = event;
      let model = this.modelList.find(x => x.model_No == event);
      this.itemAdd.model_name = model.model_Name
      this.itemAdd.dev_season = model.dev_Season;
      this.itemAdd.production_season = model.prod_Season;
      await this.getCSOperation();
    }
  }

  async changeStage(event: string): Promise<void> {
    if (event) {
      this.itemAdd.stage_id = event
      await this.getCSOperation();
    }
  }

  changeCSOperation(event: string): void {
    if (event) {
      let criticalEfficiency = this.resultCSOperation.find(x => x.operation_id == event)?.critical_efficiency;
      let criticalQuality = this.resultCSOperation.find(x => x.operation_id == event)?.critical_quality;
      if (criticalEfficiency || criticalQuality) {
        this.itemAdd.is_critical_process = true;
      } else {
        this.itemAdd.is_critical_process = false;
      }
    }
  }

  changeArticleNo() {
    if (this.itemAdd.article_no_is_general)
      this.itemAdd.article_no_remarks = '';
  }

  btnSave(check: boolean): void {
    this.spinner.show();
    this.itemAdd.cs_machine_model = this.itemAdd.cs_machine_model.toUpperCase();
    this.itemAdd.jig_photo_url = this.mediaUploadImage.url.includes("base64") ? this.mediaUploadImage.url : '';
    this.itemAdd.cs_video_url = this.mediaUploadVideo.url.includes("base64") ? this.mediaUploadVideo.url : '';
    this.itemAdd.sop_setup = String(this.itemAdd.sop_setup) == 'true';
    this.itemAdd.is_critical_process = String(this.itemAdd.is_critical_process) == 'true';
    this.itemAdd.production_adoption = String(this.itemAdd.production_adoption) == 'true';

    this.computerService.createComputerStitchingSetting(this.itemAdd).subscribe(res => {
      this.spinner.hide();
      if (res.success) {
        this.snotify.success(MessageConstants.CREATED_OK_MSG, CaptionConstants.SUCCESS);
        // Check save or save and next
        check ? this.back() : this.clear();
      } else {
        this.snotify.error(res.caption, CaptionConstants.ERROR);
        console.log(res.caption);
      }
    }, error => {
      this.spinner.hide();
      this.snotify.error(MessageConstants.CREATED_ERROR_MSG, CaptionConstants.ERROR);
      console.log(error)
    });
  }

  clear() {
    this.itemAdd = {
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
      sop_setup: null,
      production_adoption: null,
      is_critical_process: null,
      main_upper_material_type_id: '',
      jig_design_id: '',
      jig_design_name: '',
      jig_photo_url: '',
      cs_video_url: '',
      article_no_is_general: false
    } as ComputerStitchingSettingView;
    this.csOperations = [];
    this.mediaUploadImage.reset();
    this.mediaUploadVideo.reset();
  }

  back() {
    this.router.navigate(['/production-bp/transaction/computer-stitching-setting']);
  }

}
