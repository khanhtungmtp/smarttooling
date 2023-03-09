import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RolloutAddComponent } from './rollout-add/rollout-add.component';
import { RolloutListComponent } from './rollout-list/rollout-list.component';
import { RolloutViewComponent } from './rollout-view/rollout-view.component';


const routes: Routes = [
  {
    path: '',
    data: {
      title: 'Rollout-audit'
    },
    children: [
      {
        path: 'list',
        data: { title: 'Rollout Audit list' },
        component: RolloutListComponent,
      },
      {
        path: 'add',
        data: { title: 'Add New Rollout Audit' },
        component: RolloutAddComponent,
      },
      {
        path: 'view',
        data: { title: 'View Rollout Audit' },
        component: RolloutViewComponent,
      },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RolloutAuditRoutingModule { }
