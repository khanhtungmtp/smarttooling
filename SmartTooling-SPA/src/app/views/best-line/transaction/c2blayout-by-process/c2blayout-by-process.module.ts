import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgSelect2Module } from 'ng-select2';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { AlertModule } from 'ngx-bootstrap/alert';
import { C2blayoutByProcessRoutingModule } from './c2blayout-by-process-routing.module';
import { ProcessEditComponent } from './process-edit/process-edit.component';
import { ProcessAddComponent } from './process-add/process-add.component';
import { ProcessMainComponent } from './process-main/process-main.component';
import { CommonsModule } from '../../../commons/commons.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    PaginationModule.forRoot(),
    NgSelect2Module,
    BsDatepickerModule.forRoot(),
    AlertModule.forRoot(),
    C2blayoutByProcessRoutingModule,
    CommonsModule
  ],
  declarations: [ ProcessMainComponent, ProcessAddComponent, ProcessEditComponent],
})
export class C2blayoutByProcessModule { }