import { Component, OnInit, OnChanges, SimpleChanges } from "@angular/core";
import { Router } from "@angular/router";
import { Select2OptionData } from "ng-select2";
import { MeasumentRFT } from "../../../../_core/_models/smart-tool/measumentRFT";
import { Pagination } from "../../../../_core/_models/smart-tool/pagination";
import { RftService } from "../../../../_core/_services/smart-tool/rft.service";


@Component({
  selector: "app-rft-list",
  templateUrl: "./rft-list.component.html",
  styleUrls: ["./rft-list.component.scss"],
})
export class RftListComponent implements OnInit, OnChanges {
  listdataModelNo: any;
  listModelNo: Array<Select2OptionData>;
  rftcondition: any = {};
  rft: any = {};

  modelNo: string = "none";
  modelName: string = "";
  upperID: string = "";
  stage: string = "none";
  conditionFlag: boolean = false;
  isAddNew: boolean = true;
  listStage: Array<Select2OptionData>;
  dataMesuar: MeasumentRFT[] = [];
  pagination: Pagination = {
    currentPage: 1,
    itemsPerPage: 10,
    totalItems: 1,
    totalPages: 1,
  };
  constructor(private _rftService: RftService, private router: Router) {}

  ngOnChanges(changes: SimpleChanges): void {
    // @Input()
    console.log("changee", changes);
  }

  ngOnInit() {
    this._rftService.paramSearchSource.asObservable().subscribe(res => {
      if(res !== null) {
        this.modelNo = res.modelNo;
        this.stage = res.stage;
      }
    });
    this.getListModelNo();
    this.getListStage();
  }
  getListModelNo() {
    this._rftService.geDataModelNo().subscribe((res) => {
      this.listdataModelNo = res;
      this.listModelNo = res.map((item) => {
        return { id: item.model_no, text: item.model_no };
      });
      // 10/12不需要ALL
      // this.listModelNo.unshift({ id: "All", text: "All" });
    });
  }

  getListStage() {
    this._rftService.geDataStage().subscribe((res) => {
      this.listStage = res.map((item) => {
        return { id: item.stage_id, text: item.stage_name };
      });
      // 10/12不需要ALL
      // this.listStage.unshift({ id: "All", text: "All" });
    });
  }

  getData(currentPage = 1) {
    let param = {
      modelNo: this.modelNo,
      stage: this.stage
    }
    this._rftService.paramSearchSource.next(param);
    this._rftService
      .search(currentPage, this.modelNo, this.stage)
      .subscribe((res) => {
        this.pagination = res.pagination;
        this.dataMesuar = res.result;
        // console.log(this.dataMesuar);
      });
  }

  changeModelNo(event: any) {
    this.modelName = "";
    this.upperID = "";
    if (event !== "none") {
      this.modelName = this.listdataModelNo.find(
        (x) => x.model_no === event
      ).model_name;
      this.upperID = this.listdataModelNo.find(
        (x) => x.model_no === event
      ).upper_id;
      this.conditionFlag = true;
    }
    // this.getData();
    this.addNewIsNull();
  }

  // 更換stage下拉選單後刷新data
  changeStage(event) {
    this.getData();
    this.addNewIsNull();
  }

  // 判斷是否可AddNew(Operation是否有null可新增)
  addNewIsNull() {

    this._rftService
      .getProcessType(this.modelNo, this.stage)
      .subscribe((res) => {
        // console.log(res);
        if (res.length === 0) {
          this.isAddNew = false;
        } else {
          this.isAddNew = true;
        }
      });
  }

  pageChanged(event) {
    this.pagination.currentPage = event.page;
    this.getData(this.pagination.currentPage);
  }

  // open add page
  addRFT() {
    this.rft = {};
    // 搜尋條件植入
    this.rftcondition = {
      modelNo: this.modelNo,
      modelName: this.modelName,
      upperID: this.upperID,
      stage: this.stage,
    };
    // console.log(this.rftcondition);
    this._rftService.getRFTCondition(this.rftcondition);
    this._rftService.changeRFT(this.rft);
    this.router.navigate(["./measurement/add"]);
  }

  // open edit page
  changeToEdit(RFT: MeasumentRFT) {
    // 搜尋條件植入
    this.rftcondition = {
      modelNo: this.modelNo,
      modelName: this.modelName,
      upperID: this.upperID,
      stage: this.stage,
    };
    // console.log(this.rftcondition);
    this._rftService.getRFTCondition(this.rftcondition);
    this._rftService.changeRFT(RFT);
    this.router.navigate(["./measurement/edit"]);
  }
}
