import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { Select2OptionData } from 'ng-select2';
import { NgxSpinnerService } from 'ngx-spinner';
import { CaptionConstants, MessageConstants } from '../../../../../_core/_constants/system.constants';
import { BL_Rollout_Audit } from '../../../../../_core/_models/best-line/bl-rollout-audit';
import { BL_Critical_Process_Analysis } from '../../../../../_core/_models/best-line/bl_critical_process_analysis_audit';
import { ModelOperationRollout } from '../../../../../_core/_models/best-line/model-operation-rollout';
import { ModelRollout } from '../../../../../_core/_models/best-line/model-rollout';
import { Params_Rollout_Progress } from '../../../../../_core/_models/best-line/params-rollout-progress';
import { StageRollout } from '../../../../../_core/_models/best-line/stage-rollout';
import { RolloutAuditService } from '../../../../../_core/_services/best-line/rollout-audit.service';
import { NgSnotifyService } from '../../../../../_core/_services/ng-snotify.service';
import { FunctionUtility } from '../../../../../_core/_utility/function-utility';
@UntilDestroy()
@Component({
  selector: 'app-rollout-view',
  templateUrl: './rollout-view.component.html',
  styleUrls: ['./rollout-view.component.css']
})
export class RolloutViewComponent implements OnInit {

  changeLang: string;
  params_rollout_audit: Params_Rollout_Progress = {
    line_id: '',
    line_type_id: '',
    model_no: '',
    stage_id: '',
    operation_id: '',
    rollout_line_id: '',
  };
  audit_count: number;
  dataLine: Array<Select2OptionData> = [];
  modelRollout: ModelRollout[];
  line_type_name: string;
  dataModelNo: Array<Select2OptionData> =[];
  dataModelName: Array<Select2OptionData> =[];
  line_id: string;
  modelName: string;
  stageRollout: StageRollout[];
  dataStage: Array<Select2OptionData> =[];
  bl_Critical_Process_Analysis: BL_Critical_Process_Analysis = {
    man_remarks: '',
    machine_remarks: '',
    method_remarks: '',
    material_remarks: '',
  } as BL_Critical_Process_Analysis;
  modelOperationRollout: ModelOperationRollout[];
  dataOperationName: Array<Select2OptionData> =[];
  bL_Rollout_Audit: BL_Rollout_Audit = {} as BL_Rollout_Audit;
  audit_result_line_is_pass: string;
  audit_result_operation_is_pass: string;
  model: string;
  edit: string = '1';
  currentPage: number;
  isFlagSave: boolean = true;
  dateTodayOld: Date;
  constructor(
    private spinnerService: NgxSpinnerService,
    private rolloutAuditService: RolloutAuditService,
    private ngSnotifyService: NgSnotifyService,
    private router: Router,
    private route: ActivatedRoute,
    private functionUtility: FunctionUtility,
  ) { }

  ngOnInit(): void {
    this.rolloutAuditService.currentIDRollout.pipe(untilDestroyed(this))
      .subscribe(res => {
        this.changeLang = res.changeLang;
        //id
        this.params_rollout_audit.rollout_line_id = res.rollout_line_id;
        this.params_rollout_audit.line_type_id = res.line_type_id;
        this.params_rollout_audit.model_no = res.model_no;
        this.params_rollout_audit.stage_id = res.stage_id;
        this.params_rollout_audit.operation_id = res.operation_id;
        //text
        this.line_id = res.line_id;
        this.line_type_name = res.line_type_name;
        this.audit_count = res.audit_count;
        //model
        this.model = res.model;
        this.currentPage = res.currentPage;
        if(this.params_rollout_audit.rollout_line_id === '' || this.params_rollout_audit.rollout_line_id === null){
          this.router.navigate(['/best-line/transaction/rollout-audit/list']);
        }
      }).unsubscribe();
    this.dataLine = [
      {
        id: this.params_rollout_audit.rollout_line_id,
        text: this.line_id,
      },
      {
        id: this.params_rollout_audit.line_type_id,
        text: this.line_type_name,
      },
    ];
    this.getModelNo();
    this.getaudit();
  }
  getModelNo() {
    this.spinnerService.show();
    this.rolloutAuditService.getModelNo(this.params_rollout_audit)
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
  }
  changeModelNo(event: any) {
    this.params_rollout_audit.model_no = '';
    if (event !== '' && event !== null) {
      this.params_rollout_audit.model_no = event;
      this.modelName = this.dataModelName.find(x => x.id == event).text;
      this.getStage();
    }
  }
  getStage() {
    this.rolloutAuditService.getStage(this.params_rollout_audit)
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
  getRemarks() {
    this.rolloutAuditService.getRemarks(this.params_rollout_audit)
      .pipe(untilDestroyed(this))
      .subscribe(data => {
        this.bl_Critical_Process_Analysis = data;
      }, error => {
        console.log(error);
      });
  }
  changeStage(event: any) {
    this.params_rollout_audit.stage_id = '';
    if (event !== '' && event !== null) {
      this.params_rollout_audit.stage_id = event;
      this.getOperationName();
      this.getRemarks();
    }
  }
  getOperationName() {
    this.rolloutAuditService.getOperaName(this.params_rollout_audit)
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
  getaudit() {
    this.rolloutAuditService.getAudit(this.params_rollout_audit, this.audit_count)
      .pipe(untilDestroyed(this))
      .subscribe(data => {
        this.bL_Rollout_Audit = data;
        if (this.bL_Rollout_Audit.audit_result_line_is_pass)
          this.audit_result_line_is_pass = '1';
        else
          this.audit_result_line_is_pass = '0';
        if (this.bL_Rollout_Audit.audit_result_operation_is_pass)
          this.audit_result_operation_is_pass = '1';
        else
          this.audit_result_operation_is_pass = '0';
        this.dateTodayOld = data.audit_date;
      }, error => {
        console.log(error);
      });
  }
  backList() {
    this.rolloutAuditService.changeIDRollout({
      changeLang: this.changeLang,
      rollout_line_id: this.params_rollout_audit.rollout_line_id,
      line_type_id: this.params_rollout_audit.line_type_id,
      model: this.model,
      editFlag: this.edit,
      currentPage: this.currentPage,
    });
    this.router.navigate(['/best-line/transaction/rollout-audit/list']);
  }
}
