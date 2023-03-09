import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { DefectReasonListComponent } from "./defect-reason-list/defect-reason-list.component";
import { DefectReasonAddComponent } from "./defect-reason-add/defect-reason-add.component";
// import { DefectReasonEditComponent } from "./defect-reason-edit/defect-reason-edit.component";

const routes: Routes = [
  {
    path: "",
    data: {
      title: "Defect Reason",
    },
    children: [
      {
        path: "list",
        component: DefectReasonListComponent,
        data: {
          title: "Defect Reason",
        },
      },
      {
        path: "add",
        component: DefectReasonAddComponent,
        data: {
          title: "Add Defect Reason",
        },
      },
      {
        path: "edit",
        component: DefectReasonAddComponent,
        data: {
          title: "Edit Defect Reason",
        },
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class DefectReasonRoutingModule {}
