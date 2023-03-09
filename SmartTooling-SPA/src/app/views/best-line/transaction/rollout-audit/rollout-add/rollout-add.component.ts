import { AfterViewChecked, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { Select2OptionData } from 'ng-select2';
import { NgxSpinnerService } from 'ngx-spinner';
import { BL_Rollout_Audit } from '../../../../../_core/_models/best-line/bl-rollout-audit';
import { BL_Critical_Process_Analysis } from '../../../../../_core/_models/best-line/bl_critical_process_analysis_audit';
import { ModelOperationRollout } from '../../../../../_core/_models/best-line/model-operation-rollout';
import { ModelRollout } from '../../../../../_core/_models/best-line/model-rollout';
import { Params_Rollout_Progress } from '../../../../../_core/_models/best-line/params-rollout-progress';
import { StageRollout } from '../../../../../_core/_models/best-line/stage-rollout';
import { RolloutAuditService } from '../../../../../_core/_services/best-line/rollout-audit.service';
import { NgSnotifyService } from '../../../../../_core/_services/ng-snotify.service';
import { MessageConstants, CaptionConstants } from '../../../../../_core/_constants/system.constants';
import { FunctionUtility } from '../../../../../_core/_utility/function-utility';
@UntilDestroy()
@Component({
  selector: 'app-rollout-add',
  templateUrl: './rollout-add.component.html',
  styleUrls: ['./rollout-add.component.css']
})
export class RolloutAddComponent implements OnInit, AfterViewChecked {
  params_rollout_audit: Params_Rollout_Progress = {
    line_id: '',
    line_type_id: '',
    model_no: '',
    stage_id: '',
    operation_id: '',
    rollout_line_id: ''
  };
  bl_Rollout_Audit: BL_Rollout_Audit = {} as BL_Rollout_Audit;
  changeLang: string;
  rollout_line_name: string;
  line_type_name: string;
  dataidAdd: Array<Select2OptionData> =[];
  dataModelNo: Array<Select2OptionData> =[];
  dataModelName: Array<Select2OptionData> =[];
  dataStage: Array<Select2OptionData> =[];
  dataOperationName: Array<Select2OptionData> =[];
  modelRollout: ModelRollout[];
  stageRollout: StageRollout[];
  modelOperationRollout: ModelOperationRollout[];
  modelName: string;
  bl_Critical_Process_Analysis: BL_Critical_Process_Analysis = {
    man_remarks: '',
    machine_remarks: '',
    method_remarks: '',
    material_remarks: '',
  } as BL_Critical_Process_Analysis;
  audit_result_line_is_pass: string;
  audit_result_operation_is_pass: string;
  edit: string = '1';
  model: string;
  currentPage: number;
  constructor(
    private spinnerService: NgxSpinnerService,
    private rolloutAuditService: RolloutAuditService,
    private ngSnotifyService: NgSnotifyService,
    private router: Router,
    private functionUtility: FunctionUtility,
    private cdr: ChangeDetectorRef
  ) { }
  ngAfterViewChecked(): void {
    this.cdr.detectChanges();
  }
  ngOnInit(): void {
    this.rolloutAuditService.currentIDRollout.pipe(untilDestroyed(this))
      .subscribe(res => {
        this.changeLang = res.changeLang;
        this.params_rollout_audit.rollout_line_id = res.rollout_line_id;
        this.params_rollout_audit.line_type_id = res.line_type_id;
        this.rollout_line_name = res.rollout_line_name;
        this.line_type_name = res.line_type_name;
        this.model = res.model;
        this.currentPage = res.currentPage;
        this.bl_Rollout_Audit.audit_date = new Date();
        if(this.params_rollout_audit.rollout_line_id === null || this.params_rollout_audit.rollout_line_id === ''){
          this.router.navigate(['/best-line/transaction/rollout-audit/list']);
        }
      }).unsubscribe();
    this.dataidAdd = [
      {
        id: this.params_rollout_audit.rollout_line_id,
        text: this.rollout_line_name,
      },
      {
        id: this.params_rollout_audit.line_type_id,
        text: this.line_type_name,
      }
    ];
    this.getModelNo();
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
        this.ngSnotifyService.error(MessageConstants.SYSTEM_ERROR_MSG, CaptionConstants.ERROR)
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
    this.spinnerService.show();
    this.rolloutAuditService.getStage(this.params_rollout_audit)
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
    this.params_rollout_audit.stage_id = '';
    if (event !== '' && event !== null) {
      this.params_rollout_audit.stage_id = event;
      this.getOperationName();
    }
  }

  getAuditCount() {
    this.spinnerService.show();
    this.rolloutAuditService.getAuditCount(this.params_rollout_audit)
      .pipe(untilDestroyed(this))
      .subscribe(data => {
        this.spinnerService.hide();
        this.bl_Rollout_Audit.audit_count = data;
      }, error => {
        this.spinnerService.hide();
        console.log(error);
      });
  }

  getRemarks() {
    this.spinnerService.show();
    this.rolloutAuditService.getRemarks(this.params_rollout_audit)
      .pipe(untilDestroyed(this))
      .subscribe(data => {
        this.spinnerService.hide();
        this.bl_Critical_Process_Analysis = data;
      }, error => {
        this.spinnerService.hide();
        console.log(error);
      });
  }

  getOperationName() {
    this.spinnerService.show();
    this.rolloutAuditService.getOperaName(this.params_rollout_audit)
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
    this.params_rollout_audit.operation_id = '';
    if (event !== '' && event !== null) {
      this.params_rollout_audit.operation_id = event;
      this.bl_Rollout_Audit.rollout_line_id = this.params_rollout_audit.rollout_line_id;
      this.bl_Rollout_Audit.line_type_id = this.params_rollout_audit.line_type_id;
      this.bl_Rollout_Audit.model_no = this.params_rollout_audit.model_no;
      this.bl_Rollout_Audit.stage_id = this.params_rollout_audit.stage_id;
      this.bl_Rollout_Audit.operation_id = this.params_rollout_audit.operation_id;
      this.getRemarks();
      this.getAuditCount();
    }
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

  clean() {
    this.dataModelNo = null;
    this.modelName = '';
    this.dataStage = null;
    this.dataOperationName = null;
    this.bl_Critical_Process_Analysis = {} as BL_Critical_Process_Analysis;
    this.bl_Rollout_Audit = {} as BL_Rollout_Audit;
    this.audit_result_line_is_pass = '';
    this.audit_result_operation_is_pass = '';
    this.bl_Rollout_Audit.audit_date = new Date();
    this.getModelNo();
  }

  checkPass() {
    if (this.audit_result_line_is_pass === '1')
      this.bl_Rollout_Audit.audit_result_line_is_pass = true;
    else
      this.bl_Rollout_Audit.audit_result_line_is_pass = false;
    if (this.audit_result_operation_is_pass === '1')
      this.bl_Rollout_Audit.audit_result_operation_is_pass = true;
    else
      this.bl_Rollout_Audit.audit_result_operation_is_pass = false;
  }

  save() {
    this.checkPass();
    this.spinnerService.show();
    this.bl_Rollout_Audit.audit_date_today = this.functionUtility.getDateFormat(this.bl_Rollout_Audit.audit_date);
    this.rolloutAuditService.addNew(this.bl_Rollout_Audit)
      .subscribe((res) => {
        if (res.success) {
          this.ngSnotifyService.success(MessageConstants.CREATED_OK_MSG, CaptionConstants.SUCCESS);
          this.spinnerService.hide();
          this.backList();
        } else {
          this.spinnerService.hide();
          this.ngSnotifyService.error(MessageConstants.CREATED_ERROR_MSG, CaptionConstants.ERROR);
        }
      });
  }

  saveAndNext() {
    this.checkPass();
    this.spinnerService.show();
    this.bl_Rollout_Audit.audit_date_today = this.functionUtility.getDateFormat(this.bl_Rollout_Audit.audit_date);
    this.rolloutAuditService.addNew(this.bl_Rollout_Audit)
      .subscribe((res) => {
        if (res.success) {
          this.ngSnotifyService.success(MessageConstants.CREATED_OK_MSG, CaptionConstants.SUCCESS);
          this.spinnerService.hide();
          this.clean();
        } else {
          this.spinnerService.hide();
          this.ngSnotifyService.error(MessageConstants.CREATED_ERROR_MSG, CaptionConstants.ERROR);
        }
      });
  }
}
