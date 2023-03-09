import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { Select2OptionData } from "ng-select2";
import { NgxSpinnerService } from "ngx-spinner";
import {
  CaptionConstants,
  MessageConstants,
} from "../../../../../_core/_constants/system.constants";
import { BLLayoutDesignOverall } from "../../../../../_core/_models/best-line/bl-layout-design-overall";
import { BLLayoutDesignOverallParam } from "../../../../../_core/_models/best-line/bl-layout-design-overall-param";
import { Pagination } from "../../../../../_core/_models/smart-tool/pagination";
import { LayoutDesignOverallService } from "../../../../../_core/_services/best-line/layout-design-overall.service";
import { NgSnotifyService } from "../../../../../_core/_services/ng-snotify.service";

@Component({
  selector: "app-layout-design-overall-main",
  templateUrl: "./layout-design-overall-main.component.html",
  styleUrls: ["./layout-design-overall-main.component.scss"],
})
export class LayoutDesignOverallMainComponent implements OnInit {
  data: BLLayoutDesignOverall[] = [];
  model: string = "";
  lineNo: string = "";
  lineType: string = "";
  modelName: string = "";
  lineNoList: Array<Select2OptionData> = [];
  lineTypeList: Array<Select2OptionData> = [];
  prodSeasonList: Select2OptionData[] = []
  optionsSelectLineNo = {
    placeholder: "Select Line No...",
    allowClear: true,
    width: "100%",
  };
  optionsSelectProdSeason = {
    placeholder: "Select Prod. Season...",
    allowClear: true,
    width: "100%",
  };
  optionsSelectLineType = {
    placeholder: "Select Line Type...",
    allowClear: true,
    width: "100%",
  };
  pagination: Pagination = {
    currentPage: 1,
    itemsPerPage: 10,
    totalItems: 0,
    totalPages: 0,
  };
  paramSearch: BLLayoutDesignOverallParam = {
    line_no: "",
    line_type: "",
    model: "",
    prodSeason: ""
  } as BLLayoutDesignOverallParam;

  checkNotifySearch: boolean = false;
  message = MessageConstants;
  constructor(
    private layoutDesignOverallService: LayoutDesignOverallService,
    private snotify: NgSnotifyService,
    private spinnerService: NgxSpinnerService,
    private router: Router
  ) { }

  ngOnInit() {
    this.spinnerService.show();
    this.getAllLineNo();
    this.getAllLineType();
    this.getAllProdSeason();
    this.layoutDesignOverallService.currentModelSearch.subscribe((res) => {
      if (res != null) {
        this.paramSearch = res;
        this.pagination.currentPage = res.currentPage;
        this.getData();
      } else {
        this.getData();
      }
    });
    this.spinnerService.hide();
  }
  getData() {
    this.spinnerService.show();
    this.paramSearch.model = this.paramSearch.model.toUpperCase();
    this.layoutDesignOverallService.search(this.pagination, this.paramSearch).subscribe((res) => {
      this.spinnerService.hide();
      this.pagination = res.pagination;
      this.data = res.result;
      if (this.checkNotifySearch) {
        this.spinnerService.hide();
        this.checkNotifySearch = false;
      }
    }, (error) => {
      this.spinnerService.hide();
      this.snotify.error(MessageConstants.SYSTEM_ERROR_MSG, CaptionConstants.ERROR);
    }
    );
  }
  search() {
    this.spinnerService.show();
    this.pagination.currentPage = 1;
    this.checkNotifySearch = true;
    this.getData();
    this.paramSearch.currentPage = this.pagination.currentPage;
    this.layoutDesignOverallService.setParamSearch(this.paramSearch);
    this.spinnerService.hide();
  }
  clear() {
    this.paramSearch = {
      line_no: "",
      line_type: "",
      model: "",
      prodSeason: ""
    } as BLLayoutDesignOverallParam;
    this.pagination = {
      currentPage: 1,
      itemsPerPage: 10,
    } as Pagination;
    this.getData();
    this.paramSearch.currentPage = this.pagination.currentPage;
    this.layoutDesignOverallService.setParamSearch(this.paramSearch);
  }
  add() {
    this.router.navigate(["/best-line/transaction/layout-design-overall/add"]);
  }
  update(item: BLLayoutDesignOverall) {
    this.layoutDesignOverallService.setEdit(item);
    this.router.navigate([
      "/best-line/transaction/layout-design-overall/edit/",
    ]);
  }
  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.getData();
    this.paramSearch.currentPage = this.pagination.currentPage;
    this.layoutDesignOverallService.setParamSearch(this.paramSearch);
  }
  getAllLineNo() {
    this.layoutDesignOverallService.getAllLineNo().subscribe((res) => {
      this.lineNoList = res.map((item) => {
        return {
          id: item.line_id,
          text: item.line_name,
        };
      });
    });
  }
  getAllLineType() {
    this.layoutDesignOverallService.getAllLineType().subscribe((res) => {
      this.lineTypeList = res.map((item) => {
        return {
          id: item.line_type_id,
          text: item.line_type_name,
        };
      });
    });
  }

  getAllProdSeason() {
    this.layoutDesignOverallService.getAllProdSeason().subscribe({
      next: (res) => {
        this.prodSeasonList = res.map((item) => {
          return {
            id: item.prod_season,
            text: item.prod_season,
          };
        });

        this.spinnerService.hide();
      },
      error: () => {
        this.spinnerService.hide();
        this.snotify.error('cannot get ProdSeason', CaptionConstants.ERROR)
      }
    })
  }
}
