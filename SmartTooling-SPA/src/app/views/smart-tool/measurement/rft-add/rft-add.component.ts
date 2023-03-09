import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { Select2OptionData } from "ng-select2";
import { CaptionConstants, MessageConstants } from "../../../../_core/_constants/system.constants";
import { NgSnotifyService } from "../../../../_core/_services/ng-snotify.service";
import { RftService } from "../../../../_core/_services/smart-tool/rft.service";


@Component({
  selector: "app-rft-add",
  templateUrl: "./rft-add.component.html",
  styleUrls: ["./rft-add.component.scss"],
})
export class RftAddComponent implements OnInit {
  constructor(
    private _rftService: RftService,
    private snotifyAlert: NgSnotifyService,
    private router: Router
  ) {}
  rftcondition: any = {}; // 放條件
  rft: any = {}; // 資料容器
  // operation name condition
  modelno: string;
  stage: string;
  process: string = "";
  operation: string = "";
  rftpercent: number = 0;
  processTypeList: Array<Select2OptionData>;
  operationnameTypeList: Array<Select2OptionData>;
  defectreasonTypeList: Array<Select2OptionData>;

  ngOnInit() {
    // 搜尋條件傳入
    this._rftService.currentCondition.subscribe(
      (rftcondition) => (this.rftcondition = rftcondition)
    );
    // console.log(this.rftcondition);
    this._rftService.currentRFT.subscribe((rft) => (this.rft = rft));
    this.rft.total_produced_qty = 0;
    this.rft.defect_qty = 0;
    // 載入下拉選單
    this.getProcessType();
    this.getDefectReason();
  }

  backList() {
    this.router.navigate(["./measurement/list"]);
  }

  // 補值回rft陣列
  fillDataBeforeSave() {
    this.rft.model_no = this.rftcondition.modelNo;
    this.rft.operation_id = this.operation;
    this.rft.rft_percent = this.rftpercent;
    this.rft.stage_id = this.rftcondition.stage;
  }

  saveAndNext() {
    this.fillDataBeforeSave();
    this._rftService.createMeasurementRFT(this.rft).subscribe(
      () => {
        this.snotifyAlert.success(MessageConstants.CREATED_OK_MSG, CaptionConstants.SUCCESS);
        this.clearInput();
      },
      (error) => {
        this.snotifyAlert.error(error, CaptionConstants.ERROR);
      }
    );
  }

  save() {
    this.fillDataBeforeSave();
    this._rftService.createMeasurementRFT(this.rft).subscribe(
      () => {
        this.snotifyAlert.success(MessageConstants.CREATED_OK_MSG, CaptionConstants.SUCCESS);
        this.router.navigate(["./measurement/list"]);
      },
      (error) => {
        this.snotifyAlert.error(error, CaptionConstants.ERROR);
      }
    );
  }

  clearInput() {
    this.rft.total_produced_qty = 0;
    this.rft.defect_qty = 0;
    this.rftpercent = 0;
    this.rft.defect_pic = "";
    this.rft.inspector = "";
    this.rft.defect_reason_id = "";
    this.process = "";
    this.getListOpera();
  }

  getProcessType() {
    this._rftService
      .getProcessType(this.rftcondition.modelNo, this.rftcondition.stage)
      .subscribe((res) => {
        // console.log(res);
        this.processTypeList = res.map((item) => {
          return {
            id: item.process_type_id,
            text: item.process_type_name_local,
          };
        });
      });
  }

  getListOpera() {
    this._rftService
      .getOperationName(
        this.rftcondition.modelNo,
        this.rftcondition.stage,
        this.process
      )
      .subscribe((res) => {
        // console.log(res);
        this.operationnameTypeList = res.map((item) => {
          return { id: item.operation_id, text: item.operation_name_local };
        });
        // this.dataKaizen.operation_id = "";
      });
  }

  getDefectReason() {
    this._rftService.getDataDefectReason().subscribe((res) => {
      this.defectreasonTypeList = res.map((item) => {
        return { id: item.defect_reason_id, text: item.defect_reason_name };
      });
    });
  }

  countRFTPercent(Value: string, LorR: string): void {
    if (LorR === "L") {
      this.rft.total_produced_qty = Value;
    } else if (LorR === "R") {
      this.rft.defect_qty = Value;
    }
    this.rftpercent =
      ((this.rft.total_produced_qty - this.rft.defect_qty) /
        this.rft.total_produced_qty) *
      100;
    this.rftpercent = +this.rftpercent.toFixed(2);
  }
}
