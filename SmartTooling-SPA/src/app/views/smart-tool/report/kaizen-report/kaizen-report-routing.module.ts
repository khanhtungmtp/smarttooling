import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { KaizenDeailComponent } from './kaizen-deail/kaizen-deail.component';
import { KaizenListComponent } from './kaizen-list/kaizen-list.component';
import { ModelDetailComponent } from './model-detail/model-detail.component';


const routes: Routes = [
  {
    path: '',
    data: {
      title: 'Kaizen Report'
    },
    children: [
      {
        path: 'main',
        component: KaizenListComponent,
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
        component: KaizenDeailComponent,
        data: {
          title: 'Kaizen Detail'
        }
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class KaizenReportRoutingModule { }
