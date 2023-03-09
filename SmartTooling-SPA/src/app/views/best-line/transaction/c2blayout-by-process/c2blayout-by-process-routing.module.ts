import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProcessAddComponent } from './process-add/process-add.component';
import { ProcessEditComponent } from './process-edit/process-edit.component';
import { ProcessMainComponent } from './process-main/process-main.component';
const routes: Routes = [
  {
    path: '',
    data: { title: 'Process Main' },
    component: ProcessMainComponent
  },
  {
    path: 'add',
    data: { title: 'Add Process' },
    component: ProcessAddComponent
  },
  {
    path: 'edit',
    data: { title: 'Edit Process' },
    component: ProcessEditComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class C2blayoutByProcessRoutingModule { }