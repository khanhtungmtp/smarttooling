import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { Select2OptionData } from 'ng-select2';
import { NgxSpinnerService } from 'ngx-spinner';
import { CaptionConstants, MessageConstants } from '../../../../../_core/_constants/system.constants';
import { BLLineType } from '../../../../../_core/_models/best-line/bl-line-type';
import { BL_Lines } from '../../../../../_core/_models/best-line/bl-lines';
import { BL_Rollout_Progress } from '../../../../../_core/_models/best-line/bl-rollout-progress';
import { Pagination } from '../../../../../_core/_models/best-line/pagination-best-line';
import { RolloutProgressService } from '../../../../../_core/_services/best-line/rollout-progress.service';
import { NgSnotifyService } from '../../../../../_core/_services/ng-snotify.service';
import { FunctionUtility } from '../../../../../_core/_utility/function-utility';

@UntilDestroy()
@Component({
  selector: 'app-rollout-list',
  templateUrl: './rollout-list.component.html',
  styleUrls: ['./rollout-list.component.css']
})
export class RolloutListComponent implements OnInit {

  dataListRollout: BL_Rollout_Progress[] = [];
  datalistLineNo: Array<Select2OptionData>;
  dataLineNo: string = '';
  dataLineNoTmp: string = '';
  dataLineNoName: string = '';
  datalistLineType: Array<Select2OptionData> = [];
  dataLineType: string = '';
  dataLineTypeName: string = '';
  modelNo: string = '';
  changeLang: string = "0";
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
    private rolloutProgressService: RolloutProgressService,
    private ngSnotifyService: NgSnotifyService,
    private router: Router,
    private route: ActivatedRoute,
    private cd: ChangeDetectorRef,
    private utility: FunctionUtility
  ) { }

  ngOnInit(): void {
    this.rolloutProgressService.currentIDRollout.pipe(untilDestroyed(this))
      .subscribe(res => {
        if(res.editFlag ==='1'){
          this.changeLang = res.changeLang;
          this.dataLineNo = res.line_id;
          this.dataLineNoTmp = res.line_id;
          this.dataLineType = res.line_type_id;
          this.modelNo = res.model;
          this.pagination.currentPage = res.currentPage;
          this.search(1);
        }
      }).unsubscribe();
      this.getLineNo();
  }
  ngAfterViewChecked(): void {
    this.cd.detectChanges();
  }
  getLineNo() {
    this.spinnerService.show();
    this.rolloutProgressService.getLineNo()
      .pipe(untilDestroyed(this))
      .subscribe(data => {
        this.spinnerService.hide();
        this.datalistLineNo = data.map(item => {
          return { id: item.line_id, text: item.line_name };
        });
      }, error => {
        this.spinnerService.hide();
        this.ngSnotifyService.error(MessageConstants.SYSTEM_ERROR_MSG, CaptionConstants.ERROR);
      });
  }
  changeLinelNo(event: any) {
    this.dataLineNo = '';
    if (event !== '' && event !== null) {
      this.dataLineNo = event;
      this.dataLineNoName = this.datalistLineNo.find(x => x.id == event)?.text;
      this.flag = false;
      this.getLineType();
    }
    if(this.dataLineNo !== this.dataLineNoTmp)
    {
      this.dataLineType = null;
      this.dataLineNoTmp = this.dataLineNo;
    }
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
  getLineType() {
    this.spinnerService.show();
    this.rolloutProgressService.getLineType(this.dataLineNo)
      .pipe(untilDestroyed(this))
      .subscribe(data => {
        this.spinnerService.hide();
        this.datalistLineType = data.map(item => {
          return { id: item.line_type_id, text: item.line_type_name };
        });
      }, error => {
        this.spinnerService.hide();
        console.log(error);
      });
  }
  search(check: number) {
    if (this.utility.isEmpty(this.dataLineNo) || this.utility.isEmpty(this.dataLineType)) {
      return this.ngSnotifyService.warning(MessageConstants.SELECT_QUERY_OPTIONS, CaptionConstants.WARNING);
    }
    this.spinnerService.show();
    if (check === 0) {
      this.pagination.currentPage = 1;
    }
    this.rolloutProgressService.getAll(this.pagination.currentPage, this.dataLineNo, this.dataLineType, this.modelNo)
      .pipe(untilDestroyed(this))
      .subscribe(data => {
        this.spinnerService.hide();
        this.dataListRollout = data.result;
        this.pagination = data.pagination;
        if( check === 0)
        {
          this.ngSnotifyService.success(MessageConstants.QUERY_SUCCESS, CaptionConstants.SUCCESS);
        }
      }, error => {
        this.spinnerService.hide();
        this.ngSnotifyService.error(MessageConstants.SYSTEM_ERROR_MSG, CaptionConstants.ERROR)
      });
  }
  clear() {
    this.dataLineNo = '';
    this.dataLineType = '';
    this.modelNo = '';
    this.changeLang = '0';
    this.flag = true;
    this.flagAdd = true;
    this.pagination.currentPage = 1;
    this.getLineNo();
    this.pagination = {} as Pagination;
    this.dataListRollout = [];
    this.rolloutProgressService.changeIDRollout({
      editFlag:'',
    });
    this.ngSnotifyService.success(MessageConstants.CLEAR, CaptionConstants.SUCCESS);
  }
  pageChanged(event: any) {
    this.pagination.currentPage = event.page;
    this.search(1);
  }
  addNew() {
    this.rolloutProgressService.changeIDRollout({
      line_id: this.dataLineNo,
      lineNoName: this.dataLineNoName,
      line_type_id: this.dataLineType,
      lineTypeName: this.dataLineTypeName,
      changeLang: this.changeLang,
      model: this.modelNo,
      currentPage: this.pagination.currentPage,
      editFlag: '',
    });
    this.router.navigate(['/best-line/transaction/rollout-progress/add']);
  }
  edit(item: BL_Rollout_Progress) {
    this.rolloutProgressService.changeIDRollout({
      //id
      line_id: item.line_id,
      line_type_id: item.line_type_id,
      model_no: item.model_no,
      stage_id: item.stage_id,
      operation_id: item.operation_id,
      rollout_line_id: item.rollout_line_id,
      hour: item.hourly_output,
      model: this.modelNo,
      currentPage: this.pagination.currentPage,
      editFlag: '',
      //name
      lineNoName: item.line_name,
      lineTypeName: item.line_type_name,
      rolloutLineName: item.rollout_line_name,
      changeLang: this.changeLang,
    });
    this.router.navigate(['/best-line/transaction/rollout-progress/edit']);
  }
}
