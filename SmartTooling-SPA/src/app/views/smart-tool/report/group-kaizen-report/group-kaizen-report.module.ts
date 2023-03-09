import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ChartsModule } from 'ng2-charts';
import { HighchartsChartModule } from 'highcharts-angular';
import { GroupKaizenReportRoutingModule } from './group-kaizen-report-routing.module';
import { KaizenGroupListComponent } from './kaizen-group-list/kaizen-group-list.component';
import { KaizenGroupDetailComponent } from './kaizen-group-detail/kaizen-group-detail.component';
import { ModelDetailComponent } from './model-detail/model-detail.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { NgxSpinnerModule } from 'ngx-spinner';
import { AlertModule } from 'ngx-bootstrap/alert';

@NgModule({
  declarations: [
    KaizenGroupListComponent,
    KaizenGroupDetailComponent,
    ModelDetailComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    PaginationModule,
    NgxSpinnerModule,
    ChartsModule,
    HighchartsChartModule,
    AlertModule.forRoot(),
    GroupKaizenReportRoutingModule
  ]
})
export class GroupKaizenReportModule { }
