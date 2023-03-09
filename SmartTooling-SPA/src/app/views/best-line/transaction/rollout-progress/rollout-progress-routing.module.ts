import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RolloutAddComponent } from './rollout-add/rollout-add.component';
import { RolloutEditComponent } from './rollout-edit/rollout-edit.component';
import { RolloutListComponent } from './rollout-list/rollout-list.component';


const routes: Routes = [
  {
    path: '',
    data: {
      title: 'Rollout-Progress'
    },
    children: [
      {
        path: 'list',
        data: { title: 'Rollout Progress list' },
        component: RolloutListComponent,
      },
      {
        path: 'add',
        data: { title: 'Add New Rollout Progress' },
        component: RolloutAddComponent,
      },
      {
        path: 'edit',
        data: { title: 'Edit Rollout Progress' },
        component: RolloutEditComponent,
      },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RolloutProgressRoutingModule { }
