import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { Select2OptionData } from 'ng-select2';
import { NgxSpinnerService } from 'ngx-spinner';
import { BLCriticalProcessAnalysis } from '../../../../../_core/_models/best-line/bl-critical-process-analysis';
import { BLLineType } from '../../../../../_core/_models/best-line/bl-line-type';
import { BL_Lines } from '../../../../../_core/_models/best-line/bl-lines';
import { Stage } from '../../../../../_core/_models/best-line/stage';
import { Model } from '../../../../../_core/_models/smart-tool/model';
import { ModelOperation } from '../../../../../_core/_models/smart-tool/model-operation';
import { UserForLogged } from '../../../../../_core/_models/user-for-logged';
import { CriticalProcessAnalysisService } from '../../../../../_core/_services/best-line/critical-process-analysis.service';
import { NgSnotifyService } from '../../../../../_core/_services/ng-snotify.service';
import { MediaUploadComponent } from '../../../../commons/media-upload/media-upload.component';
import { MessageConstants, CaptionConstants, ActionConstants } from '../../../../../_core/_constants/system.constants';
import { CriticalProcessParam } from '../../../../../_core/_helpers/params/best-line/critical-process-param';

@UntilDestroy()
@Component({
  selector: 'app-critical-process-analysis-add',
  templateUrl: './critical-process-analysis-add.component.html',
  styleUrls: ['./critical-process-analysis-add.component.scss']
})
export class CriticalProcessAnalysisAddComponent implements OnInit {
  @ViewChild('BestPractice') bestPractice: MediaUploadComponent;
  @ViewChild('ManMedia') manMedia: MediaUploadComponent;
  @ViewChild('MethodMedia') methodMedia: MediaUploadComponent;
  @ViewChild('MachineMedia') machineMedia: MediaUploadComponent;
  @ViewChild('MaterialMedia') materialMedia: MediaUploadComponent;

  ModelOperation: ModelOperation[] = [];
  criticalProcessData: BLCriticalProcessAnalysis = {} as BLCriticalProcessAnalysis;
  currentUser: UserForLogged = JSON.parse(localStorage.getItem('userSmartTooling'));
  stage: Stage[] = [];
  model: Model[] = [];
  modelName: string;
  bL_Lines: BL_Lines[] = [];
  bL_Line_Type: BLLineType[] = [];
  dataLine: Array<Select2OptionData> = [];
  dataLineNo: string;
  dataListStage: Array<Select2OptionData> = [];
  dataListModel: Array<Select2OptionData> = [];
  dataOperationName: Array<Select2OptionData>;
  lineNo: string;
  lineNoName: string;
  lineType: string;
  changeLang: string;
  currentPage: number;
  checkboxInvalid: boolean = false;
  flag = '0';
  noImage: string = "../../../../../../assets/img/no-image.jpg";
  modelsearch: string = '';
  editFlag: string = '1';
  stageSearch: string = '';
  criticalProcessParam: CriticalProcessParam = {
    factory_id: '',
    line_id: '',
    line_type_id: '',
    model_no: '',
    operation_id: '',
    stage_id: ''
  }

  constructor(
    private router: Router,
    private spinnerService: NgxSpinnerService,
    private snotifyService: NgSnotifyService,
    private criticalProcessServ: CriticalProcessAnalysisService,
  ) { }

