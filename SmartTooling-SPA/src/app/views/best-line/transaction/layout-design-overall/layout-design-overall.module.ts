import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { NgSelect2Module } from "ng-select2";
import { PaginationModule } from "ngx-bootstrap/pagination";
import { NgxSpinnerModule } from "ngx-spinner";
import { CommonsModule } from "../../../commons/commons.module";
import { LayoutDesignOverallAddComponent } from "./layout-design-overall-add/layout-design-overall-add.component";
import { LayoutDesignOverallEditComponent } from "./layout-design-overall-edit/layout-design-overall-edit.component";
import { LayoutDesignOverallMainComponent } from "./layout-design-overall-main/layout-design-overall-main.component";
import { LayoutDesignOverallRoutingModule } from "./layout-design-overall-routing.module";

@NgModule({
  declarations: [
    LayoutDesignOverallMainComponent,
    LayoutDesignOverallAddComponent,
    LayoutDesignOverallEditComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    LayoutDesignOverallRoutingModule,
    ReactiveFormsModule,
    NgxSpinnerModule,
    PaginationModule,
    NgSelect2Module,
    CommonsModule
  ],
})
export class LayoutDesignOverallModule { }
