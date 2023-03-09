import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { Select2OptionData } from "ng-select2";
import { CaptionConstants, MessageConstants } from "../../../../../_core/_constants/system.constants";
import { ModelOperation } from "../../../../../_core/_models/smart-tool/model-operation";
import { ModelOperationEditParam } from "../../../../../_core/_models/smart-tool/model-operationEditParam";
import { NgSnotifyService } from "../../../../../_core/_services/ng-snotify.service";
import { ModelOperationService } from "../../../../../_core/_services/smart-tool/model-operation.service";


@Component({
  selector: 'app-model-operation-edit',
  templateUrl: './model-operation-edit.component.html',
  styleUrls: ['./model-operation-edit.component.scss']
})
export class ModelOperationEditComponent implements OnInit {
  isCheckedQuality: any = false;
  isCheckedEfficiency: any = false;
  modelOperation = new ModelOperation();
  modelOperationEditParam = new ModelOperationEditParam();
  stageList: Array<Select2OptionData> = [];
  processTypeList: Array<Select2OptionData> = [];
  constructor(private modelOperationService: ModelOperationService,
    private snotifyAlert: NgSnotifyService,
    private router: Router) { }

  ngOnInit() {
    this.modelOperation.process_type_id = '';
    this.modelOperation.stage_id = '';
    if (localStorage.getItem("modelOperationEditParam")) {
      this.modelOperationEditParam = JSON.parse(localStorage.getItem("modelOperationEditParam"));
    } else {
      this.router.navigate(["/model-operation/list"]);
    }
    this.loadModelOperation();
    this.getStage();
    this.getProcessType();
  }


  loadModelOperation() {
    this.modelOperationService.getModelOperationEdit(this.modelOperationEditParam).subscribe(
      res => {
        this.modelOperation = res;
      }, error => {
        this.snotifyAlert.error("Can not load Model Operation", CaptionConstants.ERROR);
      });
  }

  save() {
    this.modelOperationService.updateModelOperation(this.modelOperation).subscribe(
      () => {
        this.router.navigate(["/model-operation/list"]);
        this.snotifyAlert.success(MessageConstants.UPDATED_OK_MSG, CaptionConstants.SUCCESS);
      },
      (error) => {
        this.snotifyAlert.error(MessageConstants.UPDATED_ERROR_MSG, CaptionConstants.ERROR);
      });
  }

  backList() {
    this.router.navigate(["/model-operation/list"]);
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
