import { AfterViewChecked, ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { Select2OptionData } from 'ng-select2';
import { NgxSpinnerService } from 'ngx-spinner';
import { environment } from '../../../../../../environments/environment';
import { BL_Lines } from '../../../../../_core/_models/best-line/bl-lines';
import { BL_Rollout_Progress } from '../../../../../_core/_models/best-line/bl-rollout-progress';
import { BL_RolloutHourlyPPH } from '../../../../../_core/_models/best-line/bl-rollouthourlypph';
import { ModelOperationRollout } from '../../../../../_core/_models/best-line/model-operation-rollout';
import { ModelRollout } from '../../../../../_core/_models/best-line/model-rollout';
import { Params_Rollout_Progress } from '../../../../../_core/_models/best-line/params-rollout-progress';
import { StageRollout } from '../../../../../_core/_models/best-line/stage-rollout';
import { RolloutProgressService } from '../../../../../_core/_services/best-line/rollout-progress.service';
import { NgSnotifyService } from '../../../../../_core/_services/ng-snotify.service';
import { FunctionUtility } from '../../../../../_core/_utility/function-utility';
import { MediaUploadComponent } from '../../../../commons/media-upload/media-upload.component';
import { MessageConstants, CaptionConstants } from '../../../../../_core/_constants/system.constants';

@UntilDestroy()
@Component({
  selector: 'app-rollout-add',
  templateUrl: './rollout-add.component.html',
  styleUrls: ['./rollout-add.component.css']
})
export class RolloutAddComponent implements OnInit, AfterViewChecked  {

  @ViewChild('mediaUploadVideo') mediaUploadVideo: MediaUploadComponent;
  @ViewChild('mediaUploadImage') mediaUploadImage: MediaUploadComponent;
  formBL_Rollout_Prrogress: FormGroup;
  bl_rollout_prrogress: BL_Rollout_Progress = {} as BL_Rollout_Progress;
  params_rollout_progress: Params_Rollout_Progress = {
    line_id: '',
    line_type_id: '',
    model_no: '',
    stage_id: '',
    operation_id: '',
    rollout_line_id: ''
  };
  lineNo: string;
  lineNoName: string;
  lineTypeName: string;
  changeLang: string;
  modelRollout: ModelRollout[];
  stageRollout: StageRollout[];
  bL_Lines: BL_Lines[];
  modelOperationRollout: ModelOperationRollout[];
  bl_rolloutHourlyPPH: BL_RolloutHourlyPPH = {
    hourly_output: 0,
    pph: 0,
  };
  dataLine: Array<Select2OptionData> = [];
  dataModelNo: Array<Select2OptionData> = [];
  dataModelName: Array<Select2OptionData> = [];
  modelName: string;
  dataStage: Array<Select2OptionData> = [];
  dataOperationName: Array<Select2OptionData> = [];
  dataRolloutLine: Array<Select2OptionData> =[];
  cTAfter: number;
  baseUrl: string = environment.imageUrl;
  noImage: string = "../../../../../../assets/img/no-image.jpg";
  pph:number;
  hour: number;
  model: string;
  currentPage: number;
  constructor(
    private spinnerService: NgxSpinnerService,
    private rolloutProgressService: RolloutProgressService,
    private ngSnotifyService: NgSnotifyService,
    private router: Router,
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private functionUtility: FunctionUtility,
    private cdr: ChangeDetectorRef
  ) { }
  ngAfterViewChecked(): void {
    this.cdr.detectChanges();
  }

  ngOnInit(): void {
    this.rolloutProgressService.currentIDRollout.pipe(untilDestroyed(this))
      .subscribe(res => {
        this.changeLang = res.changeLang;
        this.params_rollout_progress.line_id = res.line_id;
        this.params_rollout_progress.line_type_id = res.line_type_id;
        this.lineNoName = res.lineNoName;
        this.lineTypeName = res.lineTypeName;
        this.model = res.model;
        this.currentPage = res.currentPage;
        if(this.params_rollout_progress.line_id === null || this.params_rollout_progress.line_id === ''){
          this.router.navigate(['/best-line/transaction/rollout-progress/list']);
        }
      }).unsubscribe();
    this.dataLine = [
      {
        id: this.params_rollout_progress.line_id,
        text: this.lineNoName,
      },
      {
        id: this.params_rollout_progress.line_type_id,
        text: this.lineTypeName,
      }
    ];
    this.getModelNo();
  }
  initForm() {
    this.formBL_Rollout_Prrogress = this.fb.group({
      line_id: ["", Validators.compose([Validators.required])],
    });
  }
  getModelNo() {
    this.spinnerService.show();
    this.rolloutProgressService.getModelNo(this.params_rollout_progress)
      .pipe(untilDestroyed(this))
      .subscribe(data => {
        this.spinnerService.hide();
        this.modelRollout = data;
        this.dataModelNo = this.modelRollout.map(item => {
          return { id: item.model_no, text: item.model_no };
        });
        this.dataModelName = this.modelRollout.map(item => {
          return { id: item.model_no, text: item.model_name };
        });
      }, error => {
        this.spinnerService.hide();
        this.ngSnotifyService.error(MessageConstants.SYSTEM_ERROR_MSG, CaptionConstants.ERROR);
      });
    this.getRolloutLine();
  }
  changeModelNo(event: any) {
    this.params_rollout_progress.model_no = '';
    if (event !== '' && event !== null) {
      this.params_rollout_progress.model_no = event;
      this.modelName = this.dataModelName.find(x => x.id == event).text;
      this.getStage();
      this.getOperationName();
      this.cTAfter = null;
      this.bl_rolloutHourlyPPH.hourly_output = null;
      this.bl_rolloutHourlyPPH.pph = null;
      this.bl_rollout_prrogress.mp_allocated=0;
      this.hour= 0;
    }

  }
  getStage() {
    this.spinnerService.show();
    this.rolloutProgressService.getStage(this.params_rollout_progress)
      .pipe(untilDestroyed(this))
      .subscribe(data => {
        this.spinnerService.hide();
        this.stageRollout = data;
        this.dataStage = this.stageRollout.map(item => {
          return { id: item.stage_id, text: item.stage_name };
        });
      }, error => {
        this.spinnerService.hide();
        console.log(error);
      });
  }
  changeStage(event: any) {
    this.params_rollout_progress.stage_id = '';
    if (event !== '' && event !== null) {
      this.params_rollout_progress.stage_id = event;
      this.getOperationName();
      this.cTAfter = null;
      this.bl_rolloutHourlyPPH.hourly_output = null;
      this.bl_rolloutHourlyPPH.pph = null;
      this.bl_rollout_prrogress.mp_allocated=0;
    }

  }
  getOperationName() {
    this.spinnerService.show();
    this.rolloutProgressService.getOperaName(this.params_rollout_progress)
      .pipe(untilDestroyed(this))
      .subscribe(data => {
        this.spinnerService.hide();
        this.modelOperationRollout = data;
        this.dataOperationName = this.modelOperationRollout.map(item => {
          if (this.changeLang === '0') {
            return { id: item.operation_id, text: item.operation_name_en };
          }
          if (this.changeLang === '1') {
            return { id: item.operation_id, text: item.operation_name_local };
          }
          if (this.changeLang === '2') {
            return { id: item.operation_id, text: item.operation_name_zh };
          }
        });
      }, error => {
        this.spinnerService.hide();
        console.log(error);
      });
  }
  changeOperationName(event: any) {
    this.params_rollout_progress.operation_id = '';
    if (event !== '' && event !== null) {
      this.params_rollout_progress.operation_id = event;
      this.getCTAfter();
      this.gethourlypph();
      this.bl_rollout_prrogress.line_id = this.params_rollout_progress.line_id;
      this.bl_rollout_prrogress.line_type_id = this.params_rollout_progress.line_type_id;
      this.bl_rollout_prrogress.model_no = this.params_rollout_progress.model_no;
      this.bl_rollout_prrogress.stage_id = this.params_rollout_progress.stage_id;
      this.bl_rollout_prrogress.operation_id = this.params_rollout_progress.operation_id;
      this.bl_rollout_prrogress.mp_allocated=0;
    }
  }
  getRolloutLine() {
    this.spinnerService.show();
    this.rolloutProgressService.getRolloutLine()
      .pipe(untilDestroyed(this))
      .subscribe(data => {
        this.spinnerService.hide();
        this.bL_Lines = data;
        this.dataRolloutLine = this.bL_Lines.map(item => {
          return { id: item.line_id, text: item.line_name };
        });
      }, error => {
        this.spinnerService.hide();
        console.log(error);
      });
  }
  getCTAfter() {
    this.spinnerService.show();
    this.rolloutProgressService.getCTAfter(this.params_rollout_progress)
      .pipe(untilDestroyed(this))
      .subscribe(data => {
        this.spinnerService.hide();
        this.cTAfter = data;
      }, error => {
        this.spinnerService.hide();
        console.log(error);
      });
  }
  gethourlypph() {
    this.spinnerService.show();
    this.rolloutProgressService.getHourlyPPH(this.params_rollout_progress)
      .pipe(untilDestroyed(this))
      .subscribe(data => {
        this.spinnerService.hide();
        this.bl_rolloutHourlyPPH = data;
      }, error => {
        this.spinnerService.hide();
        console.log(error);
      });
  }
  changeH(){
    if(this.bl_rollout_prrogress.mp_allocated !== null){
      this.hour = this.bl_rolloutHourlyPPH === null ? 0 : (this.bl_rolloutHourlyPPH.pph * this.bl_rollout_prrogress.mp_allocated);
    }else{
      this.hour =0;
    }
  }
  backList() {
    this.rolloutProgressService.changeIDRollout({
      editFlag: '1',
      line_id: this.params_rollout_progress.line_id,
      line_type_id: this.params_rollout_progress.line_type_id,
      model: this.model,
      changeLang: this.changeLang,
      currentPage: this.currentPage,
    });
    this.router.navigate(['/best-line/transaction/rollout-progress/list']);
  }
  clear() {
    this.dataModelNo = null;
    this.dataModelNo = null;
    this.modelName = '';
    this.dataStage = null;
    this.dataOperationName = null;
    this.dataRolloutLine = null;
    this.cTAfter = null;
    this.bl_rolloutHourlyPPH.hourly_output = 0;
    this.bl_rolloutHourlyPPH.pph = 0;
    this.bl_rollout_prrogress = {} as BL_Rollout_Progress;
    this.mediaUploadVideo.reset();
    this.mediaUploadImage.reset();
    this.getModelNo();
    this.hour = null;
  }

  save(type: string) {
    if (this.mediaUploadVideo.url.includes('base64'))
      this.bl_rollout_prrogress.operation_video_url = this.mediaUploadVideo.url;
    else
      this.bl_rollout_prrogress.operation_video_url = '';

    if (this.mediaUploadImage.url.includes('base64'))
      this.bl_rollout_prrogress.rollout_operation_layout = this.mediaUploadImage.url;
    else
      this.bl_rollout_prrogress.rollout_operation_layout = '';
    this.bl_rollout_prrogress.rollout_date_convert = this.functionUtility.getDateFormat(this.bl_rollout_prrogress.rollout_date);
    this.spinnerService.show();
    this.rolloutProgressService.addNew(this.bl_rollout_prrogress)
      .subscribe((res) => {
        this.spinnerService.hide();
        if (res.success) {
          this.ngSnotifyService.success(MessageConstants.CREATED_OK_MSG, CaptionConstants.SUCCESS);
          type === "SAVE" ? this.backList() : this.clear();
        } else if(res.caption == "Exist") {
          this.ngSnotifyService.error(MessageConstants.DATA_EXIST, CaptionConstants.ERROR);
        } else {
          this.ngSnotifyService.error(MessageConstants.CREATED_ERROR_MSG, CaptionConstants.ERROR);
        }
      }, error => {
        this.spinnerService.hide();
        this.ngSnotifyService.error(MessageConstants.UN_KNOWN_ERROR, CaptionConstants.ERROR);
      });
  }
}
