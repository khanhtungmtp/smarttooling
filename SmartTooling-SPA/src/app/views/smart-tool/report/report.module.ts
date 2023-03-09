import { NgModule } from "@angular/core";
import { KaizenReportModule } from "./kaizen-report/kaizen-report.module";
import { GroupKaizenReportModule } from "./group-kaizen-report/group-kaizen-report.module";
import { RftReportModule } from "./rft-report/rft-report.module";
import { GroupRftReportModule } from "./group-rft-report/group-rft-report.module";
import { ReportRoutingModule } from "./report-routing.module";
import { CrossSiteSharingModule } from "./cross-site-sharing/cross.module";
import { KaizenReportGuard } from "../../../_core/_guards/smart-tool/kaizen-report.guard";
import { GroupKaizenReportGuard } from "../../../_core/_guards/smart-tool/group-kaizen-report.guard";
import { RFTReportGuard } from "../../../_core/_guards/smart-tool/rft-report.guard";
import { GroupRFTReportGuard } from "../../../_core/_guards/smart-tool/group-rft-report.guard";
import { CrossSiteSharingGuard } from "../../../_core/_guards/smart-tool/cross-site-sharing.guard";
@NgModule({
  declarations: [],
  imports: [
    KaizenReportModule,
    GroupKaizenReportModule,
    RftReportModule,
    GroupRftReportModule,
    ReportRoutingModule,
    CrossSiteSharingModule,
  ],
  providers:[
    KaizenReportGuard,
    GroupKaizenReportGuard,
    RFTReportGuard,
    GroupRFTReportGuard,
    CrossSiteSharingGuard,
  ]
})
export class ReportModule {}
