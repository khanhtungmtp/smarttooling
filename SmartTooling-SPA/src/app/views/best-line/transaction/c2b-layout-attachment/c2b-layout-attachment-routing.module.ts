import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { AddComponent } from "./add/add.component";
import { MainComponent } from "./main/main.component";

const routes: Routes = [
  {
    path: "",
    data: {
      title: "C2B Layout Attachment",
    },
    children: [
      {
        path: "main",
        component: MainComponent,
        data: {
          title: "C2B Layout Attachment Main",
        },
      },
      {
        path: "add",
        component: AddComponent,
        data: {
          title: "Add new C2B Layout Attachment",
        },
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class C2BLayoutAttachmentRouting { }
