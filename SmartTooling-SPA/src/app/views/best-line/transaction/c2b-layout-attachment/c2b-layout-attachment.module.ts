import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { NgSelect2Module } from "ng-select2";
import { AlertModule } from "ngx-bootstrap/alert";
import { PaginationModule } from "ngx-bootstrap/pagination";
import { NgxSpinnerModule } from "ngx-spinner";
import { AddComponent } from "./add/add.component";
import { C2BLayoutAttachmentRouting } from "./c2b-layout-attachment-routing.module";
import { MainComponent } from "./main/main.component";

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    NgxSpinnerModule,
    AlertModule.forRoot(),
    C2BLayoutAttachmentRouting,
    PaginationModule,
    NgSelect2Module,
  ],
  declarations: [MainComponent, AddComponent],
})
export class C2BLayoutAttachmentModule {}
