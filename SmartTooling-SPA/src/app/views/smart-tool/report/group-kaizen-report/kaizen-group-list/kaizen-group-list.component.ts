import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { NgxSpinnerService } from "ngx-spinner";
import { CaptionConstants } from "../../../../../_core/_constants/system.constants";
import { Factory } from "../../../../../_core/_models/smart-tool/factory";
import { ModelKaizenReport } from "../../../../../_core/_models/smart-tool/model-kaizen-report";
import { Pagination } from "../../../../../_core/_models/smart-tool/pagination";
import { NgSnotifyService } from "../../../../../_core/_services/ng-snotify.service";
import { GroupKaizenReportService } from "../../../../../_core/_services/smart-tool/group-kaizen-report.service";
import { FunctionUtility } from "../../../../../_core/_utility/function-utility";


@Component({
  selector: "app-kaizen-group-list",
  templateUrl: "./kaizen-group-list.component.html",
  styleUrls: ["./kaizen-group-list.component.scss"],
})
export class KaizenGroupListComponent implements OnInit {
  filterParam: any;
  model_no: string = "";
  active = "all";
  factories: Factory[];
  factory: string;
  models: ModelKaizenReport[] = [];
  alerts: any = [
    {
      type: "success",
      msg: `You successfully read this important alert message.`,
    },
    {
      type: "info",
      msg: `This alert needs your attention, but it's not super important.`,
    },
    {
      type: "danger",
      msg: `Better check yourself, you're not looking too good.`,
    },
  ];
  pagination: Pagination = {
    currentPage: 1,
    itemsPerPage: 10,
    totalItems: 0,
    totalPages: 0,
  };
  constructor(
    private kaizenGroupSerivce: GroupKaizenReportService,
    private spinnerService: NgxSpinnerService,
    private snotifyAlert: NgSnotifyService,
    private router: Router,
    private utility: FunctionUtility
  ) {}

  ngOnInit() {
    this.kaizenGroupSerivce.paramSearchSource.asObservable().subscribe(res => {
      if(res !== null) {
        this.factory = res.factory_id;
        this.model_no = res.model_No;
        this.active = res.active;
        this.getData();
      }
    });
    this.getAllFactory();
  }
  getAllFactory() {
    this.kaizenGroupSerivce.getAllFactory().subscribe((res) => {
      this.factories = res;
    });
  }
  getData() {
    if (this.factory === undefined || this.factory === null || this.factory === '') {
      this.snotifyAlert.error("Please option factory", CaptionConstants.ERROR);
    } else {
      this.filterParam = {
        factory_id: this.factory,
        model_No: this.model_no.toUpperCase(),
        active: this.active,
      };
      this.spinnerService.show();
      this.kaizenGroupSerivce
        .search(
          this.pagination.currentPage,
          this.pagination.itemsPerPage,
          this.filterParam
        )
        .subscribe((res) => {
          this.spinnerService.hide();
          this.models = res.result;
          this.models.map((obj) => {
            obj.volume_string = this.utility.convertNumber(obj.volume);
            return obj;
          });
          this.pagination = res.pagination;
        });
    }
  }
  search() {
    this.pagination.currentPage = 1;
    this.getData();
  }
  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.getData();
  }
  modelDetail(model: ModelKaizenReport) {
    let filterParam = {
      factory_id: this.factory,
      model_No: this.model_no,
      active: this.active,
    };
    this.kaizenGroupSerivce.paramSearchSource.next(filterParam);
    this.kaizenGroupSerivce.changeModel(model);
    this.router.navigate(['/report/group-kaizen-report/model-detail']);
  }
  clear() {
    this.models.length = 0;
    this.model_no = '';
    this.active = 'all';
    this.factory = '';
  }
  exportExcel() {
    if(this.factory === undefined || this.factory === null) {
      this.snotifyAlert.error('Please option factory!', CaptionConstants.ERROR);
    } else {
      this.filterParam = {
        factory_id: this.factory,
        model_No: this.model_no.toUpperCase(),
        active: this.active,
      };
      this.kaizenGroupSerivce.exportExcel(this.filterParam);
    }
  }
}
