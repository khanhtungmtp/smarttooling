import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { CriticalProcessReportDetailComponent } from "./critical-process-report-detail/critical-process-report-detail.component";
import { CriticalProcessReportMainComponent } from "./critical-process-report-main/critical-process-report-main.component";
import { KaizenDetailComponent } from "./kaizen-detail/kaizen-detail.component";



const routes: Routes = [
    {
        path: "",
        data: {
            title: "Critical Process Report",
        },
        children: [
            {
                path: "main",
                component: CriticalProcessReportMainComponent,
                data: {
                    title: "Critical Process Report Main",
                },
            },
            {
                path: "detail",
                component: CriticalProcessReportDetailComponent,
                data: {
                    title: "Critical Process Report Detail",
                },
            },
            {
                path: "kaizen-detail",
                component: KaizenDetailComponent,
                data: {
                    title: "Kaizen Detail",
                }
            }
        ],
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class CriticalProcessReportRoutingModule { }