  ngOnInit() {
    this.criticalProcessServ.currentFlag.subscribe(flag => this.flag = flag);
    if (this.flag === '1') {
      this.checkboxInvalid = this.criticalProcessData.invalid === '1' ? true : false;
    }

    this.criticalProcessServ.currentIDCritical.pipe(untilDestroyed(this)).subscribe((res) => {
      this.changeLang = res.changeLang;
      this.criticalProcessParam.line_id = res.line_id;
      this.criticalProcessParam.factory_id = res.factory_id;
      this.criticalProcessParam.line_type_id = res.line_type_id;
      this.criticalProcessParam.model_no = res.model_no;
      this.criticalProcessParam.stage_id = res.stage_id;
      this.criticalProcessParam.operation_id = res.operation_id;
      this.modelsearch = res.model;
      this.stageSearch = res.stageSearch;

      this.criticalProcessData.line_id = res.line_id;
      this.criticalProcessData.line_type_id = res.line_type_id;

      this.lineNo = res.line_name;
      this.lineNoName = res.line_name;
      this.lineType = res.line_type_name;
      this.currentPage = res.currentPage;
      if (this.criticalProcessParam.line_id === null || this.criticalProcessParam.line_id === '') {
        this.router.navigate(['/best-line/transaction/critical-process-analysis/main']);
      }
    })

    this.dataLine = [
      {
        id: this.criticalProcessData.line_id,
        text: this.lineNoName,
      },
      {
        id: this.criticalProcessData.line_type_id,
        text: this.lineType,
      }
    ];

    console.log(this.dataLine);


    this.getModelNo();
    this.getStage();
  }

  getModelNo() {
    this.spinnerService.show();
    this.criticalProcessServ.getModel()
      .pipe(untilDestroyed(this))
      .subscribe(data => {
        this.spinnerService.hide();
        this.model = data;
        this.dataListModel = this.model.map(item => {
          return { id: item.model_no, text: item.model_no };
        });
      }, error => {
        this.spinnerService.hide();
        this.snotifyService.error(MessageConstants.SYSTEM_ERROR_MSG, CaptionConstants.ERROR);
      });
    this.getStage();
  }

  getStage() {
    this.spinnerService.show();
    this.criticalProcessServ.getStage()
      .pipe(untilDestroyed(this))
      .subscribe(data => {
        this.spinnerService.hide();
        this.stage = data;
        this.dataListStage = this.stage.map(item => {
          return { id: item.stage_id, text: item.stage_name };
        });
      }, error => {
        this.spinnerService.hide();
        console.log(error);
      });
  }

  changeModelNo(event: any) {
    this.criticalProcessData.model_no = '';
    if (event !== '' && event !== null) {
      this.criticalProcessData.model_no = event;
      this.modelName = this.model.find(x => x.model_no == event).model_name;
      this.getStage();
    }
  }

  changeStage(event: any) {
    this.criticalProcessData.stage_id = '';
    if (event !== '' && event !== null) {
      this.criticalProcessData.stage_id = event;
      this.getOperationName();
    }
  }

  getOperationName() {
    this.spinnerService.show();
    this.criticalProcessServ.getOperationName(this.criticalProcessData.model_no, this.criticalProcessData.stage_id)
      .pipe(untilDestroyed(this))
      .subscribe(data => {
        this.spinnerService.hide();
        this.ModelOperation = data;
        this.dataOperationName = this.ModelOperation.map(item => {
          if (this.changeLang === '2') {
            return { id: item.operation_id, text: item.operation_name_en };
          }
          if (this.changeLang === '1') {
            return { id: item.operation_id, text: item.operation_name_local };
          }
          if (this.changeLang === '3') {
            return { id: item.operation_id, text: item.operation_name_zh };
          }
        });
      }, error => {
        this.spinnerService.hide();
        console.log(error);
      });
  }

  changeOperationName(event: any) {
    this.criticalProcessData.operation_id = '';
    if (event !== '' && event !== null) {
      this.criticalProcessData.operation_id = event;
    }
  }

  saveAndNext() {
    // Validate values
    if (!this.validateForm()) {
      return;
    }
    // Format values
    this.formValue();
    this.criticalProcessServ.add(this.criticalProcessData).subscribe(res => {
      if (res.success) {
        this.snotifyService.success(MessageConstants.CREATED_OK_MSG, CaptionConstants.SUCCESS);
        this.clearData();
      } else {
        this.snotifyService.error(MessageConstants.CREATED_ERROR_MSG, CaptionConstants.ERROR);
        if (res.caption) {
          console.log(res.caption);
        }
      }
    }, error => {
      console.log(error);
    });
  }

