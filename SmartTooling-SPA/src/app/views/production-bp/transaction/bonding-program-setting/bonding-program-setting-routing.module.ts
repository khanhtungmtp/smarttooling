import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { BondingProgramSettingAddComponent } from "./bonding-program-setting-add/bonding-program-setting-add.component";
import { BondingProgramSettingEditComponent } from "./bonding-program-setting-edit/bonding-program-setting-edit.component";
import { BondingProgramSettingMainComponent } from "./bonding-program-setting-main/bonding-program-setting-main.component";

const routes: Routes = [
  {
    path: "",
    data: {
      title: "Bonding Program Setting",
    },
    children: [
      {
        path: "main",
        component: BondingProgramSettingMainComponent,
        data: {
          title: "Bonding Program Setting Main",
        },
      },
      {
        path: "add",
        component: BondingProgramSettingAddComponent,
        data: {
          title: "Add Bonding Program Setting",
        },
      },
      {
        path: "edit",
        component: BondingProgramSettingEditComponent,
        data: {
          title: "Edit Bonding Program Setting",
        },
      }
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class BoundingProgramSettingRoutingModule { }
