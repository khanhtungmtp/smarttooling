import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { OverallLayoutReportDetailComponent } from "./overall-layout-report-detail/overall-layout-report-detail.component";
import { OverallLayoutReportMainComponent } from "./overall-layout-report-main/overall-layout-report-main.component";


const routes: Routes = [
    {
        path: "",
        data: {
            title: "Overall Layout Report",
        },
        children: [
            {
                path: "main",
                component: OverallLayoutReportMainComponent,
                data: {
                    title: "Overall Layout Report Main",
                },
            },
            {
                path: "detail",
                component: OverallLayoutReportDetailComponent,
                data: {
                    title: "Overall Layout Report Detail",
                },
            },
        ],
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class C2BOverallLayoutReportRoutingModule { }
