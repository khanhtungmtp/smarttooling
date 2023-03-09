import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CriticalProcessReportMainComponent } from './critical-process-report-main/critical-process-report-main.component';
import { CriticalProcessReportDetailComponent } from './critical-process-report-detail/critical-process-report-detail.component';
import { CriticalProcessReportRoutingModule } from './critical-process-report-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgSelect2Module } from 'ng-select2';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { NgxSpinnerModule } from 'ngx-spinner';
import { AlertModule } from 'ngx-bootstrap/alert';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { KaizenDetailComponent } from './kaizen-detail/kaizen-detail.component';



@NgModule({
  declarations: [CriticalProcessReportMainComponent, CriticalProcessReportDetailComponent, KaizenDetailComponent],
  imports: [
    CommonModule,
    CriticalProcessReportRoutingModule,
    FormsModule,
    NgSelect2Module,
    ReactiveFormsModule,
    PaginationModule,
    NgxSpinnerModule,
    AlertModule.forRoot(),
    TabsModule.forRoot()
  ]
})
export class CriticalProcessReportModule { }
