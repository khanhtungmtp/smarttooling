import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ComputerStitchingSettingAddComponent } from './computer-stitching-setting-add/computer-stitching-setting-add.component';
import { ComputerStitchingSettingEditComponent } from './computer-stitching-setting-edit/computer-stitching-setting-edit.component';
import { ComputerStitchingSettingMainComponent } from './computer-stitching-setting-main/computer-stitching-setting-main.component';

const routes: Routes = [
  {
    path: "",
    data: {
      title: "Computer Stitching Setting"
    },
    children: [
      {
        path: "",
        component: ComputerStitchingSettingMainComponent,
        data: {
          title: "Computer Stitching Setting Main"
        }
      },
      {
        path: "computer-stitching-setting-add",
        // resolve: {auditDetail: AuditReportDetailResolver},
        component: ComputerStitchingSettingAddComponent,
        data: {
          title: "Computer Stitching Setting Add"
        }
      },
      {
        path: "computer-stitching-setting-edit",
        // resolve: {auditDetail: AuditReportDetailResolver},
        component: ComputerStitchingSettingEditComponent,
        data: {
          title: "Computer Stitching Setting Edit"
        }
      }
    ]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})

export class ComputerStitchingSettingRoutingModule {};
