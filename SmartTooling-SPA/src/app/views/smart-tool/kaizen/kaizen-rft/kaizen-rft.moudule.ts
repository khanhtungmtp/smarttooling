import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgSelect2Module } from 'ng-select2';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { KaizenRftListComponent } from './kaizen-rft-list/kaizen-rft-list.component';
import { KaizenRftEditComponent } from './kaizen-rft-edit/kaizen-rft-edit.component';
import { KaizenRFTRoutingModule } from './kaizen-rtf-routing.module';
import { AlertModule } from 'ngx-bootstrap/alert';


@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    KaizenRFTRoutingModule,
    PaginationModule.forRoot(),
    NgSelect2Module,
    BsDatepickerModule.forRoot(),
    AlertModule.forRoot(),
  ],
  declarations: [KaizenRftListComponent, KaizenRftEditComponent],
})
export class KaizenRFTModule { }