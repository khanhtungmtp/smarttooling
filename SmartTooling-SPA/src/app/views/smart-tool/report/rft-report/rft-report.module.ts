import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChartsModule } from 'ng2-charts';
import { HighchartsChartModule } from 'highcharts-angular';

import { RftReportRoutingModule } from './rft-report-routing.module';
import { RftListComponent } from './rft-list/rft-list.component';
import { RftDetailComponent } from './rft-detail/rft-detail.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { AlertModule } from 'ngx-bootstrap/alert';
import { NgxSpinnerModule } from 'ngx-spinner';


@NgModule({
  declarations: [RftListComponent, RftDetailComponent],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    PaginationModule,
    NgxSpinnerModule,
    AlertModule.forRoot(),
    RftReportRoutingModule,
    ChartsModule,
    HighchartsChartModule
  ]
})
export class RftReportModule { }
