import { BsDatepickerModule, DatepickerModule } from 'ngx-bootstrap/datepicker';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BondingProgramSettingMainComponent } from './bonding-program-setting-main/bonding-program-setting-main.component';
import { BoundingProgramSettingRoutingModule } from './bonding-program-setting-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgxSpinnerModule } from 'ngx-spinner';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { NgSelect2Module } from 'ng-select2';
import { BondingProgramSettingService } from '../../../../_core/_services/production-bp/bonding-program-setting.service';
import { BondingProgramSettingEditComponent } from './bonding-program-setting-edit/bonding-program-setting-edit.component';
import { BondingProgramSettingAddComponent } from './bonding-program-setting-add/bonding-program-setting-add.component';


@NgModule({
  declarations: [
    BondingProgramSettingMainComponent,
    BondingProgramSettingEditComponent,
    BondingProgramSettingAddComponent
  ],
  imports: [
    CommonModule,
    BoundingProgramSettingRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    NgxSpinnerModule,
    PaginationModule,
    NgSelect2Module,
    BsDatepickerModule.forRoot()
  ],
  providers: [
    BondingProgramSettingService
  ]
})
export class BondingProgramSettingModule { }
