import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { FormComponent } from "./form/form.component";
import { LayoutDesignOverallAddComponent } from "./layout-design-overall-add/layout-design-overall-add.component";
import { LayoutDesignOverallEditComponent } from "./layout-design-overall-edit/layout-design-overall-edit.component";
import { LayoutDesignOverallMainComponent } from "./layout-design-overall-main/layout-design-overall-main.component";

const routes: Routes = [
  {
    path: "",
    data: {
      title: "C2B Overall Layout",
    },
    children: [
      {
        path: "main",
        component: LayoutDesignOverallMainComponent,
        data: {
          title: "C2B Overall Layout Main",
        },
      },
      {
        path: "add",
        component: FormComponent,
        data: {
          title: "Add C2B Overall Layout",
        },
      },
      {
        path: "edit",
        component: FormComponent,
        data: {
          title: "Edit C2B Overall Layout",
        },
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class LayoutDesignOverallRoutingModule { }
