import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PadPrintSettingMainComponent } from './pad-print-setting-main/pad-print-setting-main.component';
import { PadPrintSettingAddComponent } from './pad-print-setting-add/pad-print-setting-add.component';
import { PadPrintSettingEditComponent } from './pad-print-setting-edit/pad-print-setting-edit.component';
import { PadPrintSettingRoutingModule } from './pad-print-setting-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgSelect2Module } from 'ng-select2';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { CommonsModule } from '../../../commons/commons.module';
import { DirectiveModule } from '../../../../_core/_directive/directive.module';
import { NgxSpinnerModule } from 'ngx-spinner';



@NgModule({
  declarations: [PadPrintSettingMainComponent, PadPrintSettingAddComponent, PadPrintSettingEditComponent],
  imports: [
    CommonModule,
    DirectiveModule,
    PadPrintSettingRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    CommonsModule,
    NgSelect2Module,
    PaginationModule.forRoot(),
    NgxSpinnerModule
  ],
})
export class PadPrintSettingModule { }
