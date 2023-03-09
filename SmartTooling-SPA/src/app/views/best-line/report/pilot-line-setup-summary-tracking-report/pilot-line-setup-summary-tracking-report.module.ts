import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PilotLineSetupSummaryTrackingReportRoutingModule } from './pilot-line-setup-summary-tracking-report-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgSelect2Module } from 'ng-select2';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { NgxSpinnerModule } from 'ngx-spinner';
import { PilotLineSetupSummaryTrackingComponent } from './pilot-line-setup-summary-tracking/pilot-line-setup-summary-tracking.component';


@NgModule({
  declarations: [
    PilotLineSetupSummaryTrackingComponent
  ],
  imports: [
    CommonModule,
    PilotLineSetupSummaryTrackingReportRoutingModule,
    NgSelect2Module,
    FormsModule,
    ReactiveFormsModule,
    PaginationModule,
    NgxSpinnerModule,
  ]
})
export class PilotLineSetupSummaryTrackingReportModule { }
