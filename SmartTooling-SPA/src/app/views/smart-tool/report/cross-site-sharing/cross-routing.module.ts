import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CrossSiteSharingEditComponent } from './cross-site-sharing-edit/cross-site-sharing-edit.component';
import { CrossSiteSharingListComponent } from './cross-site-sharing-list/cross-site-sharing-list.component';
import { CrossSiteSharingPdfListComponent } from './cross-site-sharing-pdf-list/cross-site-sharing-pdf-list.component';
import { CrossSiteSharingPdfComponent } from './cross-site-sharing-pdf/cross-site-sharing-pdf.component';


const routes: Routes = [
  {
    path: '',
    data: {
      title: 'Cross Site Sharing'
    },
    children: [
      {
        path: 'main',
        component: CrossSiteSharingListComponent,
        data: {
          title: 'Main'
        }
      },
      {
        path: 'Cross-detail/:factory/:modelNo/:serial_no',
        component: CrossSiteSharingEditComponent,
        data: {
          title: 'Cross Detail'
        }
      },
      {
        path: 'Cross-pdf',
        component: CrossSiteSharingPdfListComponent,
        data: {
          title: 'Cross PDF'
        }
      },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CrossSiteSharingRoutingModule { }
