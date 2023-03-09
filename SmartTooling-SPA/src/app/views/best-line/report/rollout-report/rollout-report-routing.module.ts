import { RouterModule, Routes } from "@angular/router";
import { NgModule } from "@angular/core";
import { RolloutReportComponent } from "./rollout-report.component";

const routes: Routes = [
  {
    path: "",
    data: {
      title: "Report"
    },
    children: [
      {
        path: "",
        component: RolloutReportComponent,
        data: {
          title: "Rollout Report"
        }
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class RolloutReportRoutingModule {}
