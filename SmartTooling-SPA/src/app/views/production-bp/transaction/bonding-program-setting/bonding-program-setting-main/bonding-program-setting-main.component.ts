import { HttpParams } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Select2OptionData } from 'ng-select2';
import { NgxSpinnerService } from 'ngx-spinner';
import { PBPBondingProgramSetting } from '../../../../../_core/_models/production-bp/pbp-bonding-program-setting';
import { PBPBondingProgramSettingPram } from '../../../../../_core/_models/production-bp/pbp-bonding-program-setting-pram';
import { Pagination } from '../../../../../_core/_models/smart-tool/pagination';
import { NgSnotifyService } from '../../../../../_core/_services/ng-snotify.service';
import { BondingProgramSettingService } from '../../../../../_core/_services/production-bp/bonding-program-setting.service';
import {
  CaptionConstants,
  MessageConstants,
} from "../../../../../_core/_constants/system.constants";
import { environment } from '../../../../../../environments/environment';

@Component({
  selector: 'app-bonding-program-setting-main',
  templateUrl: './bonding-program-setting-main.component.html',
  styleUrls: ['./bonding-program-setting-main.component.css']
})
export class BondingProgramSettingMainComponent implements OnInit {
  downloadFileSample = `${environment.baseUrl}template/ProductionBP/Sample_BondingProgramSetting.xlsx`;
  data: PBPBondingProgramSetting[] = [];
  autoTechList: Array<Select2OptionData> = [];
  chemicalProcessTypeList: Array<Select2OptionData> = [];
  fileImportExcel: File = null;
  fileName: string = "";
  accept: string = '.xls, .xlsm, .xlsx';
  optionsSelectAutoTech = {
    placeholder: "Select Auto Tech...",
    allowClear: true,
    width: "100%",
  };

  optionsSelectChemicalProcessType = {
    placeholder: "Select Chemical Process Type...",
    allowClear: true,
    width: "100%",
  };

  pagination: Pagination = {
    currentPage: 1,
    itemsPerPage: 10,
    totalItems: 0,
    totalPages: 0,
  };

  checkNotifySearch: boolean = false;

  paramSearch: PBPBondingProgramSettingPram = {
    production_season: "",
    model: "",
    chemical_process_type_id: "",
    auto_tech_id: ""
  } as PBPBondingProgramSettingPram;
  message = MessageConstants;
  constructor(
    private bondingProgramSettingService: BondingProgramSettingService,
    private snotify: NgSnotifyService,
    private spinnerService: NgxSpinnerService,
    private router: Router,
  ) { }

  ngOnInit(): void {
    this.spinnerService.show();
    this.getAllAutoTech();
    this.getAllLineType();
    this.bondingProgramSettingService.currentModelSearch.subscribe((res) => {
      if (res != null) {
        this.paramSearch = res;
      }
    });
    this.getData();
    this.onFileUploadInit();
    this.spinnerService.hide();
  }

  getAllAutoTech() {
    this.bondingProgramSettingService.getAllAutoTech().subscribe((res) => {
      this.autoTechList = res.map((item) => {
        return {
          id: item.key,
          text: item.value,
        };
      });
    });
  }

  getAllLineType() {
    this.bondingProgramSettingService.getAllChemicalProcessType().subscribe((res) => {
      this.chemicalProcessTypeList = res.map((item) => {
        return {
          id: item.key,
          text: item.value,
        };
      });
    });
  }

  getData() {
    console.log("Get Data");
    this.spinnerService.show();
    this.paramSearch.model = this.paramSearch.model.toUpperCase();
    this.bondingProgramSettingService
      .search(this.pagination, this.paramSearch)
      .subscribe(
        (res) => {
          this.spinnerService.hide();
          this.pagination = res.pagination;
          this.data = res.result;
          if (this.checkNotifySearch) {
            this.spinnerService.hide();
            this.snotify.success(
              MessageConstants.QUERY_SUCCESS,
              CaptionConstants.SUCCESS
            );
          }
        },
        (error) => {
          this.spinnerService.hide();
          this.snotify.error(
            MessageConstants.SYSTEM_ERROR_MSG,
            CaptionConstants.ERROR
          );
        }
      );
  }

  search() {
    this.spinnerService.show();
    this.pagination.currentPage = 1;
    this.getData();
    this.checkNotifySearch = true;
    this.bondingProgramSettingService.setParamSearch(this.paramSearch);
    this.spinnerService.hide();
  }

  clear() {
    this.paramSearch = {
      production_season: "",
      model: "",
      chemical_process_type_id: "",
      auto_tech_id: "",
    } as PBPBondingProgramSettingPram;
    this.onRemoveFile();
    this.bondingProgramSettingService.setParamSearch(this.paramSearch);
    this.getData();
    return;
  }

  add() {
    this.router.navigate(["/production-bp/transaction/bonding-program-setting/add"]);
  }

  update(item: PBPBondingProgramSetting) {
    this.bondingProgramSettingService.setEdit(item);
    this.router.navigate([
      "/production-bp/transaction/bonding-program-setting/edit/",
    ]);
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.getData();
  }

  onSelectFile(event: any) {
    if (event.target.files && event.target.files[0]) {
      // check file name extension
      const fileNameExtension = event.target.files[0].name.split('.').pop();
      if (!this.accept.includes(fileNameExtension)) {
        return this.snotify.warning(MessageConstants.ALLOW_EXCEL_FILE, CaptionConstants.WARNING);
      }
      this.fileImportExcel = event.target.files[0];
    }
  }

  uploadExcel() {
    if (this.fileImportExcel == null) {
      return this.snotify.warning(MessageConstants.INVALID_FILE, CaptionConstants.WARNING);
    }
    this.spinnerService.show();
    this.bondingProgramSettingService.uploadExcel(this.fileImportExcel).subscribe(res => {
      if (res.success) {
        this.snotify.success(MessageConstants.UPLOAD_OK_MSG, CaptionConstants.SUCCESS);
        this.getData();
      }
      else if (res.caption === 'NotModel')
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

  onFileUploadInit() {
    let inputLabel = document.getElementById('labelFile');
    document.getElementById('input_uploadFile').addEventListener('change', function (e: any) {
      if (e.target.files && e.target.files.length > 0) {
        inputLabel.innerHTML = e.target.files[0].name;
      } else {
        inputLabel.innerHTML = 'Choose file...';
      }
    })
  }

  onRemoveFile() {
    (<HTMLInputElement>document.getElementById("input_uploadFile")).value = null;
    document.getElementById('labelFile').innerHTML = 'Choose file...';
    this.fileImportExcel = null;
  }
}
