import { ChangeDetectorRef, Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { Select2OptionData } from "ng-select2";
import { NgxSpinnerService } from "ngx-spinner";
import {
  CaptionConstants,
  MessageConstants,
} from "../../../../../_core/_constants/system.constants";
import { BLLayoutDesignOverall } from "../../../../../_core/_models/best-line/bl-layout-design-overall";
import { BLLayoutDesignOverallParam } from "../../../../../_core/_helpers/params/best-line/bl-layout-design-overall-param";
import { Pagination } from "../../../../../_core/_models/smart-tool/pagination";
import { LayoutDesignOverallService } from "../../../../../_core/_services/best-line/layout-design-overall.service";
import { NgSnotifyService } from "../../../../../_core/_services/ng-snotify.service";
import { BLLayoutDesignOverallDetail } from "../../../../../_core/_models/best-line/bl-layout-design-overall-detail";

@Component({
  selector: "app-layout-design-overall-main",
  templateUrl: "./layout-design-overall-main.component.html",
  styleUrls: ["./layout-design-overall-main.component.scss"],
})
export class LayoutDesignOverallMainComponent implements OnInit {
  data: BLLayoutDesignOverall[] = [];
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
  pagination: Pagination = <Pagination>{
    currentPage: 1,
    itemsPerPage: 10,
  };
  paramSearch: BLLayoutDesignOverallParam = <BLLayoutDesignOverallParam>{
    line_no: "",
    line_type: "",
    model: "",
    prod_season: "",
  };

  message: typeof MessageConstants = MessageConstants;
  constructor(
    private layoutDesignOverallService: LayoutDesignOverallService,
    private snotify: NgSnotifyService,
    private spinnerService: NgxSpinnerService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) { }

  ngAfterViewChecked() {
    this.cdr.detectChanges();
  }

  ngOnInit() {
    this.spinnerService.show();
    this.getAllLineNo();
    this.getAllLineType();
    this.getAllProdSeason();
    this.getData();
    this.spinnerService.hide();
  }

  getData() {
    this.spinnerService.show();
    this.layoutDesignOverallService.search(this.pagination, this.paramSearch).subscribe({
      next: (res) => {
        this.spinnerService.hide();
        this.pagination = res.pagination;
        this.data = res.result;
        this.spinnerService.hide();
      },
      error: () => {
        this.spinnerService.hide();
        this.snotify.error(MessageConstants.SYSTEM_ERROR_MSG, CaptionConstants.ERROR);
      }
    })
  }

  search() {
    this.pagination.currentPage == 1 ? this.getData() : this.pagination.currentPage = 1
  }

  clear() {
    this.paramSearch = <BLLayoutDesignOverallParam>{
      line_no: "",
      line_type: "",
      model: "",
      prod_season: ""
    };
    this.getData();
    this.layoutDesignOverallService.setParamSearch(this.paramSearch);
  }

  add() {
    this.router.navigate(["/best-line/transaction/layout-design-overall/add"]);
  }

  update(item: BLLayoutDesignOverallDetail) {
    item.type = 'edit'
    this.layoutDesignOverallService.setEdit(item);
    this.router.navigate([
      "/best-line/transaction/layout-design-overall/edit/",
    ]);
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.getData();
  }

  getAllLineNo() {
    this.layoutDesignOverallService.getLineNoOfMain().subscribe({
      next: (res) => {
        this.lineNoList = res.map((item) => {
          return {
            id: item.line_id,
            text: item.line_name,
          };
        });
        this.spinnerService.hide();
      },
      error: () => {
        this.spinnerService.hide();
        this.snotify.error(MessageConstants.SYSTEM_ERROR_MSG, CaptionConstants.ERROR);
      }
    })
  }

  getAllLineType() {
    this.layoutDesignOverallService.getLineTypeOfMain().subscribe({
      next: (res) => {
        this.lineTypeList = res.map((item) => {
          return {
            id: item.line_type_id,
            text: item.line_type_name,
          };
        });
        this.spinnerService.hide();
      },
      error: () => {
        this.spinnerService.hide();
        this.snotify.error(MessageConstants.SYSTEM_ERROR_MSG, CaptionConstants.ERROR);
      }
    })
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
