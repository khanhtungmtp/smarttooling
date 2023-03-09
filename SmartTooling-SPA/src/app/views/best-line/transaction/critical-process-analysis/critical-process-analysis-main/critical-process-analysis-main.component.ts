import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { Select2OptionData } from 'ng-select2';
import { NgxSpinnerService } from 'ngx-spinner';
import { CaptionConstants, MessageConstants } from '../../../../../_core/_constants/system.constants';
import { SearchBLCriticalParam } from '../../../../../_core/_helpers/params/best-line/search-critical-Param';
import { BLCriticalProcess } from '../../../../../_core/_models/best-line/bl-critical-process';
import { BLLineType } from '../../../../../_core/_models/best-line/bl-line-type';
import { BL_Lines } from '../../../../../_core/_models/best-line/bl-lines';
import { Pagination } from '../../../../../_core/_models/best-line/pagination-best-line';
import { Stage } from '../../../../../_core/_models/best-line/stage';
import { CriticalProcessAnalysisService } from '../../../../../_core/_services/best-line/critical-process-analysis.service';
import { NgSnotifyService } from '../../../../../_core/_services/ng-snotify.service';
import { FunctionUtility } from '../../../../../_core/_utility/function-utility';

@UntilDestroy()
@Component({
  selector: 'app-critical-process-analysis-main',
  templateUrl: './critical-process-analysis-main.component.html',
  styleUrls: ['./critical-process-analysis-main.component.scss']
})
export class CriticalProcessAnalysisMainComponent implements OnInit {
  criticalProcessData: BLCriticalProcess[] = [];
  stage: Stage[] = [];
  bL_Lines: BL_Lines[] = [];
  bL_Line_Type: BLLineType[] = [];
  dataListLineNo: Array<Select2OptionData> = [];
  dataLineNoName: string = '';
  dataLineNo: string;
  datalistLineType: Array<Select2OptionData> = [];
  dataListStage: Array<Select2OptionData> = [];
  dataLineType: string;
  dataLineTypeName: string;
  modelNo: string = '';
  changeLang: string = "1";
  flag: boolean = true;
  flagAdd: boolean = true;
  editFlag: string = '';
  LinetypeTemp: string = '';
  noData: string = MessageConstants.NO_DATA;
  pagination: Pagination = {
    currentPage: 1,
    pageSize: 10,
    totalCount: 0,
    totalPage: 0,
  };
  searchBLCriticalParam: SearchBLCriticalParam = {
    lineNo: '',
    lineType: '',
    modelNo: '',
    stage: '',

  };
  message = MessageConstants;
  constructor(
    private cd: ChangeDetectorRef,
    private utility: FunctionUtility,
    private spinnerService: NgxSpinnerService,
    private criticalProcessServ: CriticalProcessAnalysisService,
    private router: Router,
    private snotifyService: NgSnotifyService,
  ) { }

  ngOnInit() {
    this.criticalProcessServ.currentIDCritical
      .pipe(untilDestroyed(this))
      .subscribe(res => {
        if (res.editFlag === '1') {
          this.changeLang = res.changeLang;
          this.searchBLCriticalParam.lineNo = res.line_id;
          this.searchBLCriticalParam.lineType = res.line_type_id;
          this.LinetypeTemp = res.line_id;
          this.searchBLCriticalParam.modelNo = res.model;
          this.searchBLCriticalParam.stage = res.stageSearch;
          this.editFlag = res.editFlag;
          this.search(1)
        }
      }).unsubscribe();
    this.getLineNo();
    this.getStage()
  }


  search(check: number) {
    if (check === 0) {
      this.pagination.currentPage = 1;
    }
    if (this.utility.isEmpty(this.searchBLCriticalParam.lineNo) || this.utility.isEmpty(this.searchBLCriticalParam.lineType)) {
      this.snotifyService.warning(MessageConstants.SELECT_QUERY_OPTIONS, CaptionConstants.WARNING);
    } else {
      this.searchBLCriticalParam.modelNo = this.searchBLCriticalParam.modelNo.toUpperCase();
      this.spinnerService.show();
      this.criticalProcessServ.getAll(this.searchBLCriticalParam, this.pagination)
        .pipe(untilDestroyed(this))
        .subscribe(data => {
          this.spinnerService.hide();
          if (check === 0) {
            this.snotifyService.success(MessageConstants.QUERY_SUCCESS, CaptionConstants.SUCCESS);
          }
          this.criticalProcessData = data.result;
          this.pagination = data.pagination;
        }, error => {
          this.spinnerService.hide();
          console.log(error);
        });
    }
  }

