import { Component, OnInit } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { AuthService } from './_core/_services/auth.service';
declare var jQuery: any;

@Component({
  // tslint:disable-next-line
  selector: 'body',
  template: `
    <router-outlet><ng-snotify>
    </ng-snotify></router-outlet>
    <ngx-spinner bdColor="rgba(0, 0, 0, 0.8)" size="medium" color="#fff" type="ball-clip-rotate" [fullScreen]="true">
    </ngx-spinner>`
})
export class AppComponent implements OnInit {
  jwtHelper = new JwtHelperService();
  constructor(private router: Router, private authService: AuthService) { }

  ngOnInit() {
    this.router.events.subscribe((evt) => {
      if (!(evt instanceof NavigationEnd)) {
        return;
      }
      window.scrollTo(0, 0);
    });
    const token = localStorage.getItem("tokenSmartTooling");
    if (token) {
      this.authService.decodedToken = this.jwtHelper.decodeToken(token);
    }
    (function ($) {
      $(document).ready(function () {
        $.fn.select2.defaults.set('theme', 'bootstrap');
      });
    })(jQuery);
  }
}
