import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { Select2OptionData } from 'ng-select2';
import { NgxSpinnerService } from 'ngx-spinner';
import { environment } from '../../../../../../environments/environment';
import { MessageConstants, CaptionConstants } from '../../../../../_core/_constants/system.constants';
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
@UntilDestroy()
@Component({
  selector: 'app-rollout-edit',
  templateUrl: './rollout-edit.component.html',
  styleUrls: ['./rollout-edit.component.css']
})
export class RolloutEditComponent implements OnInit {

  @ViewChild('mediaUploadVideo') mediaUploadVideo: MediaUploadComponent;
  @ViewChild('mediaUploadImage') mediaUploadImage: MediaUploadComponent;
  bl_rollout_prrogress: BL_Rollout_Progress = {} as BL_Rollout_Progress;
  params_rollout_progress: Params_Rollout_Progress = {
    line_id: '',
    line_type_id: '',
    model_no: '',
    stage_id: '',
    operation_id: '',
    rollout_line_id: '',
  };
  lineNo: string;
  lineNoName: string;
  lineTypeName: string;
  rolloutLineName: string;
  changeLang: string;
  modelRollout: ModelRollout[];
  stageRollout: StageRollout[];
  bL_Lines: BL_Lines[];
  modelOperationRollout: ModelOperationRollout[];
  bl_rolloutHourlyPPH: BL_RolloutHourlyPPH = {
    hourly_output: 0,
    pph: 0,
  };
  dataLine: Array<Select2OptionData> =[];
  dataModelNo: Array<Select2OptionData> = [];
  dataModelName: Array<Select2OptionData>= [];
  modelName: string;
  dataStage: Array<Select2OptionData> = [];
  dataOperationName: Array<Select2OptionData> = [];
  dataRolloutLine: Array<Select2OptionData> = [];
  cTAfter: number;
  baseUrl: string = environment.imageUrl;
  noImage: string = "../../../../../../assets/img/no-image.jpg";
  dateTnp: Date;
  random: number = Math.random();
  pph: number;
  hour:number;
  edit: string = '1';
  model: string;
  currentPage: number;
  constructor(
    private spinnerService: NgxSpinnerService,
    private rolloutProgressService: RolloutProgressService,
    private ngSnotifyService: NgSnotifyService,
    private router: Router,
    private route: ActivatedRoute,
    private functionUtility: FunctionUtility,
  ) { }

  ngOnInit(): void {
    this.rolloutProgressService.currentIDRollout.pipe(untilDestroyed(this))
      .subscribe(res => {
        this.changeLang = res.changeLang;
        this.params_rollout_progress.line_id = res.line_id;
        this.params_rollout_progress.line_type_id = res.line_type_id;
        this.params_rollout_progress.model_no = res.model_no;
        this.params_rollout_progress.stage_id = res.stage_id;
        this.params_rollout_progress.operation_id = res.operation_id;
        this.params_rollout_progress.rollout_line_id = res.rollout_line_id;
        this.lineNoName = res.lineNoName;
        this.lineTypeName = res.lineTypeName;
        this.rolloutLineName = res.rolloutLineName;
        this.hour = res.hour;
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
      },
      {
        id: this.params_rollout_progress.rollout_line_id,
        text: this.rolloutLineName,
      }
    ];
    this.getModelNo();

  }
  getBLRollout() {
    this.getRolloutLine();
    this.rolloutProgressService.getBLRollout(this.params_rollout_progress)
      .pipe(untilDestroyed(this))
      .subscribe(data => {
        this.dateTnp = data.rollout_date;
        this.bl_rollout_prrogress = data;
      }, error => {
        console.log(error);
      });
    this.dataRolloutLine = this.bL_Lines.filter(x => x.line_id === this.bl_rollout_prrogress.rollout_line_id).map(item => {
      return { id: item.line_id, text: item.line_name };
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
    }

  }
  getStage() {
    this.rolloutProgressService.getStage(this.params_rollout_progress)
      .pipe(untilDestroyed(this))
      .subscribe(data => {
        this.stageRollout = data;
        this.dataStage = this.stageRollout.map(item => {
          return { id: item.stage_id, text: item.stage_name };
        });
      }, error => {
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
    }

  }
  getOperationName() {
    this.rolloutProgressService.getOperaName(this.params_rollout_progress)
      .pipe(untilDestroyed(this))
      .subscribe(data => {
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
        console.log(error);
      });
  }
  changeOperationName(event: any) {
    this.params_rollout_progress.operation_id = '';
    if (event !== '' && event !== null) {
      this.params_rollout_progress.operation_id = event;
      this.getCTAfter();
      this.gethourlypph();
      this.getBLRollout();
      this.bl_rollout_prrogress.line_id = this.params_rollout_progress.line_id;
      this.bl_rollout_prrogress.line_type_id = this.params_rollout_progress.line_type_id;
      this.bl_rollout_prrogress.model_no = this.params_rollout_progress.model_no;
      this.bl_rollout_prrogress.stage_id = this.params_rollout_progress.stage_id;
      this.bl_rollout_prrogress.operation_id = this.params_rollout_progress.operation_id;
    }
  }
  getRolloutLine() {
    this.rolloutProgressService.getRolloutLine()
      .pipe(untilDestroyed(this))
      .subscribe(data => {
        this.bL_Lines = data;
        this.dataRolloutLine = this.bL_Lines.map(item => {
          return { id: item.line_id, text: item.line_name };
        });
      }, error => {
        console.log(error);
      });
  }
  getCTAfter() {
    this.rolloutProgressService.getCTAfter(this.params_rollout_progress)
      .pipe(untilDestroyed(this))
      .subscribe(data => {
        this.cTAfter = data;
      }, error => {
        console.log(error);
      });
  }
  gethourlypph() {
    this.rolloutProgressService.getHourlyPPH(this.params_rollout_progress)
      .pipe(untilDestroyed(this))
      .subscribe(data => {
        this.bl_rolloutHourlyPPH = data;
      }, error => {
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
      editFlag: this.edit,
      line_id: this.params_rollout_progress.line_id,
      line_type_id: this.params_rollout_progress.line_type_id,
      model: this.model,
      changeLang: this.changeLang,
      currentPage: this.currentPage,
    });
    this.router.navigate(['/best-line/transaction/rollout-progress/list']);
  }
  checkDate() {
    if (this.dateTnp !== this.bl_rollout_prrogress.rollout_date)
      this.bl_rollout_prrogress.rollout_date_convert = this.functionUtility.getDateFormat(this.bl_rollout_prrogress.rollout_date);
  }
  save() {
    if (this.mediaUploadVideo.url.includes('base64'))
      this.bl_rollout_prrogress.operation_video_url = this.mediaUploadVideo.url;
    else
      this.bl_rollout_prrogress.operation_video_url = '';

    if (this.mediaUploadImage.url.includes('base64'))
      this.bl_rollout_prrogress.rollout_operation_layout = this.mediaUploadImage.url;
    else
      this.bl_rollout_prrogress.rollout_operation_layout = '';
    this.checkDate();
    this.spinnerService.show();
    this.rolloutProgressService.upDate(this.bl_rollout_prrogress)
      .subscribe((res) => {
        if (res.success) {
          this.ngSnotifyService.success(MessageConstants.UPDATED_OK_MSG, CaptionConstants.SUCCESS);
          this.spinnerService.hide();
          this.backList();
        } else {
          this.spinnerService.hide();
          this.ngSnotifyService.error(MessageConstants.UPDATED_ERROR_MSG, CaptionConstants.ERROR);
        }
      });
  }

}
