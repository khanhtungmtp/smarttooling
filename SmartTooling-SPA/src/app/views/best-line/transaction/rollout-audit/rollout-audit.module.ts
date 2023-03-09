import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { RolloutAuditRoutingModule } from './rollout-audit-routing.module';
import { RolloutListComponent } from './rollout-list/rollout-list.component';
import { RolloutAddComponent } from './rollout-add/rollout-add.component';
import { RolloutViewComponent } from './rollout-view/rollout-view.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgSelect2Module } from 'ng-select2';
import { NgxSpinnerModule } from 'ngx-spinner';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';


@NgModule({
  declarations: [RolloutListComponent, RolloutAddComponent, RolloutViewComponent],
  imports: [
    CommonModule,
    RolloutAuditRoutingModule,
    FormsModule,
    NgSelect2Module,
    NgxSpinnerModule,
    PaginationModule.forRoot(),
    ReactiveFormsModule,
    BsDatepickerModule.forRoot(),
  ]
})
export class RolloutAuditModule { }
