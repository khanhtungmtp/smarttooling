import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { Select2OptionData } from 'ng-select2';
import { NgxSpinnerService } from 'ngx-spinner';
import { CaptionConstants, MessageConstants } from '../../../../../_core/_constants/system.constants';
import { BLLineType } from '../../../../../_core/_models/best-line/bl-line-type';
import { BL_Lines } from '../../../../../_core/_models/best-line/bl-lines';
import { BL_Rollout_Audit } from '../../../../../_core/_models/best-line/bl-rollout-audit';
import { Pagination } from '../../../../../_core/_models/best-line/pagination-best-line';
import { RolloutAuditService } from '../../../../../_core/_services/best-line/rollout-audit.service';
import { NgSnotifyService } from '../../../../../_core/_services/ng-snotify.service';
import { FunctionUtility } from '../../../../../_core/_utility/function-utility';
@UntilDestroy()
@Component({
  selector: 'app-rollout-list',
  templateUrl: './rollout-list.component.html',
  styleUrls: ['./rollout-list.component.css']
})
export class RolloutListComponent implements OnInit {

  bL_Rollout_Audit: BL_Rollout_Audit[] = [];
  datalistRolloutLineNo: Array<Select2OptionData> = [];
  dataRolloutLineNo: string = '';
  dataRolloutLineNoTmp: string = '';
  dataRolloutLineName: string = '';
  datalistLineType: Array<Select2OptionData> = [];
  dataLineType: string = '';
  dataLineTypeName: string = '';
  changeLang: string = "0";
  model: string = '';
  flag: boolean = true;
  flagAdd: boolean = true;
  pagination: Pagination = {
    currentPage: 1,
    pageSize: 10,
    totalCount: 0,
    totalPage: 0,
  };
  message = MessageConstants;
  constructor(
    private spinnerService: NgxSpinnerService,
    private rolloutAuditService: RolloutAuditService,
    private ngSnotifyService: NgSnotifyService,
    private router: Router,
    private route: ActivatedRoute,
    private cd: ChangeDetectorRef,
    private utility: FunctionUtility
  ) { }

  ngOnInit(): void {
    this.rolloutAuditService.currentIDRollout.pipe(untilDestroyed(this))
      .subscribe(res => {
        if (res.editFlag === '1') {
          this.dataRolloutLineNo = res.rollout_line_id;
          this.dataRolloutLineNoTmp = res.rollout_line_id;
          this.dataLineType = res.line_type_id;
          this.model = res.model;
          this.changeLang = res.changeLang;
          this.pagination.currentPage = res.currentPage;
          this.search(1);
        }
      }).unsubscribe();
    //   this.getRolloutLineNo();
  }
  ngAfterViewChecked(): void {
    this.cd.detectChanges();
  }
  // getRolloutLineNo() {
  //   this.spinnerService.show();
  //   this.rolloutAuditService.getRolloutLineNo()
  //     .pipe(untilDestroyed(this))
  //     .subscribe(data => {
  //       this.spinnerService.hide();
  //       this.datalistRolloutLineNo = data.map(item => {
  //         return { id: item.rollout_line_id, text: item.line_id };
  //       });
  //     }, error => {
  //       this.spinnerService.hide();
  //       this.ngSnotifyService.error(MessageConstants.SYSTEM_ERROR_MSG, CaptionConstants.ERROR);
  //     });
  // }
  changeRolloutLineNo(event: any) {
    this.dataRolloutLineNo = '';
    if (event !== '' && event !== null) {
      this.dataRolloutLineNo = event;
      this.dataRolloutLineName = this.datalistRolloutLineNo.find(x => x.id == event)?.text;
      this.flag = false;
      this.getLineType();
    }
    if (this.dataRolloutLineNo !== this.dataRolloutLineNoTmp) {
      this.dataLineType = null;
      this.dataRolloutLineNoTmp = this.dataRolloutLineNo;
    }
  }
  getLineType() {
    this.rolloutAuditService.getLineType(this.dataRolloutLineNo)
      .pipe(untilDestroyed(this))
      .subscribe(data => {
        this.datalistLineType = data.map(item => {
          return { id: item.line_type_id, text: item.line_type_name };
        });
      }, error => {
        console.log(error);
      });
  }
  changeLineType(event: any) {
    this.dataLineType = '';
    if (event !== '' && event !== null) {
      this.dataLineType = event;
      this.dataLineTypeName = this.datalistLineType.find(x => x.id == event)?.text;
      this.flagAdd = false;
    } else {
      this.flagAdd = true;
    }
  }
  search(check: number) {
    if (this.utility.isEmpty(this.dataRolloutLineNo) || this.utility.isEmpty(this.dataLineType)) {
      return this.ngSnotifyService.warning(MessageConstants.SELECT_QUERY_OPTIONS, CaptionConstants.WARNING);
    }
    this.spinnerService.show();
    if (check === 0) {
      this.pagination.currentPage = 1;
    }
    this.rolloutAuditService.getAll(this.pagination.currentPage, this.dataRolloutLineNo, this.dataLineType, this.model)
      .pipe(untilDestroyed(this))
      .subscribe(data => {
        this.spinnerService.hide();
        this.bL_Rollout_Audit = data.result;
        this.pagination = data.pagination;
        if (check === 0) {
          this.ngSnotifyService.success(MessageConstants.QUERY_SUCCESS, CaptionConstants.SUCCESS);
        }
      }, error => {
        this.spinnerService.hide();
        this.ngSnotifyService.error(MessageConstants.SYSTEM_ERROR_MSG, CaptionConstants.ERROR)
      });
  }
  clear() {
    this.dataRolloutLineNo = '';
    this.dataLineType = '';
    this.model = '';
    this.changeLang = '0';
    this.flag = true;
    this.flagAdd = true;
    this.pagination.currentPage = 1;
    //  this.getRolloutLineNo();
    this.pagination = {} as Pagination;
    this.bL_Rollout_Audit = [];
    this.rolloutAuditService.changeIDRollout({
      editFlag: '',
    });
    this.ngSnotifyService.success(MessageConstants.CLEAR, CaptionConstants.SUCCESS);
  }
  addNew() {
    this.rolloutAuditService.changeIDRollout({
      rollout_line_id: this.dataRolloutLineNo,
      rollout_line_name: this.dataRolloutLineName,
      line_type_id: this.dataLineType,
      line_type_name: this.dataLineTypeName,
      changeLang: this.changeLang,
      model: this.model,
      editFlag: '',
      currentPage: this.pagination.currentPage,
    });
    this.router.navigate(['/best-line/transaction/rollout-audit/add']);
  }
  pageChanged(event: any) {
    this.pagination.currentPage = event.page;
    this.search(1);
  }
  viewAudit(item: BL_Rollout_Audit) {
    this.rolloutAuditService.changeIDRollout({
      changeLang: this.changeLang,
      //id
      rollout_line_id: item.rollout_line_id,
      line_type_id: item.line_type_id,
      model_no: item.model_no,
      stage_id: item.stage_id,
      operation_id: item.operation_id,
      audit_count: item.audit_count,
      model: this.model,
      currentPage: this.pagination.currentPage,
      editFlag: '',
      //name
      line_id: item.line_id,
      line_type_name: item.line_type_name,
    });
    this.router.navigate(['/best-line/transaction/rollout-audit/view']);
  }

}
