import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ChartsModule } from 'ng2-charts';
import { HighchartsChartModule } from 'highcharts-angular';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { NgxSpinnerModule } from 'ngx-spinner';
import { AlertModule } from 'ngx-bootstrap/alert';
import { CrossSiteSharingListComponent } from './cross-site-sharing-list/cross-site-sharing-list.component';
import { CrossSiteSharingEditComponent } from './cross-site-sharing-edit/cross-site-sharing-edit.component';
import { CrossSiteSharingRoutingModule } from './cross-routing.module';
import { CrossSiteSharingPdfComponent } from './cross-site-sharing-pdf/cross-site-sharing-pdf.component';
import { CrossSiteSharingPdfListComponent } from './cross-site-sharing-pdf-list/cross-site-sharing-pdf-list.component';
import { NgxPrintModule } from 'ngx-print';

@NgModule({
  declarations: [
    CrossSiteSharingListComponent,
    CrossSiteSharingEditComponent,
    CrossSiteSharingPdfComponent,
    CrossSiteSharingPdfListComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    PaginationModule,
    NgxSpinnerModule,
    ChartsModule,
    HighchartsChartModule,
    NgxPrintModule,
    AlertModule.forRoot(),
    CrossSiteSharingRoutingModule,
  ]
})
export class CrossSiteSharingModule { }