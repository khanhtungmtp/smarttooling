import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { C2bLayoutByProcessReportDetailComponent } from "./c2b-layout-by-process-report-detail/c2b-layout-by-process-report-detail.component";
import { C2bLayoutByProcessReportMainComponent } from "./c2b-layout-by-process-report-main/c2b-layout-by-process-report-main.component";



const routes: Routes = [
    {
        path: "",
        data: {
            title: "Layout by Process Report",
        },
        children: [
            {
                path: "main",
                component: C2bLayoutByProcessReportMainComponent,
                data: {
                    title: "C2B Layout by Process Report Main",
                },
            },
            {
                path: "detail",
                component: C2bLayoutByProcessReportDetailComponent,
                data: {
                    title: "C2B Layout by Process Report Detail",
                },
            },
        ],
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class C2BLayoutByProcessReportRoutingModule { }