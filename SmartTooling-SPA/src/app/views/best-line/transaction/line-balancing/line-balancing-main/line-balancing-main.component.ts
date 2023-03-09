import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ChartDataSets, ChartType } from 'chart.js';
import { Select2OptionData } from 'ng-select2';
import { Label } from 'ng2-charts';
import * as pluginDataLabels from 'chartjs-plugin-datalabels';
import { BL_Layout_Design_Process } from '../../../../../_core/_models/best-line/bL_Layout_Design_Process';
import { C2BLayoutByProcessDetailDTO } from '../../../../../_core/_models/best-line/c2BLayoutByProcessDetailDTO';
import { LineBalancingService } from '../../../../../_core/_services/best-line/line-balancing.service';
import { BL_Layout_Design_Process_Data_Params } from '../../../../../_core/_models/best-line/bL_Layout_Design_Process_Data_ParamsDTO';
import { NgSnotifyService } from '../../../../../_core/_services/ng-snotify.service';
import { BestlineUtilityService } from '../../../../../_core/_services/best-line/bestline-utility.service';

@Component({
  selector: 'app-line-balancing-main',
  templateUrl: './line-balancing-main.component.html',
  styleUrls: ['./line-balancing-main.component.scss']
})
export class LineBalancingMainComponent implements OnInit {
  
  arrLineId: Array<Select2OptionData>;
  arrLineTypeId: Array<Select2OptionData>;
  arrModelNo: Array<Select2OptionData>;
  arrProcess: Array<Select2OptionData>;
  arrModelNoAndName: Array<Select2OptionData>;
  lineId: string = '';
  lineIdTemp: string = '';
  lineName: string = '';
  lineTypeId: string = '';
  lineTypeName: string = '';
  modelName: string = '';
  modelNo: string = '';
  processType: string = '';
  processTypeName: string = '';
  

  searchModel: BL_Layout_Design_Process_Data_Params = {
    line_id: '',
    line_type_id: '',
    model_no: '',
    process_type_id: '',
    before_or_after: ''
  };
  graphData: C2BLayoutByProcessDetailDTO = {} as C2BLayoutByProcessDetailDTO;
  dataModel: BL_Layout_Design_Process;

  // For Graph

  afterChartData: ChartDataSets[] = []
  beforeChartData: ChartDataSets[] = []
  beforeLabels: Label[] = this.graphData.listNodeNameBefore;
  afterLabels: Label[] = this.graphData.listNodeNameBefore;
  beforeChartType: ChartType = 'bar';
  afterChartType: ChartType = 'bar';
  beforeChartPlugins = [pluginDataLabels]
  afterChartPlugins = [pluginDataLabels]
  // End of Graph
  chartOptions = {};

  constructor(
    private _lineBalancingService: LineBalancingService,
    private router: Router,
    private snotifyalert: NgSnotifyService,
    private cd: ChangeDetectorRef,
    private bestLineUtility : BestlineUtilityService
  ) { }

  ngOnInit() {
    this._lineBalancingService.currentLineBalancing.subscribe(res => {
      if (res != null) {
        this.lineId = this.lineIdTemp = res.lineID;
        this.lineTypeId = res.lineTypeId;
        this.modelNo = res.modelNo;
        this.modelName = res.modelName;
        this.processType = res.processType;
      }
      else {
        [this.lineId, this.lineTypeId, this.modelNo, this.modelName] = ['', '', '', ''];
      }
    }).unsubscribe();
    this.getChartOptions();
    this.getLineId();
  }

  ngAfterViewChecked(): void {
    this.cd.detectChanges();
  }

  getChartOptions(){
    this.chartOptions = this.bestLineUtility.getChartOptions();
  }

  getLineId() {
    this._lineBalancingService.getLineID().subscribe(res => {
      this.arrLineId = res.map(item => {
        return { id: item.id, text: item.text };
      })
    });
  }

