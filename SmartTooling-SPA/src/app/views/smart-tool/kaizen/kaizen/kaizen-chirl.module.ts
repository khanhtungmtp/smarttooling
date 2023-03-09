import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgSelect2Module } from 'ng-select2';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { KaizenAddComponent } from './kaizen-add/kaizen-add.component';
import { KaizenEditComponent } from './kaizen-edit/kaizen-edit.component';
import { KaizenListComponent } from './kaizen-list/kaizen-list.component';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { KaizenChirlRoutingModule } from './kaizen-chirl-routing.module';
import { AlertModule } from 'ngx-bootstrap/alert';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    KaizenChirlRoutingModule,
    PaginationModule.forRoot(),
    NgSelect2Module,
    BsDatepickerModule.forRoot(),
    AlertModule.forRoot(),
  ],
  declarations: [KaizenListComponent, KaizenAddComponent, KaizenEditComponent],
})
export class KaizenChirlModule { }