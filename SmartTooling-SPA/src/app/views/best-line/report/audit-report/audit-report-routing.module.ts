import { AuditReportDetailComponent } from './audit-report-detail/audit-report-detail.component';
import { AuditReportMainComponent } from './audit-report-main/audit-report-main.component';
import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { AuditReportDetailResolver } from '../../../../_core/_resolvers/best-line/audit-report-detail.resolver';

const routes: Routes = [
  {
    path: "",
    data: {
      title: "Report"
    },
    children: [
      {
        path: "",
        component: AuditReportMainComponent,
        data: {
          title: "Audit Report"
        }
      },
      {
        path: "audit-report-detail",
        resolve: {auditDetail: AuditReportDetailResolver},
        component: AuditReportDetailComponent,
        data: {
          title: "Audit Report Detail"
        }
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AuditReportRoutingModule {}
