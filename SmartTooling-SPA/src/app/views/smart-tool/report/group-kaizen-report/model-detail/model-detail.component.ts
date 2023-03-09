import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import * as Highcharts from "highcharts";
import { environment } from "../../../../../../environments/environment";
import { Efficiency } from "../../../../../_core/_models/smart-tool/efficiency";
import { ModelKaizenReport } from "../../../../../_core/_models/smart-tool/model-kaizen-report";
import { Pagination } from "../../../../../_core/_models/smart-tool/pagination";
import { GroupKaizenReportService } from "../../../../../_core/_services/smart-tool/group-kaizen-report.service";
import { FunctionUtility } from "../../../../../_core/_utility/function-utility";

declare var $: any;
@Component({
  selector: 'app-model-detail',
  templateUrl: './model-detail.component.html',
  styleUrls: ['./model-detail.component.scss']
})
export class ModelDetailComponent implements OnInit {
  highcharts = Highcharts;
  model: ModelKaizenReport;
  dataChart: Efficiency[];
  efficiencyTargetData: number[] = [];
  actualModelEfficiencyData: number[] = [];
  monthData: string[] = [];
  seasons: string[] = [];
  season: string;
  chartOptions = null;
  dataTable: any = [];
  url: any = environment.imageUrl;
  pagination: Pagination = {
    currentPage: 1,
    itemsPerPage: 10,
    totalItems: 0,
    totalPages: 0,
  };
  random: number = Math.random();
  constructor(
    private router: Router,
    private functionUtility: FunctionUtility,
    private kaizenService: GroupKaizenReportService
  ) { }

  ngOnInit() {
    this.kaizenService.currentModel.subscribe((res) => (this.model = res));
    if (this.model !== null) {
      this.model.volume_string = this.functionUtility.convertNumber(this.model.volume);
      // Thay thế kí tự ngắt dòng bằng thẻ <br>
      this.model.remarks = this.functionUtility.replaceLineBreak(this.model.remarks);
      this.getSeasonByUpperID();
      this.getDataTable();
    } else {
      this.backForm();
    }
  }

  getSeasonByUpperID() {
    this.kaizenService
      .getSeasonByUpper(this.model.factory_id,this.model.upper_id)
      .subscribe((res) => {
        this.seasons = res;
        if (this.seasons.length > 0) {
          this.season = this.seasons[0].toString();
          this.getDataChart();
        }
      });
  }
  getDataChart() {
    this.kaizenService
      .getDataChart(this.model.factory_id,this.model.upper_id, this.season)
      .subscribe((res) => {
        this.dataChart = res;
        this.efficiencyTargetData = this.dataChart.map((obj) => {
          return obj.efficiency_target;
        });
        this.actualModelEfficiencyData = this.dataChart.map((obj) => {
          return obj.efficiency_actual;
        });
        let dataMonth = this.dataChart.map((obj) => {
          return obj.month.toString().trim();
        });
        this.monthData = dataMonth.map((obj) => {
          if (obj === "1") {
            obj = "January";
          } else if (obj === "2") {
            obj = "February";
          } else if (obj === "3") {
            obj = "March";
          } else if (obj === "4") {
            obj = "April";
          } else if (obj === "5") {
            obj = "May";
          } else if (obj === "6") {
            obj = "June";
          } else if (obj === "7") {
            obj = "July";
          } else if (obj === "8") {
            obj = "August";
          } else if (obj === "9") {
            obj = "September";
          } else if (obj === "10") {
            obj = "October";
          } else if (obj === "11") {
            obj = "November";
          } else if (obj === "12") {
            obj = "December";
          }
          return obj;
        });
        this.chartOptions = {
          chart: {
            type: "spline",
          },
          title: {
            text: "Model Efficiency Tracking",
          },
          //  subtitle: {
          //     text: "Source: WorldClimate.com"
          //  },
          xAxis: {
            categories: this.monthData,
          },
          yAxis: {
            title: {
              text: "",
            },
            labels: {
              formatter: function() {
                return this.value + '%';
              }
            }
          },
          plotOptions: {
            series: {
              dataLabels: {
                enabled: true,
                format: '{y} %'
              },
              enableMouseTracking: true
            },

          },
          tooltip: {
            valueSuffix: "%",
            enabled: false,
          },
          series: [
            {
              name: "Model Efficiency Target",
              data: this.efficiencyTargetData,
              color: "#62a8e8",
            },
            {
              name: "Actual Model Efficiency",
              data: this.actualModelEfficiencyData,
              color: "#de8e58",
            },
          ],
        };
      });
  }

  changeSeason() {
    this.getDataChart();
  }
  getDataTable() {
    this.kaizenService.getKaizens(this.pagination.currentPage, this.pagination.itemsPerPage, this.model.factory_id, this.model.model_no)
    .subscribe(res => {
      console.log(res);
      this.dataTable = res.result;
        this.dataTable.map((obj) => {
          obj.improv = Math.round(
            100 * ((obj.ct_before_sec - obj.ct_after_sec) / obj.ct_before_sec)
          );
          return obj;
        });
        this.pagination = res.pagination;
    });
  }
  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.getDataTable();
  }
  kaizenDetail(item: object) {
    this.kaizenService.updateClickTimes(item).subscribe();
    this.kaizenService.changeKaizen(item);
    this.router.navigate(['/report/group-kaizen-report/kaizen-detail']);
  }
  backForm() {
    this.router.navigate(["/report/group-kaizen-report/main"]);
  }

  ngAfterViewChecked() {
    $('.highcharts-credits').html('');
  }
}
