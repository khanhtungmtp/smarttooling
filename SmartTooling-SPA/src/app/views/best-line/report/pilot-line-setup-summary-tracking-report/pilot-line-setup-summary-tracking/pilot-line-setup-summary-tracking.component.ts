import { Component, OnInit } from '@angular/core';
import { Select2OptionData } from 'ng-select2';
import { NgxSpinnerService } from 'ngx-spinner';
import { MessageConstants } from '../../../../../_core/_constants/system.constants';
import { PilotLineSetupSummaryTrackingGroupReport } from '../../../../../_core/_models/smart-tool/pilot-line-setup-summary-tracking-report';
import { NgSnotifyService } from '../../../../../_core/_services/ng-snotify.service';
import { PilotLineReportService } from '../../../../../_core/_services/smart-tool/pilot-line-report.service';
import { FunctionUtility } from '../../../../../_core/_utility/function-utility';

@Component({
  selector: 'app-pilot-line-setup-summary-tracking',
  templateUrl: './pilot-line-setup-summary-tracking.component.html',
  styleUrls: ['./pilot-line-setup-summary-tracking.component.scss']
})
export class PilotLineSetupSummaryTrackingComponent implements OnInit {
  message = MessageConstants;
  prodSeason: string = null;

  prodSeasons: Array<Select2OptionData> = [];
  dataGroups: PilotLineSetupSummaryTrackingGroupReport[] = [];
  constructor(
    private pilotServices: PilotLineReportService,
    private functionUtility: FunctionUtility,
    private spinnerService: NgxSpinnerService,
    private snotify: NgSnotifyService,
  ) { }

  ngOnInit(): void {
    this.getSeasons();
  }

  getSeasons() {
    this.spinnerService.show();
    this.pilotServices.getSeasons().subscribe({
      next: result => {
        this.prodSeasons = result.map(x => {
          return { id: x, text: x }
        })
        this.spinnerService.hide();
      }
    })
  }

  getPilotsReport() {
    this.spinnerService.show();
    this.pilotServices.getAllPilots(this.prodSeason).subscribe({
      next: result => {
        this.dataGroups = result;
        this.spinnerService.hide();
      }
    })
  }

  clear() {
    this.prodSeason = null;
    this.dataGroups = [];
  }

  exportExcel() {
    this.spinnerService.show();
    this.pilotServices.exportExcel(this.prodSeason)
      .subscribe({
        next: (result: Blob) => {
          if (result.size === 0) {
            this.snotify.error("No Data");
            this.spinnerService.hide();
            return;
          } else {
            if (result.type !== "xlsx") {
              this.spinnerService.hide();
            }

            const blob = new Blob([result]);
            const url = window.URL.createObjectURL(blob);
            const link = document.createElement("a");
            const currentTime = new Date();
            const fileName =
              "Pilot_Line_Setup_Summary_Tracking_Report_" +
              currentTime.getFullYear().toString() +
              (currentTime.getMonth() + 1) +
              currentTime.getDate() +
              ".xlsx";
            link.href = url;
            link.setAttribute("download", fileName);
            document.body.appendChild(link);
            link.click();
          }
        },
        error: () => { this.spinnerService.hide(); this.snotify.error("Error!") },
        complete: () => this.spinnerService.hide(),
      });
  }

  search = () => this.functionUtility.isEmpty(this.prodSeason) ? this.dataGroups = [] : this.getPilotsReport();
}
