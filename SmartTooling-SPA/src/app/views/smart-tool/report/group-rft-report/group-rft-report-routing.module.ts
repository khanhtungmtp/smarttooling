import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { RftListComponent } from "./rft-list/rft-list.component";
import { RftDetailComponent } from "./rft-detail/rft-detail.component";

const routes: Routes = [
  {
    path: "",
    data: {
      title: "Group RFT Report",
    },
    children: [
      {
        path: "main",
        component: RftListComponent,
        data: {
          title: "Main",
        },
      },
      {
        path: "rft-detail",
        component: RftDetailComponent,
        data: {
          title: "Group RFT Detail",
        },
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class GroupRftReportRoutingModule {}
