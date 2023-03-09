import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { PadPrintSettingAddComponent } from "./pad-print-setting-add/pad-print-setting-add.component";
import { PadPrintSettingEditComponent } from "./pad-print-setting-edit/pad-print-setting-edit.component";
import { PadPrintSettingMainComponent } from "./pad-print-setting-main/pad-print-setting-main.component";




const routes: Routes = [
    {
        path: "",
        data: {
            title: "Pad Print Setting",
        },
        children: [
            {
                path: "main",
                component: PadPrintSettingMainComponent,
                data: {
                    title: "Pad Print Setting Main",
                },
            },
            {
                path: "add",
                component: PadPrintSettingAddComponent,
                data: {
                    title: "Pad Print Setting Add",
                },
            },
            {
                path: "edit",
                component: PadPrintSettingEditComponent,
                data: {
                    title: "Pad Print Setting Edit",
                },
            },
        ],
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class PadPrintSettingRoutingModule { }