  clearData() {
    this.criticalProcessData = <BLCriticalProcessAnalysis>{
      line_id: this.dataLine.map(x => x.id)[0],
      line_type_id: this.dataLine.map(x => x.id)[1],
      best_practice_url: '',
      ct_after_sec: null,
      ct_before_sec: null,
      takt_time: null,
      model_no: null,
      stage_id: "",
      operation_id: "",
      machine_remarks: "",
      man_remarks: "",
      method_remarks: "",
      material_remarks: "",
      machine_media_url: "",
      method_media_url: "",
      material_media_url: "",
      man_media_url: "",
    };

    this.bestPractice.reset();
    this.manMedia.reset();
    this.machineMedia.reset();
    this.materialMedia.reset();
    this.methodMedia.reset();

    this.modelName = "";
  }

  save() {
    // Validate values
    if (!this.validateForm()) {
      return;
    }
    // Format values
    this.formValue();
    this.criticalProcessServ.add(this.criticalProcessData).subscribe(res => {
      if (res.success) {
        this.snotifyService.success(MessageConstants.CREATED_OK_MSG, CaptionConstants.SUCCESS);
        this.back();
      } else {
        this.snotifyService.error(MessageConstants.CREATED_ERROR_MSG, CaptionConstants.ERROR);
        if (res.caption) {
          console.log(res.caption);
        }
      }
    }, error => {
      console.log(error);
    });
  }

  validateForm() {
    if (this.criticalProcessData.model_no === null || this.criticalProcessData.model_no.trim() === '') {
      this.snotifyService.warning('Please check model no.', 'Invalid');
      return false;
    }
    if (this.criticalProcessData.line_id === null || this.criticalProcessData.line_id.trim() === '') {
      this.snotifyService.warning('Please check line no.', 'Invalid');
      return false;
    }
    if (this.criticalProcessData.line_type_id === null || this.criticalProcessData.line_type_id.trim() === '') {
      this.snotifyService.warning('Please check line type.', 'Invalid');
      return false;
    }
    if (this.criticalProcessData.stage_id === null || this.criticalProcessData.stage_id.trim() === '') {
      this.snotifyService.warning('Please check model_no.', 'Invalid');
      return false;
    }
    if (Number.isNaN(this.criticalProcessData.ct_after_sec)) {
      this.snotifyService.warning('Please enter ct before.', 'Invalid');
      return false;
    }
    if (Number.isNaN(this.criticalProcessData.ct_after_sec)) {
      this.snotifyService.warning('Please enter ct after.', 'Invalid');
      return false;
    }
    if (Number.isNaN(this.criticalProcessData.takt_time)) {
      this.snotifyService.warning('Please enter takl time.', 'Invalid');
      return false;
    }
    return true;
  }

  formValue() {
    let bestPractice = this.bestPractice.url;
    let manMedia = this.manMedia.url;
    let machineMedia = this.machineMedia.url;
    let materialMedia = this.materialMedia.url;
    let methodMedia = this.methodMedia.url;

    this.criticalProcessData.best_practice_url = bestPractice;
    this.criticalProcessData.man_media_url = manMedia;
    this.criticalProcessData.method_media_url = methodMedia;
    this.criticalProcessData.machine_media_url = machineMedia;
    this.criticalProcessData.material_media_url = materialMedia;
  }

  back() {
    this.criticalProcessServ.changeIDCritical({
      editFlag: this.editFlag,
      line_id: this.criticalProcessParam.line_id,
      line_type_id: this.criticalProcessParam.line_type_id,
      model: this.modelsearch,
      operation_id: this.criticalProcessParam.operation_id,
      changeLang: this.changeLang,
      stageSearch: this.stageSearch,
    })
    this.router.navigate(['/best-line/transaction/critical-process-analysis/main']);
  }

  checkValue(event: any) {
    // Prevent invalid characters
    if (event.key === '-' || event.key === '+' || event.key === 'e' || event.key === 'E' || event.key === '.') {
      event.preventDefault();
    }
  }

}
