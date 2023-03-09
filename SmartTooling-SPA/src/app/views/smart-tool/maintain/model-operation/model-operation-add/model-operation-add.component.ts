import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { Select2OptionData } from "ng-select2";
import { CaptionConstants, MessageConstants } from "../../../../../_core/_constants/system.constants";
import { ModelOperation } from "../../../../../_core/_models/smart-tool/model-operation";
import { NgSnotifyService } from "../../../../../_core/_services/ng-snotify.service";
import { ModelOperationService } from "../../../../../_core/_services/smart-tool/model-operation.service";


@Component({
  selector: 'app-model-operation-add',
  templateUrl: './model-operation-add.component.html',
  styleUrls: ['./model-operation-add.component.scss']
})
export class ModelOperationAddComponent implements OnInit {
  isCheckedQuality: any = false;
  isCheckedEfficiency: any = false;
  modelOperations = new ModelOperation();
  stageList: Array<Select2OptionData> = [];
  processTypeList: Array<Select2OptionData> = [];
  modelName: string = "";
  modelLocal: { modelNo?: string, modelName?: string } = {};

  constructor(private modelOperationService: ModelOperationService,
    private snotifyAlert: NgSnotifyService,
    private router: Router) { }

  ngOnInit() {
    this.modelOperations.stage_id = '';
    this.modelOperations.process_type_id = '';
    this.modelLocal = JSON.parse(localStorage.getItem("modelLocal"));
    if (this.modelLocal) {
      this.modelOperations.model_no = this.modelLocal.modelNo;
      this.modelName = this.modelLocal.modelName;
    } else {
      this.router.navigate(["/model-operation/list"]);
    }
    this.getStage();
    this.getProcessType();
    this.modelOperations.sequence = 0;
  }


  changeQuality() {
    this.modelOperations.critical_quality = this.isCheckedQuality;
  }

  changeEfficiency() {
    this.modelOperations.critical_efficiency = this.isCheckedEfficiency;
  }


  backList() {
    this.router.navigate(["/model-operation/list"]);
  }

  saveAndNext() {
    this.modelOperations.operation_id = this.modelOperations.operation_id.trim().toUpperCase();
    this.modelOperationService.createModelOperation(this.modelOperations).subscribe(
      () => {
        this.snotifyAlert.success(MessageConstants.CREATED_OK_MSG, CaptionConstants.SUCCESS);
        this.modelOperations = new ModelOperation;
        this.modelOperations.model_no = this.modelLocal.modelNo;
        this.modelOperations.stage_id = "";
        this.modelOperations.process_type_id = "";
        this.isCheckedQuality = false;
        this.isCheckedEfficiency = false;
      },
      (error) => {
        this.snotifyAlert.error(error, CaptionConstants.ERROR);
      }
    );
  }

  save() {
    this.modelOperations.operation_id = this.modelOperations.operation_id.trim().toUpperCase();
    this.modelOperationService.createModelOperation(this.modelOperations).subscribe(
      () => {
        this.snotifyAlert.success(MessageConstants.CREATED_OK_MSG, CaptionConstants.SUCCESS);
        this.router.navigate(["/model-operation/list"]);
      },
      (error) => {
        this.snotifyAlert.error(MessageConstants.CREATED_ERROR_MSG, CaptionConstants.ERROR);
      }
    );
  }

  getStage() {
    this.modelOperationService.getStage().subscribe(res => {
      this.stageList = res.map(item => {
        return { id: item.stage_id, text: item.stage_name };
      });
    });
  }


  getProcessType() {
    this.modelOperationService.getProcessType().subscribe(res => {
      this.processTypeList = res.map(item => {
        return { id: item.process_type_id, text: item.process_type_name_en };
      });
    });
  }

}
