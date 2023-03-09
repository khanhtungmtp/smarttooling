import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { Select2OptionData } from 'ng-select2';
import { NgxSpinnerService } from 'ngx-spinner';
import { environment } from '../../../../../../environments/environment';
import { CriticalProcessParam } from '../../../../../_core/_helpers/params/best-line/critical-process-param';
import { BLCriticalProcessAnalysis } from '../../../../../_core/_models/best-line/bl-critical-process-analysis';
import { Stage } from '../../../../../_core/_models/best-line/stage';
import { Model } from '../../../../../_core/_models/smart-tool/model';
import { ModelOperation } from '../../../../../_core/_models/smart-tool/model-operation';
import { CriticalProcessAnalysisService } from '../../../../../_core/_services/best-line/critical-process-analysis.service';
import { NgSnotifyService } from '../../../../../_core/_services/ng-snotify.service';
import { MediaUploadComponent } from '../../../../commons/media-upload/media-upload.component';
import { MessageConstants, CaptionConstants, ActionConstants } from '../../../../../_core/_constants/system.constants';

@UntilDestroy()
@Component({
  selector: 'app-critical-process-analysis-edit',
  templateUrl: './critical-process-analysis-edit.component.html',
  styleUrls: ['./critical-process-analysis-edit.component.scss']
})
export class CriticalProcessAnalysisEditComponent implements OnInit {
  @ViewChild('BestPractice') bestPractice: MediaUploadComponent;
  @ViewChild('ManMedia') manMedia: MediaUploadComponent;
  @ViewChild('MethodMedia') methodMedia: MediaUploadComponent;
  @ViewChild('MachineMedia') machineMedia: MediaUploadComponent;
  @ViewChild('MaterialMedia') materialMedia: MediaUploadComponent;

  ModelOperation: ModelOperation[] = [];

  criticalProcessData: BLCriticalProcessAnalysis = {} as BLCriticalProcessAnalysis;
  stage: Stage[] = [];
  model: Model[] = [];
  baseUrl: string = environment.imageUrl;
  noImage: string = "../../../../../../assets/img/no-image.jpg";
  typeImage: string = 'image/*';
  typeVideo: string = 'video/mp4';
  lineNo: string;
  lineType: string;
  changeLang: string;
  checkboxInvalid: boolean = false;
  flag = '0';
  dataLine: Array<Select2OptionData> = [];
  dataLineNo: string;
  dataListStage: Array<Select2OptionData> = [];
  dataListModel: Array<Select2OptionData> = [];
  dataOperationName: Array<Select2OptionData>;
  modelName: string = '';
  modelsearch: string = '';
  editFlag: string = '1';
  stageSearch: string = '';
  random: number = Math.random();
  criticalProcessParam: CriticalProcessParam = {
    factory_id: '',
    line_id: '',
    line_type_id: '',
    model_no: '',
    operation_id: '',
    stage_id: ''
  }

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private spinnerService: NgxSpinnerService,
    private snotifyService: NgSnotifyService,
    private criticalProcessServ: CriticalProcessAnalysisService,
  ) { }

  ngOnInit() {
    this.criticalProcessServ.currentIDCritical
      .pipe(untilDestroyed(this))
      .subscribe(res => {
        this.changeLang = res.changeLang;
        this.criticalProcessParam.line_id = res.line_id;
        this.criticalProcessParam.factory_id = res.factory_id;
        this.criticalProcessParam.line_type_id = res.line_type_id;
        this.criticalProcessParam.model_no = res.model_no;
        this.criticalProcessParam.stage_id = res.stage_id;
        this.criticalProcessParam.operation_id = res.operation_id;
        this.modelsearch = res.model;
        this.stageSearch = res.stageSearch;

        this.lineNo = res.line_name;
        this.lineType = res.line_type_name;
        this.criticalProcessData = res;
        if (this.criticalProcessParam.line_id === null || this.criticalProcessParam.line_id === '') {
          this.router.navigate(['/best-line/transaction/critical-process-analysis/main']);
        }
      });

    this.dataLine = [
      {
        id: this.criticalProcessData.line_id,
        text: this.lineNo,
      },
      {
        id: this.criticalProcessData.line_type_id,
        text: this.lineType,
      }
    ];
    this.getModelNo();
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
      this.getOperationName();
    }
  }

  changeStage(event: any) {
    this.criticalProcessData.stage_id = '';
    if (event !== '' && event !== null) {
      this.criticalProcessParam.stage_id = event;
      this.getOperationName();
    }
  }

  getOperationName() {
    this.spinnerService.show();
    this.criticalProcessServ.getOperationName(this.criticalProcessParam.model_no, this.criticalProcessParam.stage_id)
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
      this.criticalProcessParam.operation_id = event;
      this.criticalProcessData.factory_id = this.criticalProcessParam.factory_id;
      this.criticalProcessData.model_no = this.criticalProcessParam.model_no;
      this.criticalProcessData.stage_id = this.criticalProcessParam.stage_id;
      this.criticalProcessData.line_type_id = this.criticalProcessParam.line_type_id;
      this.criticalProcessData.line_id = this.criticalProcessParam.line_id;
    }
  }
  save() {
    if (this.bestPractice.url.includes('base64'))
      this.criticalProcessData.best_practice_url = this.bestPractice.url;
    else
      this.criticalProcessData.best_practice_url = "";

    if (this.manMedia.url.includes('base64'))
      this.criticalProcessData.man_media_url = this.manMedia.url;
    else
      this.criticalProcessData.man_media_url = "";

    if (this.machineMedia.url.includes('base64'))
      this.criticalProcessData.machine_media_url = this.machineMedia.url;
    else
      this.criticalProcessData.machine_media_url = "";

    if (this.materialMedia.url.includes('base64'))
      this.criticalProcessData.material_media_url = this.materialMedia.url;
    else
      this.criticalProcessData.material_media_url = "";

    if (this.methodMedia.url.includes('base64'))
      this.criticalProcessData.method_media_url = this.methodMedia.url;
    else
      this.criticalProcessData.method_media_url = "";

    this.criticalProcessData.operation_id = this.criticalProcessParam.operation_id;

    this.spinnerService.show();
    this.criticalProcessServ.upDate(this.criticalProcessData)
      .subscribe((res) => {
        if (res.success) {
          this.snotifyService.success(MessageConstants.UPDATED_OK_MSG, CaptionConstants.SUCCESS);
          this.spinnerService.hide();
          this.router.navigateByUrl('/best-line/transaction/critical-process-analysis/main');
        } else {
          this.spinnerService.hide();
          this.snotifyService.error(MessageConstants.UPDATED_ERROR_MSG, CaptionConstants.ERROR);
        }
      });
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
