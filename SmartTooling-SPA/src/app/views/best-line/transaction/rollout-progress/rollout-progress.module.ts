import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { RolloutProgressRoutingModule } from './rollout-progress-routing.module';
import { RolloutListComponent } from './rollout-list/rollout-list.component';
import { RolloutAddComponent } from './rollout-add/rollout-add.component';
import { RolloutEditComponent } from './rollout-edit/rollout-edit.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgSelect2Module } from 'ng-select2';
import { NgxSpinnerModule } from 'ngx-spinner';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { CommonsModule } from '../../../commons/commons.module';


@NgModule({
  declarations: [RolloutListComponent, RolloutAddComponent, RolloutEditComponent],
  imports: [
    CommonModule,
    RolloutProgressRoutingModule,
    FormsModule,
    NgSelect2Module,
    NgxSpinnerModule,
    PaginationModule.forRoot(),
    ReactiveFormsModule,
    BsDatepickerModule.forRoot(),
    CommonsModule
  ]
})
export class RolloutProgressModule { }
