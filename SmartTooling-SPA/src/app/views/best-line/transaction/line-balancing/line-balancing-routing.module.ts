import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LineBalancingInputComponent } from './line-balancing-input/line-balancing-input.component';
import { LineBalancingMainComponent } from './line-balancing-main/line-balancing-main.component';
const routes: Routes = [
  {
    path: '',
    data: { title: 'Line Balancing Main' },
    component: LineBalancingMainComponent
  },
  {
    path: 'input',
    data: { title: 'Line Balancing Input' },
    component: LineBalancingInputComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LineBalancingRoutingModule { }