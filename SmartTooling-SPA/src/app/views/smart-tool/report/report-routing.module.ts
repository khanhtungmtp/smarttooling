import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from '../../../_core/_guards/auth.guard';
import { CrossSiteSharingGuard } from '../../../_core/_guards/smart-tool/cross-site-sharing.guard';
import { GroupKaizenReportGuard } from '../../../_core/_guards/smart-tool/group-kaizen-report.guard';
import { GroupRFTReportGuard } from '../../../_core/_guards/smart-tool/group-rft-report.guard';
import { KaizenReportGuard } from '../../../_core/_guards/smart-tool/kaizen-report.guard';
import { RFTReportGuard } from '../../../_core/_guards/smart-tool/rft-report.guard';


const routes: Routes = [
  {
    path: '',
    data: {
      title: 'Report'
    },
    children: [
      {
        canLoad: [KaizenReportGuard],
        path: 'kaizen-report',
        loadChildren: () => import('./kaizen-report/kaizen-report.module').then(m => m.KaizenReportModule)
      },
      {
        canLoad: [GroupKaizenReportGuard],
        path: 'group-kaizen-report',
        loadChildren: () => import('./group-kaizen-report/group-kaizen-report.module').then(m => m.GroupKaizenReportModule)
      },
      {
        canLoad: [RFTReportGuard],
        path: 'rft-report',
        loadChildren: () => import('./rft-report/rft-report.module').then(m => m.RftReportModule)
      },
      {
        canLoad: [GroupRFTReportGuard],
        path: 'group-rft-report',
        loadChildren: () => import('./group-rft-report/group-rft-report.module').then(m => m.GroupRftReportModule)
      },
      {
        canLoad: [CrossSiteSharingGuard],
        path: 'cross-site-sharing',
        loadChildren: () => import('./cross-site-sharing/cross.module').then(m => m.CrossSiteSharingModule)
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ReportRoutingModule { }
