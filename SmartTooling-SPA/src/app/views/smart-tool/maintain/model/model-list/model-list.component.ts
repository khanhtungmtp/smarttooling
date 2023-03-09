import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { Select2OptionData } from "ng-select2";
import { NgxSpinnerService } from "ngx-spinner";
import { environment } from "../../../../../../environments/environment";
import { CaptionConstants, MessageConstants } from "../../../../../_core/_constants/system.constants";
import { Model } from "../../../../../_core/_models/smart-tool/model";
import { Pagination, PaginatedResult } from "../../../../../_core/_models/smart-tool/pagination";
import { NgSnotifyService } from "../../../../../_core/_services/ng-snotify.service";
import { ModelService } from "../../../../../_core/_services/smart-tool/model.service";


@Component({
  selector: "app-model-list",
  templateUrl: "./model-list.component.html",
  styleUrls: ["./model-list.component.scss"],
})
export class ModelListComponent implements OnInit {
  downloadFileSample = `${environment.baseUrl}template/SmartTool/Sample_Model.xlsx`;
  models: Model[] = [];
  model: any = {};
  activeList: Array<Select2OptionData>;
  noData: boolean = false;
  accept: string = '.xls, .xlsm, .xlsx';
  fileImportExcel: File = null;
  paramSearch = {
    active: "all",
    model_search: ""
  };
  pagination: Pagination = {
    currentPage: 1,
    itemsPerPage: 10,
    totalItems: 1,
    totalPages: 1,
  };
  constructor(
    private modelService: ModelService,
    private snotifyAlert: NgSnotifyService,
    private route: ActivatedRoute,
    private router: Router,
    private spinner: NgxSpinnerService
  ) { }

  ngOnInit() {
    this.modelService.currentModelSearch.subscribe(res => {
      if (res !== null) {
        this.paramSearch = res;
      }
    });
    this.onFileUploadInit();
    this.loadData();
    
  }

  loadData() {
    this.spinner.show();
    this.noData = false;
    this.modelService.changeParamModelSearch(this.paramSearch);
    this.paramSearch.model_search = this.paramSearch.model_search.toUpperCase();
    this.modelService
      .search(
        this.pagination.currentPage,
        this.pagination.itemsPerPage,
        this.paramSearch
      )
      .subscribe(
        (res: PaginatedResult<Model[]>) => {
          console.log(res);
          
          this.models = res.result;
          this.pagination = res.pagination;
          if (this.models.length == 0) {
            this.noData = true;
          }
          this.spinner.hide();
        },
        (error) => {
          this.snotifyAlert.error("List Model failed loading data", CaptionConstants.ERROR);
        }
      );
  }

  search() {
    this.pagination.currentPage = 1;
    this.loadData();
  }

  clear() {
    this.paramSearch.active = "all";
    this.paramSearch.model_search = "";
    this.loadData();
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadData();
  }

  addModel() {
    this.router.navigate(["/model/add"]);
  }

  updateModel(modelNo) {
    this.router.navigate(["/model/edit/" + modelNo]);
  }

  exportExcel(): void {
    this.spinner.show();
    this.modelService
      .exportExcel(this.pagination, this.paramSearch)
      .subscribe({
        next: (result: Blob) => {
          if (result.size === 0) {
            this.snotifyAlert.error("No Data");
            this.spinner.hide();
            return;
          } else {
            if (result.type !== "xlsx") {
              this.spinner.hide();
            }

            const blob = new Blob([result]);
            const url = window.URL.createObjectURL(blob);
            const link = document.createElement("a");
            const currentTime = new Date();
            const fileName =
              "Model_Report_" +
              currentTime.getFullYear().toString() +
              (currentTime.getMonth() + 1) +
              currentTime.getDate() +
              ".xlsx";
            link.href = url;
            link.setAttribute("download", fileName);
            document.body.appendChild(link);
            link.click();
          }
        },
        error: () => { this.spinner.hide(); this.snotifyAlert.error("Error!") },
        complete: () => this.spinner.hide(),
      });
  }

  onSelectFile(event: any) {
    if (event.target.files && event.target.files[0]) {
      // check file name extension
      const fileNameExtension = event.target.files[0].name.split('.').pop();
      if (!this.accept.includes(fileNameExtension)) {
        return this.snotifyAlert.warning(MessageConstants.ALLOW_EXCEL_FILE, CaptionConstants.WARNING);
      }
      this.fileImportExcel = event.target.files[0];
    }
  }

  uploadExcel(): void {
    if (this.fileImportExcel == null) {
      return this.snotifyAlert.warning(MessageConstants.INVALID_FILE, CaptionConstants.WARNING);
    }
    this.spinner.show();
    this.modelService.uploadExcel(this.fileImportExcel).subscribe(res => {
      if (res.success) {
        this.snotifyAlert.success(MessageConstants.UPLOAD_OK_MSG, CaptionConstants.SUCCESS);
        this.loadData();
      }
      else if (res.caption === 'NotModel')
        this.snotifyAlert.error(`Model No "${res.data}" not found, please help to check Model No in the system`, CaptionConstants.ERROR);
      else
        this.snotifyAlert.error(MessageConstants.UPLOAD_ERROR_MSG, CaptionConstants.ERROR);

      this.spinner.hide();
    }, () => {
      this.spinner.hide();
      this.snotifyAlert.error(MessageConstants.UN_KNOWN_ERROR, CaptionConstants.ERROR);
    });
    this.onRemoveFile();
  }

  onFileUploadInit(): void {
    let inputLabel = document.getElementById('labelFile');
    document.getElementById('input_uploadFile').addEventListener('change', function (e: any) {
      if (e.target.files && e.target.files.length > 0) {
        inputLabel.innerHTML = e.target.files[0].name;
      } else {
        inputLabel.innerHTML = 'Choose file...';
      }
    })
  }

  onRemoveFile(): void {
    (<HTMLInputElement>document.getElementById("input_uploadFile")).value = null;
    document.getElementById('labelFile').innerHTML = 'Choose file...';
    this.fileImportExcel = null;
  }
}
