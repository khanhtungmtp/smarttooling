import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: '',
    data: {
      title: 'Maintain'
    },
    children: [
      {
        path: 'model',
        loadChildren: () => import('./model/model.module').then(m => m.ModelModule)
      },
      {
        path: 'model-operation',
        loadChildren: () => import('./model-operation/model-operation.module').then(m => m.ModelOperationModule)
      },
      {
        path: 'model-efficiency',
        loadChildren: () => import('./model-efficiency/model-efficiency.module').then(m => m.ModelEfficiencyModule)
      },
      // {
      //   path: 'defect-reason',
      //   loadChildren: () => import('./defect-reason/defect-reason.module').then(m => m.DefectReasonModule)
      // }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class MaintainRoutingModule {
}
