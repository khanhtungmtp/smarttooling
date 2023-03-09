import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserComponent } from './user/user.component';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { UserRoutingModule } from './user-routing.module';
import { ModalModule } from 'ngx-bootstrap/modal';
import { FormsModule } from '@angular/forms';
import { NgxSpinnerModule } from 'ngx-spinner';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    NgxSpinnerModule,
    PaginationModule.forRoot(),
    UserRoutingModule,
    ModalModule.forRoot()
  ],
  declarations: [
    UserComponent
  ]
})
export class UserModule { }
