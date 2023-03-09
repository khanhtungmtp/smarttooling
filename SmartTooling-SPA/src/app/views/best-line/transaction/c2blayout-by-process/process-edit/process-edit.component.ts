import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { Select2OptionData } from 'ng-select2';
import { NgxSpinnerService } from 'ngx-spinner';
import { environment } from '../../../../../../environments/environment';
import { CaptionConstants, MessageConstants } from '../../../../../_core/_constants/system.constants';
import { BL_Layout_Design_Process } from '../../../../../_core/_models/best-line/bL_Layout_Design_Process';
import { BL_Layout_Design_Process_Params } from '../../../../../_core/_models/best-line/bL_Layout_Design_Process_Params';
import { C2blayoutByProcessService } from '../../../../../_core/_services/best-line/c2blayout-by-process.service';
import { NgSnotifyService } from '../../../../../_core/_services/ng-snotify.service';
import { MediaUploadComponent } from '../../../../commons/media-upload/media-upload.component';

@UntilDestroy()
@Component({
  selector: 'app-process-edit',
  templateUrl: './process-edit.component.html',
  styleUrls: ['./process-edit.component.scss']
})
export class ProcessEditComponent implements OnInit {
  @ViewChild('processBefore') processBefore: MediaUploadComponent;
  @ViewChild('processAfter') processAfter: MediaUploadComponent;
  modelName: string = '';
  noImage: string = '../../../../assets/img/no-image.jpg';
  data: BL_Layout_Design_Process = {} as BL_Layout_Design_Process;
  dataProcess: BL_Layout_Design_Process_Params = {
    each_process_image_after: '',
    each_process_image_before: ''
  } as BL_Layout_Design_Process_Params;
  urlImage = environment.imageUrl;
  dataLine: Array<Select2OptionData> = [];
  random: number = Math.random();

  constructor(private _processService: C2blayoutByProcessService,
    private router: Router,
    private spinnerService: NgxSpinnerService,
    private snotifyalert: NgSnotifyService,
    private cd: ChangeDetectorRef) { }

  ngOnInit() {

    this._processService.currentProcessEdit
      .pipe(untilDestroyed(this))
      .subscribe(res => {
        if (!res) {
          return this.backList();
        }
        this.dataProcess.line_id = res.lineID;
        this.dataProcess.line_name = res.lineName;
        this.dataProcess.line_type_id = res.lineTypeID;
        this.dataProcess.line_type_name = res.lineTypeName;
        this.dataProcess.model_no = res.modelNo;
        this.dataProcess.model_name = res.modelName;
        this.dataProcess.each_process_image_after = res.eachAfter;
        this.dataProcess.each_process_image_before = res.eachBefore;
        this.dataProcess.remarks = res.remarks;
        this.dataProcess.process_type_id = res.processType;
        this.dataProcess.process_type_name_en = res.processTypeName;
        this.modelName = res.modelName;
      });

    this.dataLine = [
      {
        id: this.dataProcess.line_id,
        text: this.dataProcess.line_name,
      },
      {
        id: this.dataProcess.line_type_id,
        text: this.dataProcess.line_type_name,
      },
      {
        id: this.dataProcess.model_no,
        text: this.dataProcess.model_no,
      },
      {
        id: this.dataProcess.process_type_id,
        text: this.dataProcess.process_type_name_en,
      },
    ];
    this.loadData();
  }

  ngAfterViewChecked(): void {
    this.cd.detectChanges();
  }

  backList() {
    this._processService.changeProcessEdit({
      lineID: this.dataProcess.line_id,
      lineTypeID: this.dataProcess.line_type_id,
      modelNo: this.dataProcess.model_no,
      modelName: this.modelName
    });
    this.router.navigate(['/best-line/transaction/c2b-layout-by-process']);
  }

  loadData() {
    this._processService.getDataEdit(this.dataProcess.line_id, this.dataProcess.line_type_id, this.dataProcess.model_no, this.dataProcess.process_type_id)
                        .subscribe(res => {
                          this.data = res;
                        });
                      }

  save() {
    this.spinnerService.show();
    this.data.line_id = this.dataProcess.line_id;
    this.data.line_type_id = this.dataProcess.line_type_id;
    this.data.model_no = this.dataProcess.model_no;
    this.data.process_type_id = this.dataProcess.process_type_id;
    this.data.remarks = this.dataProcess.remarks;
    if (this.processBefore.url.includes("base64"))
      this.data.each_process_image_before = this.processBefore.url;
    else
      this.data.each_process_image_before = '';
    if (this.processAfter.url.includes("base64"))
      this.data.each_process_image_after = this.processAfter.url;
    else
      this.data.each_process_image_after = '';
    this._processService.edit(this.data).subscribe(res => {
      if (res.success) {
        this.snotifyalert.success(MessageConstants.UPDATED_OK_MSG, CaptionConstants.SUCCESS);
        this.spinnerService.hide();
        this.backList();
      }
      else {
        this.snotifyalert.error(MessageConstants.UPDATED_ERROR_MSG, CaptionConstants.ERROR);
        this.spinnerService.hide();
      }
    })
  }
}
