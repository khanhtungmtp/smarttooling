import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ComputerReportRoutingModule } from './computer-report-routing.module';
import { ComputerReportMainComponent } from './computer-report-main/computer-report-main.component';
import { ComputerReportDetailComponent } from './computer-report-detail/computer-report-detail.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgSelect2Module } from 'ng-select2';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { NgxSpinnerModule } from 'ngx-spinner';


@NgModule({
  declarations: [ComputerReportMainComponent, ComputerReportDetailComponent],
  imports: [
    CommonModule,
    ComputerReportRoutingModule,
    FormsModule,
    NgSelect2Module,
    ReactiveFormsModule,
    PaginationModule,
    NgxSpinnerModule,
  ]
})
export class ComputerReportModule { }
