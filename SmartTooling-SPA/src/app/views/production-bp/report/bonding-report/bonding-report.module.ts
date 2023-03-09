import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { BondingReportRoutingModule } from './bonding-report-routing.module';
import { BondingReportMainComponent } from './bonding-report-main/bonding-report-main.component';
import { BondingReportDetailComponent } from './bonding-report-detail/bonding-report-detail.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgSelect2Module } from 'ng-select2';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { NgxSpinnerModule } from 'ngx-spinner';


@NgModule({
  declarations: [BondingReportMainComponent, BondingReportDetailComponent],
  imports: [
    CommonModule,
    BondingReportRoutingModule,
    FormsModule,
    NgSelect2Module,
    ReactiveFormsModule,
    PaginationModule,
    NgxSpinnerModule,
  ]
})
export class BondingReportModule { }
