import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ModelEfficiencyEditComponent } from './model-efficiency-edit/model-efficiency-edit.component';

const routes: Routes = [
    {
      path: '',
      data: {
        title: 'Model-Efficiency'
      },
      children: [
          {
            path: 'edit',
            component: ModelEfficiencyEditComponent,
            data: {
              title: 'Edit Model - Efficiency'
            }
          },
      ]
    }
  ];

  @NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
  })

export class ModelEfficiencyRoutingModule {
}