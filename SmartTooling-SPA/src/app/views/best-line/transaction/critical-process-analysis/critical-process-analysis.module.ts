import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CriticalProcessAnalysisAddComponent } from './critical-process-analysis-add/critical-process-analysis-add.component';
import { CriticalProcessAnalysisEditComponent } from './critical-process-analysis-edit/critical-process-analysis-edit.component';
import { CriticalProcessAnalysisMainComponent } from './critical-process-analysis-main/critical-process-analysis-main.component';
import { CriticalProcessAnalysisRoutingModule } from './critical-process-analysis-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonsModule } from '../../../commons/commons.module';
import { NgSelect2Module } from 'ng-select2';
import { CriticalProcessAnalysisResolver } from '../../../../_core/_resolvers/best-line/critical-process-analysis.resolver';
import { PaginationModule } from 'ngx-bootstrap/pagination';

@NgModule({
  imports: [
    CommonModule,
    CriticalProcessAnalysisRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    CommonsModule,
    NgSelect2Module,
    PaginationModule.forRoot(),
  ],
  declarations: [
    CriticalProcessAnalysisMainComponent,
    CriticalProcessAnalysisAddComponent,
    CriticalProcessAnalysisEditComponent
  ],
  providers: [
    CriticalProcessAnalysisResolver
  ]
})
export class CriticalProcessAnalysisModule { }
