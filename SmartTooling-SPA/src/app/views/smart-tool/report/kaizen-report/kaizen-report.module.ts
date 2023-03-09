import { NgModule, NO_ERRORS_SCHEMA } from "@angular/core";
import { CommonModule } from "@angular/common";
import { ChartsModule } from 'ng2-charts';
import { HighchartsChartModule } from 'highcharts-angular';


import { KaizenReportRoutingModule } from "./kaizen-report-routing.module";
import { KaizenListComponent } from "./kaizen-list/kaizen-list.component";
import { KaizenDeailComponent } from "./kaizen-deail/kaizen-deail.component";
import { ModelDetailComponent } from "./model-detail/model-detail.component";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { AlertModule } from 'ngx-bootstrap/alert';
import { NgxSpinnerModule } from 'ngx-spinner';

@NgModule({
  declarations: [
    KaizenListComponent,
    KaizenDeailComponent,
    ModelDetailComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    PaginationModule,
    NgxSpinnerModule,
    AlertModule.forRoot(),
    KaizenReportRoutingModule,
    ChartsModule,
    HighchartsChartModule
  ],
  schemas: [NO_ERRORS_SCHEMA],
})
export class KaizenReportModule {}
