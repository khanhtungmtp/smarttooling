import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { KaizenRFTGuard } from '../../../_core/_guards/smart-tool/kaizen-rft.guard';
import { KaizenGuard } from '../../../_core/_guards/smart-tool/kaizen.guard';


const routes: Routes = [
  {
    path: '',
    data: {
      title: 'Kaizen'
    },
    children: [
      {
        canLoad: [KaizenGuard],
        path: 'kaizen',
        loadChildren: () => import('./kaizen/kaizen-chirl.module').then(m => m.KaizenChirlModule)
      },
      {
        canLoad: [KaizenRFTGuard],
        path: 'kaizen-rft',
        loadChildren: () => import('./kaizen-rft/kaizen-rft.moudule').then(m => m.KaizenRFTModule)
      },

    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class KaizenRoutingModule { }
