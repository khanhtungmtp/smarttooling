import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { NgSelect2Module } from "ng-select2";
import { PaginationModule } from "ngx-bootstrap/pagination";
import { NgxSpinnerModule } from "ngx-spinner";
import { PadPrintSettingReportDetailComponent } from "./pad-print-setting-report-detail/pad-print-setting-report-detail.component";
import { PadPrintSettingReportMainComponent } from "./pad-print-setting-report-main/pad-print-setting-report-main.component";
import { PadPrintSettingReportRoutingModule } from "./pad-print-setting-report-routing.module";
@NgModule({
    declarations: [
        PadPrintSettingReportMainComponent,
        PadPrintSettingReportDetailComponent
    ],
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        PadPrintSettingReportRoutingModule,
        NgxSpinnerModule,
        PaginationModule,
        NgSelect2Module,
    ],
})
export class PadPrintSettingReportModule { }
