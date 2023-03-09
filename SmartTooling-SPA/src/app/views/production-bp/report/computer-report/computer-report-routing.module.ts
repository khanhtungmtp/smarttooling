import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ComputerReportDetailComponent } from './computer-report-detail/computer-report-detail.component';
import { ComputerReportMainComponent } from './computer-report-main/computer-report-main.component';


const routes: Routes = [
  {
    path: "",
        data: {
            title: "Computer Stitching Setting Report",
        },
        children: [
            {
                path: "main",
                component: ComputerReportMainComponent,
                data: {
                    title: "Computer Stitching Setting Report Main",
                },
            },
            {
                path: "detail",
                component: ComputerReportDetailComponent,
                data: {
                    title: "Computer Stitching Setting Report Detail",
                },
            },
        ],
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ComputerReportRoutingModule { }
