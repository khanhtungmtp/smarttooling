import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { DefectReasonModule } from './defect-reason/defect-reason.module';
import { MaintainRoutingModule } from './maintain-routing.module';
import { ModelEfficiencyModule } from './model-efficiency/model-efficiency.module';
import { ModelOperationModule } from './model-operation/model-operation.module';
import { ModelRoutingModule } from './model/model-routing.module';
import { ModelModule } from './model/model.module';

@NgModule({
  imports: [
      DefectReasonModule,
      ModelModule,
      ModelOperationModule,
      ModelEfficiencyModule
  ],
  declarations: [

  ]
})

export class MaintainModule {
}
