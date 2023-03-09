import { CaptionConstants, MessageConstants } from './../../../../../_core/_constants/system.constants';
import { catchError } from 'rxjs/operators';
import { PBPSearchComputerStitchingSetting } from './../../../../../_core/_helpers/params/production-bp/pbp-search-computer-stitching-setting';
import { PbpComputerStitchingSettingService } from './../../../../../_core/_services/production-bp/pbp-computer-stitching-setting.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { NgSnotifyService } from '../../../../../_core/_services/ng-snotify.service';
import { Pagination } from '../../../../../_core/_models/smart-tool/pagination';
import { ComputerStitchingSettingView } from '../../../../../_core/_models/production-bp/pbp-computer-stitching-setting-view';
import { of } from 'rxjs';
import { environment } from '../../../../../../environments/environment';

@Component({
  selector: 'app-computer-stitching-setting-main',
  templateUrl: './computer-stitching-setting-main.component.html',
  styleUrls: ['./computer-stitching-setting-main.component.scss']
})
export class ComputerStitchingSettingMainComponent implements OnInit {
  downloadFileSample = `${environment.baseUrl}template/ProductionBP/Sample_ComputerStitchingSetting.xlsx`;
  imageUrl: string = environment.imageUrl;
  noImage: string = "../../../../../../assets/img/no-image.jpg";
  random: number = Math.random();
  computers: ComputerStitchingSettingView[] = [];
  lang: string = 'en';
  formData: FormData = null;
  accept: string = '.xls, .xlsm, .xlsx';
  search: PBPSearchComputerStitchingSetting = {
    devSeason: '',
    productionSeason: '',
    model: '',
    productionAdoption: '',
  } as PBPSearchComputerStitchingSetting;
  pagination: Pagination = {
    currentPage: 1,
    itemsPerPage: 10
  } as Pagination;
  message = MessageConstants;
  constructor(
    private computerService: PbpComputerStitchingSettingService,
    private spinner: NgxSpinnerService,
    private snotify: NgSnotifyService,
    private router: Router,
  ) { }

  async ngOnInit() {
    // await this.loadData();
    this.computerService.currentParamSearch.subscribe(res => {
      if (res) {
        this.search = res;
        this.lang = res.lang;
        this.pagination.currentPage = res.currentPage;
        this.loadData();
      } else
        this.loadData();
    }).unsubscribe();
    this.onFileUploadInit();
  }

  async loadData() {
    this.spinner.show();
    let res = await this.computerService.getAllComputerStitchingSetting(this.search, this.pagination).pipe(
      catchError(error => {
        this.spinner.hide();
        this.snotify.error(MessageConstants.SYSTEM_ERROR_MSG, CaptionConstants.ERROR);
        console.log(error);
        return of(null);
      })
    ).toPromise();
    if (res) {
      this.computers = res.result;
      this.pagination = res.pagination;
    }

    this.search.currentPage = this.pagination.currentPage;
    this.search.lang = this.lang;
    await this.computerService.setParamSearch(this.search);
    this.spinner.hide();
  }

  async searchQuery() {
    this.pagination.currentPage = 1;
    await this.loadData();
    this.snotify.success(MessageConstants.QUERY_SUCCESS, CaptionConstants.SUCCESS);
  }

  async btnAdd() {
    this.router.navigate(['/production-bp/transaction/computer-stitching-setting/computer-stitching-setting-add']);
  }

  btnEdit(item: ComputerStitchingSettingView) {
    this.computerService.setModel(item);
    this.router.navigate(['/production-bp/transaction/computer-stitching-setting/computer-stitching-setting-edit']);
  }

  chooseFile(event: any) {
    if (event.target.files && event.target.files[0]) {
      // check file name extension
      const fileNameExtension = event.target.files[0].name.split('.').pop();
      if (!this.accept.includes(fileNameExtension))
        return this.snotify.warning(MessageConstants.ALLOW_EXCEL_FILE, CaptionConstants.WARNING);

      this.formData = new FormData();
      this.formData.append('file', event.target.files[0]);
    }
  }

  uploadExcel() {
    this.spinner.show();
    if (this.formData) {
      this.computerService.uploadExcel(this.formData).subscribe(res => {
        if (res.success) {
          this.snotify.success(MessageConstants.UPLOAD_OK_MSG, CaptionConstants.SUCCESS);
          this.loadData();
        }
        else if (res.caption === 'NotModel')
          this.snotify.error(`Model No "${res.data}" not found, please help to check Model No in the system`, CaptionConstants.ERROR);
        else if (res.caption === 'NotCS')
          this.snotify.error(`CS Operation (En) "${res.data} operation" not found, please help to check CS Operation (En) in the system`, CaptionConstants.ERROR);
        else
          this.snotify.error(MessageConstants.UPLOAD_ERROR_MSG, CaptionConstants.ERROR);

        this.formData = null;
        this.resetFile();
        this.spinner.hide();
      }, error => {
        this.spinner.hide();
        this.snotify.error(MessageConstants.UN_KNOWN_ERROR, CaptionConstants.ERROR);
      });
    }
    else {
      this.spinner.hide();
      this.snotify.warning(MessageConstants.INVALID_FILE, CaptionConstants.WARNING);
    }
  }

  onFileUploadInit() {
    let inputLabel = document.getElementById('labelFile');
    document.getElementById('inputFile').addEventListener('change', function (e: any) {
      if (e.target.files && e.target.files.length > 0) {
        inputLabel.innerHTML = e.target.files[0].name;
      } else {
        inputLabel.innerHTML = 'Choose file...';
      }
    })
  }
  resetFile() {
    (<HTMLFormElement>document.getElementById("form-input-file")).reset();
    let inputLabel = document.getElementById('labelFile');
    inputLabel.innerHTML = 'Choose file...';
  }

  async clear() {
    this.search = {
      devSeason: '',
      productionSeason: '',
      model: '',
      productionAdoption: '',
    } as PBPSearchComputerStitchingSetting;
    this.lang = 'en';
    this.pagination = {
      currentPage: 1,
      itemsPerPage: 10
    } as Pagination;
    await this.loadData();
    this.snotify.success(MessageConstants.CLEAR, CaptionConstants.SUCCESS);
  }

  async pageChanged(event: any) {
    this.pagination.currentPage = event.page;
    await this.loadData();
  }
}
