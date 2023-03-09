import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { Select2OptionData } from 'ng-select2';
import { NgxSpinnerService } from 'ngx-spinner';
import { CaptionConstants, MessageConstants } from '../../../../../_core/_constants/system.constants';
import { BL_Layout_Design_Process } from '../../../../../_core/_models/best-line/bL_Layout_Design_Process';
import { C2blayoutByProcessService } from '../../../../../_core/_services/best-line/c2blayout-by-process.service';
import { NgSnotifyService } from '../../../../../_core/_services/ng-snotify.service';
import { MediaUploadComponent } from '../../../../commons/media-upload/media-upload.component';


@UntilDestroy()
@Component({
  selector: 'app-process-add',
  templateUrl: './process-add.component.html',
  styleUrls: ['./process-add.component.scss']
})
export class ProcessAddComponent implements OnInit {
  @ViewChild('processBefore') processBefore: MediaUploadComponent;
  @ViewChild('processAfter') processAfter: MediaUploadComponent;
  lineId: string = '';
  lineTypeId: string = '';
  modelNo: string = '';
  modelName: string = '';
  lineName: string = '';
  arrProcessType: Array<Select2OptionData>;
  dataLine: Array<Select2OptionData> = [];
  remarks: string = '';
  data: BL_Layout_Design_Process = {
    each_process_image_after: '',
    each_process_image_before: ''
  } as BL_Layout_Design_Process;
  constructor(
    private _processService: C2blayoutByProcessService,
    private router: Router,
    private spinnerService: NgxSpinnerService,
    private snotifyalert: NgSnotifyService,
    private cd: ChangeDetectorRef) { }

  ngOnInit() {
    this._processService.currentProcess
      .pipe(untilDestroyed(this))
      .subscribe(res => {
        if (!res) {
          return this.backList();
        }
        this.data.line_id = res.lineID;
        this.data.line_type_id = res.lineTypeID;
        this.data.model_no = res.modelNo;

        this.lineName = res.lineName;
        this.lineId = res.lineID;
        this.modelNo = res.modelNo;
        this.modelName = res.modelName;
        this.lineTypeId = res.lineTypeID;
      });
    this.dataLine = [
      {
        id: this.lineId,
        text: this.lineName,
      },
      {
        id: this.lineTypeId,
        text: this.lineTypeId,
      },
      {
        id: this.modelNo,
        text: this.modelNo,
      }
    ];
    this.getProcessType();
  }

  ngAfterViewChecked(): void {
    this.cd.detectChanges();
  }

  backList() {
    this._processService.changeProcessEdit({
      lineID: this.data.line_id,
      lineTypeID: this.data.line_type_id,
      modelNo: this.data.model_no,
      modelName: this.modelName
    });
    this.router.navigate(['/best-line/transaction/c2b-layout-by-process']);
  }

  getProcessType() {
    this._processService.getProcessType().subscribe(res => {
      this.arrProcessType = res.map(item => {
        return { id: item.id, text: item.text };
      })
    })
  }

  saveAndNext() {
    this.spinnerService.show();
    this.setData();
    this._processService.add(this.data).subscribe(res => {
      if (res.success) {
        this.snotifyalert.success(MessageConstants.CREATED_OK_MSG, CaptionConstants.SUCCESS);
        this.spinnerService.hide();
        this.clear();
      }
      else {
        this.snotifyalert.error(MessageConstants.CREATED_ERROR_MSG, CaptionConstants.ERROR);
        this.spinnerService.hide();
      }
    })
  }

  clear() {
    this.data.line_id = this.lineId;
    this.data.line_type_id = this.lineTypeId;
    this.data.model_no = this.modelNo;
    this.data.process_type_id = '';
    this.data.remarks = '';
    this.data.each_process_image_before = '';
    this.data.each_process_image_after = '';
  }

  save() {
    this.spinnerService.show();
    this.setData();
    this._processService.add(this.data).subscribe(res => {
      if (res.success) {
        this.snotifyalert.success(MessageConstants.CREATED_OK_MSG, CaptionConstants.SUCCESS);
        this.spinnerService.hide();
        this.backList();
      }
      else {
        this.snotifyalert.error(MessageConstants.CREATED_ERROR_MSG, CaptionConstants.ERROR);
        this.spinnerService.hide();
      }
    })
  }

  setData() {
    this.data.line_id = this.lineId;
    this.data.line_type_id = this.lineTypeId;
    this.data.model_no = this.modelNo;
    this.data.each_process_image_before = this.processBefore.url == this.processBefore.noImage ? '' : this.processBefore.url;
    this.data.each_process_image_after = this.processAfter.url == this.processAfter.noImage ? '' : this.processAfter.url;
  }
}
