import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { NgxSpinnerService } from 'ngx-spinner';
import { environment } from '../../../../../../environments/environment';
import { CaptionConstants, MessageConstants } from '../../../../../_core/_constants/system.constants';
import { PadPrintSettingParam } from '../../../../../_core/_helpers/params/production-bp/pad-print-setting-param';
import { PBP_Pad_Print_Setting_DTO } from '../../../../../_core/_models/production-bp/pbp-pad-print-setting-dto';
import { Pagination } from '../../../../../_core/_models/smart-tool/pagination';
import { NgSnotifyService } from '../../../../../_core/_services/ng-snotify.service';
import { PadPrintSettingService } from '../../../../../_core/_services/production-bp/pad-print-setting.service';
@UntilDestroy()
@Component({
  selector: 'app-pad-print-setting-main',
  templateUrl: './pad-print-setting-main.component.html',
  styleUrls: ['./pad-print-setting-main.component.css']
})
export class PadPrintSettingMainComponent implements OnInit {
  downloadFileSample = `${environment.baseUrl}template/ProductionBP/Sample_PadPrintSetting.xlsx`;
  message = MessageConstants;
  isSuccess: boolean = false;
  pagination: Pagination = {
    currentPage: 1,
    itemsPerPage: 10,
    totalItems: 0,
    totalPages: 0,
  };
  searchParam: PadPrintSettingParam = {
    dev_season: '',
    production_season: '',
    model_no: ''
  } as PadPrintSettingParam;
  dataModel: PBP_Pad_Print_Setting_DTO[] = [];
  constructor(
    private snotify: NgSnotifyService,
    private router: Router,
    private spinnerService: NgxSpinnerService,
    private service: PadPrintSettingService
  ) { }
  accept: string = ".xls, .xlsm, .xlsx";
  fileImportExcel: File = null;

  ngOnInit() {
    this.service.currentSearchModel.subscribe(res => {
      if (res) {
        this.searchParam = res;
        if (res.currentPage) {
          this.pagination.currentPage = res.currentPage
        }
      }
    }).unsubscribe();
    this.getData();
    this.onFileUploadInit();
  }

  search() {
    this.pagination.currentPage = 1;
    this.getData();
    if (this.isSuccess)
      this.snotify.success(MessageConstants.QUERY_SUCCESS, CaptionConstants.SUCCESS);
  }

  getData() {
    this.spinnerService.show();
    this.searchParam.currentPage = this.pagination.currentPage;
    this.service.changeSearch(this.searchParam);
    this.service.search(this.searchParam, this.pagination).subscribe(resp => {
      this.dataModel = resp.result;
      this.pagination = resp.pagination;
      this.isSuccess = true;
      this.spinnerService.hide();
    }, err => {
      this.snotify.error(MessageConstants.SYSTEM_ERROR_MSG, CaptionConstants.ERROR);
      this.spinnerService.hide();
    });

  }

  pageChanged(event) {
    this.pagination.currentPage = event.page;
    this.getData();
  }

  clear() {
    this.searchParam = {
      dev_season: '',
      production_season: '',
      model_no: ''
    } as PadPrintSettingParam;
    this.pagination = {
      currentPage: 1,
      itemsPerPage: 10,
      totalItems: 0,
      totalPages: 0,
    };
    this.getData();
    this.snotify.success(MessageConstants.CLEAR, CaptionConstants.SUCCESS);
  }

  add() {
    this.router.navigate(['/production-bp/transaction/pad-print-setting/add']);
  }

  edit(model: PBP_Pad_Print_Setting_DTO) {
    this.service.changeModel(model);
    this.router.navigate(['/production-bp/transaction/pad-print-setting/edit']);
  }

  onSelectFile(event) {
    if (event.target.files && event.target.files[0]) {
      // check file name extension
      const fileNameExtension = event.target.files[0].name.split('.').pop();
      if (!this.accept.includes(fileNameExtension)) {
        return this.snotify.warning(MessageConstants.ALLOW_EXCEL_FILE, CaptionConstants.WARNING);
      }
      this.fileImportExcel = event.target.files[0];
    }
  }

  onFileUploadInit() {
    let inputLabel = document.getElementById('labelFile');
    document.getElementById('input_uploadFile').addEventListener('change', function (e: any) {
      if (e.target.files && e.target.files.length > 0) {
        inputLabel.innerHTML = e.target.files[0].name;
      } else {
        inputLabel.innerHTML = 'Choose a file...';
      }
    })
  }

  onRemoveFile() {
    (<HTMLInputElement>document.getElementById("input_uploadFile")).value = null;
    document.getElementById('labelFile').innerHTML = 'Choose a file...';
    this.fileImportExcel = null;
  }



  uploadExcel() {
    if (this.fileImportExcel == null) {
      return this.snotify.warning(MessageConstants.INVALID_FILE, CaptionConstants.WARNING);
    }

    this.spinnerService.show();
    this.service.uploadExcel(this.fileImportExcel).pipe(untilDestroyed(this)).subscribe(res => {
      if (res.success) {
        this.snotify.success(MessageConstants.UPLOAD_OK_MSG, CaptionConstants.SUCCESS);
        this.getData();
      } else if (res.caption === 'NotModel')
        this.snotify.error(`Model No "${res.data}" not found, please help to check Model No in the system`, CaptionConstants.ERROR);
      else
        this.snotify.error(MessageConstants.UPLOAD_ERROR_MSG, CaptionConstants.ERROR);

      this.spinnerService.hide();
    }, () => {
      this.spinnerService.hide();
      this.snotify.error(MessageConstants.UN_KNOWN_ERROR, CaptionConstants.ERROR);
    });
    this.onRemoveFile();
  }
}
