import { CommonModule } from "@angular/common";
import { HttpClientModule } from "@angular/common/http";
import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { NgSelect2Module } from "ng-select2";
import { PaginationModule } from "ngx-bootstrap/pagination";
import { NgxSpinnerModule } from "ngx-spinner";
import { AuditReportDetailComponent } from "./audit-report-detail/audit-report-detail.component";
import { AuditReportMainComponent } from "./audit-report-main/audit-report-main.component";
import { AuditReportRoutingModule } from "./audit-report-routing.module";

@NgModule({
  imports: [
    HttpClientModule,
    CommonModule,
    AuditReportRoutingModule,
    NgSelect2Module,
    FormsModule,
    ReactiveFormsModule,
    PaginationModule,
    NgxSpinnerModule,
  ],
  declarations: [
    AuditReportMainComponent,
    AuditReportDetailComponent
  ]
})

export class AuditReportModule {}
