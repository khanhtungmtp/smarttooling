import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { NgxSpinnerService } from "ngx-spinner";
import { CaptionConstants, MessageConstants } from "../../_core/_constants/system.constants";
import { AuthService } from "../../_core/_services/auth.service";
import { NgSnotifyService } from "../../_core/_services/ng-snotify.service";

@Component({
  selector: "app-dashboard",
  templateUrl: "login.component.html",
  styleUrls: ["./login.component.scss"],
})
export class LoginComponent implements OnInit {
  user: any = {};

  constructor(
    private authService: AuthService,
    private router: Router,
    private spinner: NgxSpinnerService,
    private snotifyAlert: NgSnotifyService
  ) {}

  ngOnInit() {
    if (this.authService.loggedIn) this.router.navigate(["/dashboard"]);
  }

  login() {
    console.log(this.user);
    this.spinner.show();
    this.authService.login(this.user).subscribe(
      (next) => {
        this.snotifyAlert.success(MessageConstants.LOGGED_IN, CaptionConstants.SUCCESS);
        this.spinner.hide();
      },
      (error) => {
        console.log(error);
        this.snotifyAlert.error(MessageConstants.LOGIN_FAILED, CaptionConstants.ERROR);
        this.spinner.hide();
      },
      () => {
        this.router.navigate(["/dashboard"]);
        this.spinner.hide();
      }
    );
  }
}
