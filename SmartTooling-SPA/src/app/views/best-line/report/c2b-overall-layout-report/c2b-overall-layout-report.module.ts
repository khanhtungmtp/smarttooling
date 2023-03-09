import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OverallLayoutReportDetailComponent } from './overall-layout-report-detail/overall-layout-report-detail.component';
import { OverallLayoutReportMainComponent } from './overall-layout-report-main/overall-layout-report-main.component';
import { C2BOverallLayoutReportRoutingModule } from './c2b-overall-layout-report-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { NgxSpinnerModule } from 'ngx-spinner';
import { AlertModule } from 'ngx-bootstrap/alert';
import { NgSelect2Module } from 'ng-select2';
import { TabsModule } from 'ngx-bootstrap/tabs';



@NgModule({
  declarations: [OverallLayoutReportDetailComponent, OverallLayoutReportMainComponent],
  imports: [
    CommonModule,
    C2BOverallLayoutReportRoutingModule,
    FormsModule,
    NgSelect2Module,
    ReactiveFormsModule,
    PaginationModule,
    NgxSpinnerModule,
    AlertModule.forRoot(),
    TabsModule.forRoot()
  ]
})
export class C2bOverallLayoutReportModule { }
