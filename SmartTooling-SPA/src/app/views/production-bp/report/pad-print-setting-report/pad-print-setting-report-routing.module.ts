import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { PadPrintSettingReportGuard } from "../../../../_core/_guards/production-bp/pad-print-setting-report.guard";
import { PadPrintSettingReportDetailComponent } from "./pad-print-setting-report-detail/pad-print-setting-report-detail.component";
import { PadPrintSettingReportMainComponent } from "./pad-print-setting-report-main/pad-print-setting-report-main.component";

const routes: Routes = [
    {
        path: "",
        data: {
            title: "Pad Print Setting Report",
        },
        children: [
            {
                path: "main",
                component: PadPrintSettingReportMainComponent,
                data: {
                    title: "Pad Print Setting Report Main",
                },
            },
            {
                path: "detail",
                component: PadPrintSettingReportDetailComponent,
                data: {
                    title: "Pad Print Setting Report Detail",
                },
            },

        ],
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class PadPrintSettingReportRoutingModule { }
