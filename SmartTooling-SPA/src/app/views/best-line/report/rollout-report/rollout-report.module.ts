import { NgSelect2Module } from 'ng-select2';
import { CommonModule } from "@angular/common";
import { HttpClientModule } from "@angular/common/http";
import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { NgxSpinnerModule } from 'ngx-spinner';
import { RolloutReportComponent } from './rollout-report.component';
import { RolloutReportRoutingModule } from './rollout-report-routing.module';

@NgModule({
  imports: [
    HttpClientModule,
    CommonModule,
    RolloutReportRoutingModule,
    NgSelect2Module,
    FormsModule,
    ReactiveFormsModule,
    PaginationModule,
    NgxSpinnerModule,
  ],
  declarations: [
    RolloutReportComponent
  ]
})

export class RolloutReportModule { }
