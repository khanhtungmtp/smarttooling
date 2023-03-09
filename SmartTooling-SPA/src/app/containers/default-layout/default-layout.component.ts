import { Component, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { CaptionConstants, MessageConstants } from '../../_core/_constants/system.constants';
import { AuthService } from '../../_core/_services/auth.service';
import { NgSnotifyService } from '../../_core/_services/ng-snotify.service';
import { UserService } from '../../_core/_services/user.service';
import { NavItem } from '../../_nav';

@Component({
  selector: 'app-dashboard',
  templateUrl: './default-layout.component.html'
})
export class DefaultLayoutComponent {
  public sidebarMinimized = false;
  public navItems = [];
  currentUser: any = JSON.parse(localStorage.getItem('userSmartTooling'));
  currentFactory: string = localStorage.getItem('factorySmartTooling');
  oldPassword: string;
  newPassword: string;
  confirmPassword: string;
  @ViewChild('modalChangePassword', { static: false }) modalEditUser: ModalDirective;

  toggleMinimize(e) {
    this.sidebarMinimized = e;
  }

  constructor(private authService: AuthService,
    private snotifyAlert: NgSnotifyService,
    private router: Router,
    private userService: UserService,
    private spinnerService: NgxSpinnerService,
    private nav: NavItem) {
      this.navItems = this.nav.getNav(this.currentUser);
    }

  logout() {
    localStorage.removeItem('tokenSmartTooling');
    localStorage.removeItem('userSmartTooling');
    this.authService.decodedToken = null;
    this.authService.currentUser = null;
    this.snotifyAlert.success(MessageConstants.LOGGED_OUT, CaptionConstants.SUCCESS);
    this.router.navigate(['/login']);
  }

  changePassword() {
    if (this.newPassword !== this.confirmPassword) {
      this.snotifyAlert.error('Confirm password not match!', CaptionConstants.ERROR);
      return;
    }
    this.spinnerService.show();
    this.userService.changePassword(this.currentUser.username, this.oldPassword, this.newPassword)
      .subscribe(res => {
        if (res.success) {
          this.snotifyAlert.success(res.message, CaptionConstants.SUCCESS);
          this.spinnerService.hide();
          this.modalEditUser.hide();
        }
        else {
          this.snotifyAlert.error(res.message, CaptionConstants.ERROR);
          this.spinnerService.hide();
        }
      }, error => {
        this.snotifyAlert.error('Fail change pasword user!', CaptionConstants.ERROR);
        this.spinnerService.hide();
      });
  }
}
