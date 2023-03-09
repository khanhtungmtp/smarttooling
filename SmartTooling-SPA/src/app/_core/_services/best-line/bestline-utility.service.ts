import { Injectable } from '@angular/core';
import { C2BLayoutByProcessDetailDTO } from '../../_models/best-line/c2BLayoutByProcessDetailDTO';

@Injectable({
  providedIn: 'root'
})
export class BestlineUtilityService {

  constructor() { }

  getChartOptions() {
    let option = {
      title: {
        display: false,
        padding: 10
      },
      responsive: true,
      maintainAspectRatio: false,
      scales: {
        xAxes: [{}],
        yAxes: [
          {
            id: 'y-axis-0',
            position: 'left',
            ticks: {
              beginAtZero: true
            }
          },
          {
            id: 'y-axis-1',
            position: 'right',
            ticks: {
              beginAtZero: true,
              stepSize: 1,
              display: false
            }
          }
        ]
      },
      plugins: {
        datalabels: {
          anchor: 'center',
          align: 'center'
        },
      },
      legend: {
        align: 'center',
        display: true,
        position: 'bottom'
      }
    };
    return option;
  }

  getChartData(graphData: C2BLayoutByProcessDetailDTO, beforeOrAfter: string){
    let chartData = [
      {
        data: beforeOrAfter === 'before' ? graphData.listEmployeeBefore : graphData.listEmployeeAfter,
        pointRadius: 0,
        type: 'line',
        label: 'Employee Qty',
        yAxisID: 'y-axis-1',
        backgroundColor: 'transparent',
        showLine: false,
        hidden: false
      },
      {
        data: beforeOrAfter === 'before' ? graphData.listTaktTimeBefore : graphData.listTaktTimeAfter,
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
        data: beforeOrAfter === 'before' ? graphData.listDataBefore : graphData.listDataAfter,
        type: 'bar',
        backgroundColor: '#4175fa',
        label: graphData.typeName,
        datalabels: {
          display: false
        }
      },
    ];
    return chartData;
  }
}