  lineIDChange(event) {
    if (event != '') {
      this.lineId = this.arrLineId.find(x => x.id == event)?.id;
      this.lineName = this.arrLineId.find(x => x.id == event)?.text;
      if (this.lineId != this.lineIdTemp) {
        [this.lineTypeId, this.modelNo, this.modelName] = ['', '', ''];
        this.lineIdTemp = this.lineId;
      }
      this.getLineTypeID();
    }
    else {
      [this.lineTypeId, this.modelNo, this.modelName, this.processType] = ['', '', '', ''];
    }

  }

  getLineTypeID() {
    this._lineBalancingService.getLineTypeID(this.lineId).subscribe(res => {
      this.arrLineTypeId = res.map(item => {
        return { id: item.id, text: item.text };
      })
    });
  }

  lineTypeIDChange(event) {
    if (event != '') {
      this.lineTypeId = this.arrLineTypeId.find(x => x.id == event).id;
      this.lineTypeName = this.arrLineTypeId.find(x => x.id == event).text;
      this.arrModelNo = [];
      this.getModelNo();
    }
    else {
      [this.modelNo, this.modelName, this.processType] = ['', '', ''];
    }
  }

  getModelNo() {
    this._lineBalancingService.getmodelNo(this.lineId, this.lineTypeId).subscribe(res => {
      this.arrModelNoAndName = res.map(item => {
        return { id: item.id, text: item.text }
      })
      this.arrModelNo = res.map(item => {
        return { id: item.id, text: item.id }
      })
    });
  }

  modelNoChange(event) {
    if (event != '') {
      this.modelNo = this.arrModelNoAndName?.find(x => x.id == event)?.id;
      this.modelName = this.arrModelNoAndName?.find(x => x.id == event)?.text;
      this.arrProcess = [];
      this.getProcessType();
    }
    else {
      [this.modelName, this.processType] = ['', ''];
    }

  }

  getProcessType() {
    this._lineBalancingService.getProcessType(this.lineId, this.lineTypeId, this.modelNo).subscribe(res => {
      this.arrProcess = res.map(item => {
        return { id: item.id, text: item.text }
      })
    });
  }

  processTypeChange(event) {
    if (event != '') {
      this.processType = this.arrProcess?.find(x => x.id == event)?.id;
      this.processTypeName = this.arrProcess?.find(x => x.id == event)?.text;
      this.getLayoutDesignProcessData();
    }
  }

  inputData() {
    this._lineBalancingService.changeIDProcess({
      lineID: this.lineId,
      lineName: this.lineName,
      lineTypeId: this.lineTypeId,
      lineTypeName: this.lineTypeName,
      modelNo: this.modelNo,
      modelName: this.modelName,
      processType: this.processType,
      processTypeName: this.processTypeName
    })
    this.router.navigate(['/best-line/transaction/line-balancing/input']);
  }

  getLayoutDesignProcessData() {
    this.searchModel.line_id = this.lineId;
    this.searchModel.line_type_id = this.lineTypeId;
    this.searchModel.model_no = this.modelNo;
    this.searchModel.process_type_id = this.processType;
    this.searchModel.before_or_after = ''

    this._lineBalancingService.GetLayoutDesignProcessData(this.searchModel).subscribe(res => {
      this.graphData = res;
      this.beforeChartData = this.bestLineUtility.getChartData(this.graphData, 'before');
      this.beforeLabels = this.graphData.listNodeNameBefore;

      this.afterChartData = this.bestLineUtility.getChartData(this.graphData, 'after');
      this.afterLabels = this.graphData.listNodeNameAfter;
    }, error => { this.snotifyalert.error("server error") });
  }

  getExportExcel() {
    this.searchModel.line_id = this.lineId;
    this.searchModel.line_type_id = this.lineTypeId;
    this.searchModel.model_no = this.modelNo;
    this.searchModel.process_type_id = this.processType;
    this.searchModel.before_or_after = ''

    this._lineBalancingService.getExportExcel(this.searchModel);
  }

}
