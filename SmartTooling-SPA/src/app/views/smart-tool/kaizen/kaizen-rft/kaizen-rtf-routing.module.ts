import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { KaizenRftEditComponent } from './kaizen-rft-edit/kaizen-rft-edit.component';
import { KaizenRftListComponent } from './kaizen-rft-list/kaizen-rft-list.component';
const routes: Routes = [
  {
    path: '',
    data: { title: 'kaizen-rft-list' },
    component: KaizenRftListComponent
  },
  {
    path: 'kaizen-rft-edit/:modelNo/:serial_no',
    data: { title: 'Edit Kaizen rft' },
    component: KaizenRftEditComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class KaizenRFTRoutingModule { }