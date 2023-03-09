import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgSelect2Module } from 'ng-select2';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { AlertModule } from 'ngx-bootstrap/alert';
import { LineBalancingRoutingModule } from './line-balancing-routing.module';
import { LineBalancingMainComponent } from './line-balancing-main/line-balancing-main.component';
import { LineBalancingInputComponent } from './line-balancing-input/line-balancing-input.component';
import { ChartsModule } from 'ng2-charts';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    PaginationModule.forRoot(),
    NgSelect2Module,
    AlertModule.forRoot(),
    LineBalancingRoutingModule,
    ChartsModule
  ],
  declarations: [LineBalancingMainComponent, LineBalancingInputComponent],
})
export class LineBalancingModule { }