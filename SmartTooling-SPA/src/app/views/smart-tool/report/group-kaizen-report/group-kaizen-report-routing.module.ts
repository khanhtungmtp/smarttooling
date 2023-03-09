import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { KaizenGroupDetailComponent } from './kaizen-group-detail/kaizen-group-detail.component';
import { KaizenGroupListComponent } from './kaizen-group-list/kaizen-group-list.component';
import { ModelDetailComponent } from './model-detail/model-detail.component';


const routes: Routes = [
  {
    path: '',
    data: {
      title: 'Group Kaizen Report'
    },
    children: [
      {
        path: 'main',
        component: KaizenGroupListComponent,
        data: {
          title: 'Main'
        }
      },
      {
        path: 'model-detail',
        component: ModelDetailComponent,
        data: {
          title: 'Model Detail'
        }
      },
      {
        path: 'kaizen-detail',
        component: KaizenGroupDetailComponent,
        data: {
          title: 'Group Kaizen Detail'
        }
      },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GroupKaizenReportRoutingModule { }
