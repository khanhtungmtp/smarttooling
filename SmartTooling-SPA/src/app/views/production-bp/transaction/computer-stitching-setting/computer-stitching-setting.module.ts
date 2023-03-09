import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ComputerStitchingSettingMainComponent } from './computer-stitching-setting-main/computer-stitching-setting-main.component';
import { ComputerStitchingSettingAddComponent } from './computer-stitching-setting-add/computer-stitching-setting-add.component';
import { ComputerStitchingSettingEditComponent } from './computer-stitching-setting-edit/computer-stitching-setting-edit.component';
import { HttpClientModule } from '@angular/common/http';
import { ComputerStitchingSettingRoutingModule } from './computer-stitching-setting-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgSelect2Module } from 'ng-select2';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { NgxSpinnerModule } from 'ngx-spinner';
import { CommonsModule } from '../../../commons/commons.module';

@NgModule({
  imports: [
    HttpClientModule,
    CommonModule,
    CommonsModule,
    ComputerStitchingSettingRoutingModule,
    NgSelect2Module,
    FormsModule,
    ReactiveFormsModule,
    PaginationModule,
    NgxSpinnerModule,
  ],
  declarations: [
    ComputerStitchingSettingMainComponent,
    ComputerStitchingSettingAddComponent,
    ComputerStitchingSettingEditComponent
  ]
})
export class ComputerStitchingSettingModule { }
