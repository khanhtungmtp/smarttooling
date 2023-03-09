import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Select2OptionData } from 'ng-select2';
import { NgxSpinnerService } from 'ngx-spinner';
import { environment } from '../../../../../../environments/environment';
import { CaptionConstants, MessageConstants } from '../../../../../_core/_constants/system.constants';
import { BL_Layout_Design_Process_Params } from '../../../../../_core/_models/best-line/bL_Layout_Design_Process_Params';
import { Pagination } from '../../../../../_core/_models/smart-tool/pagination';
import { C2blayoutByProcessService } from '../../../../../_core/_services/best-line/c2blayout-by-process.service';
import { NgSnotifyService } from '../../../../../_core/_services/ng-snotify.service';
import { FunctionUtility } from '../../../../../_core/_utility/function-utility';

@Component({
  selector: 'app-process-main',
  templateUrl: './process-main.component.html',
  styleUrls: ['./process-main.component.scss']
})
export class ProcessMainComponent implements OnInit {
  arrLineID: Array<Select2OptionData> = [];
  arrLineTypeID: Array<Select2OptionData> = [];
  arrModelNo: Array<Select2OptionData> = [];
  arrModelNoAndName: Array<Select2OptionData> = [];
  pagination: Pagination = {
    currentPage: 1,
    itemsPerPage: 10,
    totalItems: 1,
    totalPages: 1
  };
  lineID: string = '';
  lineIDTemp: string = '';
  lineName: string = '';
  lineTypeName: string = '';
  lineTypeID: string = '';
  modelName: string = '';
  modelNo: string = '';
  dataProcess: BL_Layout_Design_Process_Params;
  data: BL_Layout_Design_Process_Params[] = [];
  message = MessageConstants;
  noImage: string = '../../../../assets/img/no-image.jpg';
  urlImage = environment.imageUrl;
  random: number = new Date().getTime();
  constructor(private _processService: C2blayoutByProcessService,
    private router: Router,
    private spinnerService: NgxSpinnerService,
    private cd: ChangeDetectorRef,
    private snotifyalert: NgSnotifyService,
    private utility: FunctionUtility) { }

  ngOnInit() {
    this._processService.currentProcessEdit.subscribe(res => {
      if (res != null) {
        this.lineID = this.lineIDTemp = res.lineID;
        this.lineTypeID = res.lineTypeID;
        this.modelNo = res.modelNo;
        this.modelName = res.modelName;
        this.search(1);
      }
      else {
        [this.lineID, this.lineTypeID, this.modelNo, this.modelName] = ['', '', '', ''];
      }
    }).unsubscribe();
    this.getLineID();
  }

  ngAfterViewChecked(): void {
    this.cd.detectChanges();
  }

  pageChange(event) {
    this.pagination.currentPage = event.page;
    this.search(1);
  }

  getLineID() {
    this._processService.getLineID().subscribe(res => {
      this.arrLineID = res.map(item => {
        return { id: item.id, text: item.name };
      })
    });
  }

  lineIDChange(event) {
    if (event != '') {
      this.lineID = this.arrLineID.find(x => x.id == event)?.id;
      this.lineName = this.arrLineID.find(x => x.id == event)?.text;
      if (this.lineID != this.lineIDTemp) {
        [this.lineTypeID, this.modelNo, this.modelName] = ['', '', ''];
        this.lineIDTemp = this.lineID;
      }
      this.getLineTypeID();
    }
    else {
      [this.lineTypeID, this.modelNo, this.modelName] = ['', '', ''];
    }

  }

  getLineTypeID() {
    this._processService.getLineTypeID(this.lineID).subscribe(res => {
      this.arrLineTypeID = res.map(item => {
        return { id: item.lineTypeID, text: item.lineTypeName };
      })
    });
  }

  lineTypeIDChange(event) {
    if (event != '') {
      this.lineTypeID = this.arrLineTypeID.find(x => x.id == event)?.id;
      this.lineTypeName = this.arrLineTypeID.find(x => x.id == event)?.text;
      this.arrModelNo = []
      this.getModelNo();
    }
    else {
      [this.modelNo, this.modelName] = ['', ''];
    }
  }

  getModelNo() {
    this._processService.getmodelNo(this.lineID, this.lineTypeID).subscribe(res => {
      this.arrModelNoAndName = res.map(item => {
        return { id: item.id, text: item.text }
      })
      this.arrModelNo = res.map(item => {
        return { id: item.id, text: item.id }
      })
    });
  }

  modelNoChange(event) {
    if (event != '') {
      this.modelNo = this.arrModelNoAndName?.find(x => x.id == event)?.id;
      this.modelName = this.arrModelNoAndName?.find(x => x.id == event)?.text;
    }
    else {
      this.modelName = '';
    }
  }

  search(check: number) {
    if (check === 0) {
      this.pagination.currentPage = 1;
    }
    if (this.utility.isEmpty(this.lineID) || this.utility.isEmpty(this.lineTypeID) || this.utility.isEmpty(this.modelNo)){
      this.snotifyalert.warning(MessageConstants.SELECT_ALL_QUERY_OPTION, CaptionConstants.WARNING);
    }
    else {
      this.spinnerService.show();
      const query = {
        lineID: this.lineID,
        lineTypeID: this.lineTypeID,
        modelNo: this.modelNo
      }
      this._processService.search(this.pagination.currentPage, this.pagination.itemsPerPage, query).subscribe(
        res => {
          this.data = res.result;
          this.pagination = res.pagination;
          this.spinnerService.hide();
          if (check === 0) {
            this.snotifyalert.success(MessageConstants.QUERY_SUCCESS, CaptionConstants.SUCCESS);
          }
        }
      )
    }
  }

  clear() {
    [this.lineID, this.lineTypeID, this.modelNo, this.modelName] = ['', '', '', ''];
    this.data.length = 0;
    this.pagination.totalPages = 0;
    this.snotifyalert.success(MessageConstants.CLEAR, CaptionConstants.SUCCESS);
  }

  addNew() {
    this._processService.changeIDProcess({
      lineID: this.lineID,
      lineName: this.lineName,
      lineTypeID: this.lineTypeID,
      lineTypeName: this.lineTypeName,
      modelNo: this.modelNo,
      modelName: this.modelName
    });
    this.router.navigate(['/best-line/transaction/c2b-layout-by-process/add']);
  }

  edit(item: BL_Layout_Design_Process_Params) {
    this._processService.changeProcessEdit({
      lineID: item.line_id,
      lineName: item.line_name,
      lineTypeID: item.line_type_id,
      lineTypeName: item.line_type_name,
      modelNo: item.model_no,
      modelName: item.model_name,
      processTypeName: item.process_type_name_en,
      processType: item.process_type_id,
      eachBefore: item.each_process_image_before,
      eachAfter: item.each_process_image_after,
      remarks: item.remarks
    })
    this.router.navigate(['/best-line/transaction/c2b-layout-by-process/edit']);
  }
}
