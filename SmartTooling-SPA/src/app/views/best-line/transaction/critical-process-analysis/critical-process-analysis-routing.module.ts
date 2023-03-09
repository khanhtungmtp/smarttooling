import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CriticalProcessAnalysisResolver } from '../../../../_core/_resolvers/best-line/critical-process-analysis.resolver';
import { CriticalProcessAnalysisAddComponent } from './critical-process-analysis-add/critical-process-analysis-add.component';
import { CriticalProcessAnalysisEditComponent } from './critical-process-analysis-edit/critical-process-analysis-edit.component';
import { CriticalProcessAnalysisMainComponent } from './critical-process-analysis-main/critical-process-analysis-main.component';


const routes: Routes = [
  {


    path: "",
    data: {
      title: "Critical Process Analysis",
    },
    children: [
      {
        path: "main",
        resolve: { criticalProcessAnalysis: CriticalProcessAnalysisResolver },
        component: CriticalProcessAnalysisMainComponent,
        data: {
          title: "List Critical Process Analysis",
        },
      },
      {
        path: "add",
        component: CriticalProcessAnalysisAddComponent,
        data: {
          title: "Add new Critical Process Analysis",
        },
      },
      {
        path: "edit",
        component: CriticalProcessAnalysisEditComponent,
        //resolve: { model: ModelEditResolver },
        data: {
          title: "Edit Critical Process Analysis",
        },
      },
    ],

    // path: 'critical-process-analysis-main',
    // //resolve: { listLineNo: RolloutProgress },
    // // canActivate: [],
    // data: { title: 'Critical Proces sAnalysis' },
    // component: CriticalProcessAnalysisMainComponent,
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CriticalProcessAnalysisRoutingModule { }
