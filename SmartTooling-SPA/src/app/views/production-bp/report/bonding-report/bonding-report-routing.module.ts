import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { BondingReportDetailComponent } from './bonding-report-detail/bonding-report-detail.component';
import { BondingReportMainComponent } from './bonding-report-main/bonding-report-main.component';


const routes: Routes = [
  {
    path: "",
        data: {
            title: "Bonding Program Report",
        },
        children: [
            {
                path: "main",
                component: BondingReportMainComponent,
                data: {
                    title: "Bonding Program Report Main",
                },
            },
            {
                path: "detail",
                component: BondingReportDetailComponent,
                data: {
                    title: "Bonding Program Report Detail",
                },
            },
        ],
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BondingReportRoutingModule { }
