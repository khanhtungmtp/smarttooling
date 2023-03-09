import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { ModelEditResolver } from "../../../../_core/_resolvers/smart-tool/model-edit.resolver";
import { AddComponent } from "./add/add.component";
import { EditComponent } from "./edit/edit.component";
import { ModelListComponent } from "./model-list/model-list.component";

const routes: Routes = [
  {
    path: "",
    data: {
      title: "Model",
    },
    children: [
      {
        path: "list",
        component: ModelListComponent,
        data: {
          title: "List Model",
        },
      },
      {
        path: "add",
        component: AddComponent,
        data: {
          title: "Add new Model",
        },
      },
      {
        path: "edit/:modelNo",
        component: EditComponent,
        resolve: { model: ModelEditResolver },
        data: {
          title: "Edit Model",
        },
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ModelRoutingModule { }
