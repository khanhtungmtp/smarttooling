import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PilotLineSetupSummaryTrackingComponent } from './pilot-line-setup-summary-tracking/pilot-line-setup-summary-tracking.component';

const routes: Routes = [
    {
      path: "",
      data: {
        title: "Report"
      },
      children: [
        {
          path: "",
          component: PilotLineSetupSummaryTrackingComponent,
          data: {
            title: "Pilot Line Setup Summary Tracking"
          }
        }
      ]
    }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PilotLineSetupSummaryTrackingReportRoutingModule { }
