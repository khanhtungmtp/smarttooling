import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { C2bLayoutByProcessReportMainComponent } from './c2b-layout-by-process-report-main/c2b-layout-by-process-report-main.component';
import { C2bLayoutByProcessReportDetailComponent } from './c2b-layout-by-process-report-detail/c2b-layout-by-process-report-detail.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgSelect2Module } from 'ng-select2';
import { AlertModule } from 'ngx-bootstrap/alert';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { NgxSpinnerModule } from 'ngx-spinner';
import { C2BLayoutByProcessReportRoutingModule } from './c2b-layout-by-process-report-routing.module';
import { ChartsModule } from 'ng2-charts';
import { ButtonsModule } from 'ngx-bootstrap/buttons';



@NgModule({
  declarations: [C2bLayoutByProcessReportMainComponent, C2bLayoutByProcessReportDetailComponent],
  imports: [
    CommonModule,
    C2BLayoutByProcessReportRoutingModule,
    FormsModule,
    NgSelect2Module,
    ReactiveFormsModule,
    PaginationModule,
    NgxSpinnerModule,
    AlertModule.forRoot(),
    ChartsModule,
  ]
})
export class C2bLayoutByProcessReportModule { }
