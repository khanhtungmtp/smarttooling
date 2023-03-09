import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ChartDataSets, ChartType } from 'chart.js';
import * as pluginDataLabels from 'chartjs-plugin-datalabels';
import { Label } from 'ng2-charts';
import { of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../../../../../../environments/environment';
import { CaptionConstants, MessageConstants } from '../../../../../_core/_constants/system.constants';
import { BL_Layout_Design_ProcessDTO } from '../../../../../_core/_models/best-line/bl-layout-design-by-process-dto';
import { C2BLayoutByProcessDetailDTO } from '../../../../../_core/_models/best-line/c2BLayoutByProcessDetailDTO';
import { C2BLayoutByProcessDTO } from '../../../../../_core/_models/best-line/layout-by-process-dto';
import { C2BLayoutByProcessReportParam } from '../../../../../_core/_models/best-line/layout-by-process-report-param';
import { BestlineUtilityService } from '../../../../../_core/_services/best-line/bestline-utility.service';
import { C2bLayoutByProcessReportService } from '../../../../../_core/_services/best-line/c2b-layout-by-process-report.service';
import { NgSnotifyService } from '../../../../../_core/_services/ng-snotify.service';
@Component({
  selector: 'app-c2b-layout-by-process-report-detail',
  templateUrl: './c2b-layout-by-process-report-detail.component.html',
  styleUrls: ['./c2b-layout-by-process-report-detail.component.css']
})
export class C2bLayoutByProcessReportDetailComponent implements OnInit {
  baseUrl = environment.imageUrl;
  searchModel: C2BLayoutByProcessReportParam = {} as C2BLayoutByProcessReportParam;
  dataModel: BL_Layout_Design_ProcessDTO = {} as BL_Layout_Design_ProcessDTO;
  editModel: C2BLayoutByProcessDTO = {} as C2BLayoutByProcessDTO;
  graphData: C2BLayoutByProcessDetailDTO = {} as C2BLayoutByProcessDetailDTO;
  random: number = Math.random();
  // For Graph

  beforeChartData: ChartDataSets[] = []
  beforeLabels: Label[] = this.graphData.listNodeNameBefore;
  beforeChartType: ChartType = 'bar';
  beforeChartPlugins = [pluginDataLabels]

  afterChartData: ChartDataSets[] = []
  afterLabels: Label[] = this.graphData.listNodeNameBefore;
  afterChartType: ChartType = 'bar';
  afterChartPlugins = [pluginDataLabels]

  chartOptions = {};
  // End of Graph
  constructor(
    private router: Router,
    private snotify: NgSnotifyService,
    private layoutByProcessReportService: C2bLayoutByProcessReportService,
    private bestlineUtility: BestlineUtilityService) { }

  async ngOnInit() {
    this.layoutByProcessReportService.currentModel.subscribe(res => {
      if (res === null) {
        this.back();
      }
      else {
        this.editModel = res;
        this.searchModel = {
          factory: this.editModel.factory_id,
          line_no: this.editModel.line_id,
          line_type: this.editModel.line_type_id,
          model: this.editModel.model_no,
          process_no: this.editModel.process_type_id,
        }
      }
    }).unsubscribe();
    await this.getLayoutDesignProcess();
    await this.getChartOption();
    await this.getLayoutDesignProcessData();
  }

  async getLayoutDesignProcess() {
    let res = await this.layoutByProcessReportService.GetLayoutDesignProcess(this.searchModel).pipe(catchError(e => {
      this.snotify.error(MessageConstants.SYSTEM_ERROR_MSG, CaptionConstants.ERROR);
      return of(null);
    })).toPromise()
    if (res) {
      this.dataModel = res;
      this.dataModel.each_process_image_after = this.baseUrl + this.dataModel.each_process_image_after;
      this.dataModel.each_process_image_before = this.baseUrl + this.dataModel.each_process_image_before;
    }
  }

  async getLayoutDesignProcessData() {
    let res = await this.layoutByProcessReportService.GetLayoutDesignProcessData(this.searchModel).pipe(catchError(e => {
      this.snotify.error(MessageConstants.SYSTEM_ERROR_MSG, CaptionConstants.ERROR);
      return of(null);
    })).toPromise();
    if (res) {
      this.graphData = res;
      this.beforeChartData = [
        {
          data: this.graphData.listEmployeeBefore,
          pointRadius: 0,
          type: 'line',
          label: 'Employee Qty',
          yAxisID: 'y-axis-1',
          backgroundColor: 'transparent',
          showLine: false,
          hidden: false
        },
        {
          data: this.graphData.listTaktTimeBefore,
          pointRadius: 0,
          type: 'line',
          label: 'Takt Time',
          borderColor: '#eda247',
          backgroundColor: 'transparent',
          datalabels: {
            display: false
          }
        },
        {
          data: this.graphData.listDataBefore,
          type: 'bar',
          backgroundColor: '#4175fa',
          label: this.editModel.process_type_id,
          datalabels: {
            display: false
          }
        },
      ];
      this.beforeLabels = this.graphData.listNodeNameBefore;

      this.afterChartData = [
        {
          data: this.graphData.listEmployeeAfter,
          pointRadius: 0,
          type: 'line',
          label: 'Employee Qty',
          yAxisID: 'y-axis-1',
          backgroundColor: 'transparent',
          showLine: false
        },
        {
          data: this.graphData.listTaktTimeAfter,
          pointRadius: 0,
          type: 'line',
          label: 'Takt Time',
          backgroundColor: 'transparent',
          borderColor: '#eda247',
          datalabels: {
            display: false
          }
        },
        {
          data: this.graphData.listDataAfter,
          type: 'bar',
          backgroundColor: '#4175fa',
          label: this.editModel.process_type_id,
          datalabels: {
            display: false
          }
        }
      ];
      this.afterLabels = this.graphData.listNodeNameAfter;
    }
  }

  back() {
    this.router.navigate(['/best-line/report/layout-by-process-report/main']);
  }

  async getChartOption() {
    this.chartOptions = this.bestlineUtility.getChartOptions();
  }
}
