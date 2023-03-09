import { Component, OnDestroy, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { Select2OptionData } from "ng-select2";
import { NgxSpinnerService } from "ngx-spinner";
import { environment } from "../../../../../../environments/environment";
import { CaptionConstants, MessageConstants } from "../../../../../_core/_constants/system.constants";
import { ModelOperationParam } from "../../../../../_core/_helpers/params/smart-tool/model-operation-param";
import { Pagination } from "../../../../../_core/_models/best-line/pagination-best-line";
import { ModelOperation } from "../../../../../_core/_models/smart-tool/model-operation";
import { ModelOperationEditParam } from "../../../../../_core/_models/smart-tool/model-operationEditParam";
import { NgSnotifyService } from "../../../../../_core/_services/ng-snotify.service";
import { ModelOperationService } from "../../../../../_core/_services/smart-tool/model-operation.service";
import bsCustomFileInput from 'bs-custom-file-input'
import { Model } from "../../../../../_core/_models/smart-tool/model";
import { Subscription } from "rxjs";

@Component({
  selector: 'app-model-operation-list',
  templateUrl: './model-operation-list.component.html',
  styleUrls: ['./model-operation-list.component.scss']
})
export class ModelOperationListComponent implements OnInit, OnDestroy {
  modelOperations: ModelOperation[];
  modelList: Array<Select2OptionData> = [];
  stageList: Array<Select2OptionData> = [];
  activeList: Array<Select2OptionData> = [];
  noData: boolean = false;
  isChecked: any = true;
  paramSearch: ModelOperationParam = <ModelOperationParam>{
    model_search: '',
    stage: ''
  };
  modelName: string = '';
  listdataModelNo: Model[] = [];
  pagination: Pagination = <Pagination>{
    currentPage: 1,
    pageSize: 10
  };
  accept: string = ".xls, .xlsm, .xlsx";
  downloadFileSample = `${environment.baseUrl}template/SmartTool/Sample_Model_Operation.xlsx`;
  fileImportExcel: File;
  subscription: Subscription;

  constructor(
    private modelOperationService: ModelOperationService,
    private snotifyService: NgSnotifyService,
    private router: Router,
    private spinnerService: NgxSpinnerService) { }

  ngOnInit() {
    bsCustomFileInput.init();
    this.subscription = this.modelOperationService.paramSearchSource.asObservable().subscribe(res => {
      if (res.model_search || res.stage) this.paramSearch = res;
    });
    this.getListModelNo();
    this.getStage();
    this.loadData();
    localStorage.removeItem("modelNo");
    localStorage.removeItem("modelOperationEditParam");
  }

  loadData() {
    this.spinnerService.show();
    if (!this.paramSearch.model_search && !this.paramSearch.stage) {
      this.noData = true;
      this.spinnerService.hide();
    } else {
      this.noData = false;
      this.modelOperationService.paramSearchSource.next(this.paramSearch);
      this.modelOperationService.search(this.paramSearch, this.pagination).subscribe({
        next: res => {
          this.spinnerService.hide();
          this.modelOperations = res.result;
          this.pagination = res.pagination;
          if (this.modelOperations.length == 0) {
            this.noData = true;
          }
        },
        error: () => this.snotifyService.error(MessageConstants.UN_KNOWN_ERROR, CaptionConstants.ERROR)
      })
    };
  }

  clear() {
    this.paramSearch.model_search = null;
    this.paramSearch.stage = null;
    this.noData = true;
    this.modelOperations = [];
  }

  addModelOperation() {
    let modelLocal = {
      modelNo: this.paramSearch.model_search,
      modelName: this.modelName
    }

    localStorage.setItem('modelLocal', JSON.stringify(modelLocal));
    this.router.navigate(['/model-operation/add']);
  }

  updateModelOperation(item: ModelOperation) {
    let modelOperationEditParam = new ModelOperationEditParam();
    modelOperationEditParam.factory_id = item.factory_id;
    modelOperationEditParam.model_no = item.model_no;
    modelOperationEditParam.operation_id = item.operation_id;
    modelOperationEditParam.stage_id = item.stage_id;
    modelOperationEditParam.model_name = this.modelName;
    localStorage.setItem("modelOperationEditParam", JSON.stringify(modelOperationEditParam));
    this.router.navigate(['/model-operation/edit']);
  }

  deleteOperation(item: ModelOperation) {
    this.snotifyService.confirm('Are you sure you want to delete this Model Operation ?', 'Delete Model Operation', () => {
      this.modelOperationService.deleteModelOperation(item).subscribe(() => {
        this.loadData();
        this.snotifyService.success('Model Operation has been deleted', CaptionConstants.SUCCESS);
      }, error => {
        this.snotifyService.error('This Model Operation is already in use', CaptionConstants.ERROR);
      });
    });
  }

  search() {
    this.spinnerService.show();
    this.pagination.currentPage = 1;
    this.loadData();
    this.spinnerService.hide();
  }

  getListModelNo() {
    this.modelOperationService.getModelNo().subscribe(res => {
      this.listdataModelNo = res;
      this.modelList = res.map(item => {
        return { id: item.model_no, text: item.model_no };
      });
    });
  }

  getStage() {
    this.modelOperationService.getStage().subscribe(res => {
      this.stageList = res.map(item => {
        return { id: item.stage_id, text: item.stage_name };
      });
    });
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadData();
  }

  changeModelNo() {
    if (this.paramSearch.model_search) {
      this.modelName = this.listdataModelNo.find(x => x.model_no === this.paramSearch.model_search).model_name;
    }
  }

  exportExcel() {
    this.spinnerService.show();
    if (!this.paramSearch.model_search && !this.paramSearch.stage) {
      this.noData = true;
      this.spinnerService.hide();
    } else {
      this.modelOperationService.exportExcel(this.paramSearch).subscribe({
        next: result => {
          if (result.size === 0) {
            this.snotifyService.error("No Data");
            this.spinnerService.hide();
            return;
          }

          const blob = new Blob([result]);
          const url = window.URL.createObjectURL(blob);
          const link = document.createElement('a');
          const currentTime = new Date();

          let filename = 'Model_Operation_Report_' + currentTime.getFullYear().toString() +
            (currentTime.getMonth() + 1) + currentTime.getDate() +
            currentTime.toLocaleTimeString().replace(/[ ]|[,]|[:]/g, '').trim() + '.xlsx';

          link.href = url;
          link.setAttribute('download', filename);
          document.body.appendChild(link);
          link.click();
          this.spinnerService.hide();
        }
      })
    }
  }

  uploadExcel() {
    if (this.fileImportExcel == null) {
      return this.snotifyService.warning(MessageConstants.INVALID_FILE, CaptionConstants.WARNING);
    }

    this.spinnerService.show();
    this.modelOperationService.uploadExcel(this.fileImportExcel).subscribe({
      next: res => {
        this.spinnerService.hide();
        if (res.success) {
          this.snotifyService.success(MessageConstants.UPLOAD_OK_MSG, CaptionConstants.SUCCESS);
          this.loadData();
        } else {
          this.snotifyService.error(MessageConstants[res.message], CaptionConstants.ERROR);
        }
      },
      error: () => {
        this.spinnerService.hide();
        this.snotifyService.error(MessageConstants.UN_KNOWN_ERROR, CaptionConstants.ERROR);
      }
    });
    this.onRemoveFile();
  }

  onSelectFile(event: any) {
    if (event.target.files && event.target.files[0]) {
      const fileNameExtension = event.target.files[0].name.split('.').pop();
      if (!this.accept.includes(fileNameExtension)) {
        return this.snotifyService.warning(MessageConstants.ALLOW_EXCEL_FILE, CaptionConstants.WARNING);
      }
      this.fileImportExcel = event.target.files[0];
    }
  }

  onRemoveFile() {
    (<HTMLInputElement>document.getElementById("input_uploadFile")).value = null;
    document.getElementById('labelFile').innerHTML = 'Choose file...';
    this.fileImportExcel = null;
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
