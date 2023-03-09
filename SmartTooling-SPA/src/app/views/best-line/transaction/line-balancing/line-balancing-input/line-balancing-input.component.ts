import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { Select2OptionData } from 'ng-select2';
import { takeUntil } from 'rxjs/operators';
import { CaptionConstants, MessageConstants } from '../../../../../_core/_constants/system.constants';
import { LineBalancingService } from '../../../../../_core/_services/best-line/line-balancing.service';
import { DestroyService } from '../../../../../_core/_services/destroy.service';
import { NgSnotifyService } from '../../../../../_core/_services/ng-snotify.service';

@UntilDestroy()
@Component({
  selector: 'app-line-balancing-input',
  templateUrl: './line-balancing-input.component.html',
  styleUrls: ['./line-balancing-input.component.scss'],
  providers: [DestroyService]
})
export class LineBalancingInputComponent implements OnInit {
  lineId: string = '';
  lineTypeId: string = '';
  lineTypeName: string = '';
  modelNo: string = '';
  modelName: string = '';
  processType: string = '';
  processTypeName: string = '';
  lineName: string = '';
  fileImportExcel: File = null;
  dataLine: Array<Select2OptionData> = [];

  constructor(
    private _lineBalancingService: LineBalancingService,
    private router: Router,
    private snotifyalert: NgSnotifyService,
    private destroyService: DestroyService
  ) { }

  ngOnInit() {
    this._lineBalancingService.currentLineBalancing
      .pipe(untilDestroyed(this))
      .subscribe(res => {
        if (!res) {
          return this.backList();
        }
        this.lineId = res.lineID,
          this.lineName = res.lineName,
          this.lineTypeId = res.lineTypeId,
          this.lineTypeName = res.lineTypeName,
          this.modelNo = res.modelNo,
          this.modelName = res.modelName,
          this.processType = res.processType,
          this.processTypeName = res.processTypeName
      });

    this.dataLine = [
      {
        id: this.lineId,
        text: this.lineName,
      },
      {
        id: this.lineTypeId,
        text: this.lineTypeName,
      },
      {
        id: this.modelNo,
        text: this.modelNo,
      },
      {
        id: this.processType,
        text: this.processTypeName,
      }
    ];
  }

  backList() {
    this.router.navigate(['/best-line/transaction/line-balancing']);
  }

  save() {
    this._lineBalancingService.importExcel(this.lineId,
      this.lineTypeId, this.modelNo, this.processType,
      this.modelName, this.fileImportExcel)
      .pipe(takeUntil(this.destroyService.destroys$))
      .subscribe((res) => {
        if (res) {
          this.snotifyalert.success(MessageConstants.CREATED_OK_MSG, CaptionConstants.SUCCESS);
          this.backList();
        } else {
          this.snotifyalert.error(MessageConstants.CREATED_ERROR_MSG, CaptionConstants.ERROR);
        }
      }, error => {
        this.snotifyalert.error(MessageConstants.UPLOAD_ERROR_MSG, CaptionConstants.ERROR);
      });
  }

  onSelectFile(event) {
    if (event.target.files && event.target.files[0]) {
      const reader = new FileReader();

      reader.readAsDataURL(event.target.files[0]); // read file as data url
      const file = event.target.files[0];
      // check file name extension
      const fileNameExtension = event.target.files[0].name.split('.').pop();
      if (fileNameExtension != 'xlsx' && fileNameExtension != 'xlsm') {
        this.snotifyalert.warning('Warning', "Please select a file '.xlsx' or '.xls'");
        return;
      }

      this.fileImportExcel = file;
    }
  }
}