  getLineNo() {
    this.spinnerService.show();
    this.criticalProcessServ.getLineNo()
      .pipe(untilDestroyed(this))
      .subscribe(data => {
        this.spinnerService.hide();
        this.bL_Lines = data;
        this.dataListLineNo = this.bL_Lines.map(item => {
          return { id: item.line_id, text: item.line_name };
        });
      }, error => {
        this.spinnerService.hide();
        this.snotifyService.error(MessageConstants.SYSTEM_ERROR_MSG, CaptionConstants.ERROR);
      });
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
    this.dataLineNo = '';
    if (event != '' && event != null) {
      this.dataLineNo = event;
      this.getLineType();
    }
  }
  getLineType() {
    this.spinnerService.show();
    this.criticalProcessServ.getLineType(this.dataLineNo)
      .pipe(untilDestroyed(this))
      .subscribe(data => {
        this.spinnerService.hide();
        this.bL_Line_Type = data;
        this.datalistLineType = this.bL_Line_Type.map(item => {
          return { id: item.line_type_id, text: item.line_type_name };
        });
      }, error => {
        this.spinnerService.hide();
        console.log(error);
      });
  }

  changeLinelNo(event: any) {
    this.dataLineNo = '';
    if (event !== '' && event !== null) {
      this.dataLineNo = event;
      this.dataLineNoName = this.dataListLineNo.find(x => x.id == event).text;
      this.flag = false;
      this.getLineType();
    }
    if (this.dataLineNo != this.LinetypeTemp) {
      this.searchBLCriticalParam.lineType = null;
      this.LinetypeTemp = this.dataLineNo;
    }
  }
  changeLineType(event: any) {
    this.dataLineType = '';
    if (event !== '' && event !== null) {
      this.dataLineType = event;
      this.dataLineTypeName = this.datalistLineType.find(x => x.id == event)?.id;
      this.flagAdd = false;
    } else {
      this.flagAdd = true;
    }
  }

  clear() {
    this.snotifyService.success(MessageConstants.CLEAR, CaptionConstants.SUCCESS);
    this.searchBLCriticalParam = {
      lineNo: '',
      lineType: '',
      modelNo: '',
      stage: ''
    }
    this.criticalProcessData = [];
    this.dataLineNo = '';
    this.dataLineType = '';
    this.modelNo = '';
    this.changeLang = '1';
    this.flag = true;
    this.flagAdd = true;
    this.pagination.currentPage = 1;
    this.flag = true;
    this.flagAdd = true;
    this.criticalProcessServ.changeIDCritical({
      editFlag: '',
    });
  }

  pageChanged(event: any) {
    this.pagination.currentPage = event.page;
    this.search(1);
  }
  addNew() {
    this.criticalProcessServ.changeIDCritical({
      line_id: this.dataLineNo,
      line_name: this.dataLineNoName,
      line_type_id: this.dataLineType,
      line_type_name: this.dataLineTypeName,
      changeLang: this.changeLang,
      currentPage: this.pagination.currentPage,
      model: this.searchBLCriticalParam.modelNo,
      stageSearch: this.searchBLCriticalParam.stage,
      editFlag: '',
    })
    this.router.navigate(['/best-line/transaction/critical-process-analysis/add']);
  }

  changePageUpdate(item: BLCriticalProcess) {
    let changeLang = this.changeLang;

    this.criticalProcessServ.changeIDCritical({
      line_type_name: item.line_type_name,
      line_name: item.line_name,
      factory_id: item.factory_id,
      line_id: item.line_id,
      line_type_id: item.line_type_id,
      model_no: item.model_no,
      stage_id: item.stage_id,
      operation_id: item.operation_id,
      man_remarks: item.man_remarks,
      man_media_url: item.man_media_url,
      machine_remarks: item.machine_remarks,
      machine_media_url: item.machine_media_url,
      method_remarks: item.method_remarks,
      method_media_url: item.method_media_url,
      material_remarks: item.material_remarks,
      material_media_url: item.material_media_url,
      takt_time: item.takt_time,
      best_practice_url: item.best_practice_url,
      ct_before_sec: item.ct_before_sec,
      ct_after_sec: item.ct_after_sec,
      changeLang: changeLang,
      model: this.searchBLCriticalParam.modelNo,
      stageSearch: this.searchBLCriticalParam.stage,
      currentPage: this.pagination.currentPage,
      editFlag: '1'
    })
    this.router.navigateByUrl(`/best-line/transaction/critical-process-analysis/edit`);
  }

  ngAfterViewChecked(): void {
    this.cd.detectChanges();
  }
}


