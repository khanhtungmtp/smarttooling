import { BrowserModule } from "@angular/platform-browser";
import { APP_INITIALIZER, NgModule } from "@angular/core";
import { LocationStrategy, HashLocationStrategy } from "@angular/common";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { PerfectScrollbarModule } from "ngx-perfect-scrollbar";
import { AppComponent } from "./app.component";
import { DefaultLayoutComponent } from "./containers";
import { LoginComponent } from "./views/login/login.component";
import {
  AppAsideModule,
  AppBreadcrumbModule,
  AppHeaderModule,
  AppFooterModule,
  AppSidebarModule,
} from "@coreui/angular";
import { AppRoutingModule } from "./app.routing";
import { BsDropdownModule } from "ngx-bootstrap/dropdown";
import { TabsModule } from "ngx-bootstrap/tabs";
import { ChartsModule } from "ng2-charts";
import { HighchartsChartModule } from "highcharts-angular";
import { HttpClientModule, HTTP_INTERCEPTORS } from "@angular/common/http";
import { PaginationModule } from "ngx-bootstrap/pagination";
import { NgxSpinnerModule } from "ngx-spinner";
import { FormsModule } from "@angular/forms";
import { AuthService } from "./_core/_services/auth.service";
import { AuthGuard } from "./_core/_guards/auth.guard";
import { JwtModule } from "@auth0/angular-jwt";
import { ModalModule } from "ngx-bootstrap/modal";
import { ModelEditResolver } from "./_core/_resolvers/smart-tool/model-edit.resolver";
import { P404Component } from "./views/error/404.component";
import { P500Component } from "./views/error/500.component";
import { SnotifyModule, SnotifyService, ToastDefaults } from "ng-snotify";
import { UnauthorizedInterceptor } from "./_core/_helpers/unauthorized.interceptor";
import { AuditReportNavGuard } from "./_core/_guards/best-line/audit-report-nav.guard";
import { C2bLayoutByProcessReportNavGuard } from "./_core/_guards/best-line/c2b-layout-by-process-report-nav.guard";
import { C2bOverallLayoutReportNavGuard } from "./_core/_guards/best-line/c2b-overall-layout-report-nav.guard";
import { CriticalProcessReportNavGuard } from "./_core/_guards/best-line/critical-process-report-nav.guard";
import { RolloutReportNavGuard } from "./_core/_guards/best-line/rollout-report-nav.guard";
import { ComputerStitchingSettingNavGuard } from "./_core/_guards/production-bp/computer-stitching-setting-nav.guard";
import { PadPrintSettingNavGuard } from "./_core/_guards/production-bp/pad-print-setting-nav.guard";
import { BondingProgramSettingGuard } from "./_core/_guards/production-bp/bonding-program-setting.guard";
import { PadPrintSettingReportGuard } from "./_core/_guards/production-bp/pad-print-setting-report.guard";
import { CriticalProcessAnalysisGuard } from "./_core/_guards/best-line/critical-process-analysis.guard";
import { LineBalancingGuard } from "./_core/_guards/best-line/line-balancing.guard";
import { C2BLayoutByProcessGuard } from "./_core/_guards/best-line/c2b-layout-by-process.guard";
import { C2BOverallLayoutGuard } from "./_core/_guards/best-line/C2BOverallLayout.guard";
import { C2BOverallLayoutAttachmentGuard } from "./_core/_guards/best-line/C2BOverallLayoutAttachment.guard";
import { RolloutAuditGuard } from "./_core/_guards/best-line/rollout-audit.guard";
import { RolloutProgressGuard } from "./_core/_guards/best-line/rollout-progress.guard";
import { BondingProgramSettingReportGuard } from "./_core/_guards/production-bp/BondingProgramSettingReport.guard";
import { ComputerStitchingSettingReportGuard } from "./_core/_guards/production-bp/ComputerStitchingSettingReport.guard";
import { environment } from "../environments/environment";
import { UserListGuard } from "./_core/_guards/smart-tool/user-list.guard";
import { DefectReasonGuard } from "./_core/_guards/smart-tool/defect-reason.guard";
import { ModelGuard } from "./_core/_guards/smart-tool/model.guard";
import { ModelOperationGuard } from "./_core/_guards/smart-tool/model-operation.guard";
import { ModelEfficiencyGuard } from "./_core/_guards/smart-tool/model-efficiency.guard";
import { SharedResourcesService } from "./_core/_services/shared-resources.service";
import { appInitializer } from "./_core/_helpers/app.initializer";

export function tokenGetter() {
  return localStorage.getItem("tokenSmartTooling");
}
@NgModule({
  imports: [
    HttpClientModule,
    FormsModule,
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    AppAsideModule,
    AppBreadcrumbModule.forRoot(),
    AppFooterModule,
    NgxSpinnerModule,
    AppHeaderModule,
    AppSidebarModule,
    PerfectScrollbarModule,
    PaginationModule.forRoot(),
    BsDropdownModule.forRoot(),
    TabsModule.forRoot(),
    ModalModule.forRoot(),
    ChartsModule,
    HighchartsChartModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        allowedDomains: environment.allowedDomains,
        disallowedRoutes: environment.disallowedRoutes
      },
    }),
    SnotifyModule,
  ],
  declarations: [
    AppComponent,
    DefaultLayoutComponent,
    LoginComponent,
    P404Component,
    P500Component,
  ],
  providers: [
    AuthService,
    AuthGuard,
    C2BOverallLayoutGuard,
    C2BOverallLayoutAttachmentGuard,
    RolloutAuditGuard,
    RolloutProgressGuard,
    BondingProgramSettingReportGuard,
    ComputerStitchingSettingReportGuard,
    CriticalProcessAnalysisGuard,
    BondingProgramSettingGuard,
    PadPrintSettingReportGuard,
    C2BLayoutByProcessGuard,
    LineBalancingGuard,
    ModelEditResolver,
    AuditReportNavGuard,
    C2bLayoutByProcessReportNavGuard,
    C2bOverallLayoutReportNavGuard,
    CriticalProcessReportNavGuard,
    RolloutReportNavGuard,
    ComputerStitchingSettingNavGuard,
    PadPrintSettingNavGuard,
    //smart-tool-guards
    UserListGuard,
    DefectReasonGuard,
    ModelGuard,
    ModelOperationGuard,
    ModelEfficiencyGuard,
    {
      provide: LocationStrategy,
      useClass: HashLocationStrategy,
    },
    SnotifyService,
    {
      provide: 'SnotifyToastConfig',
      useValue: ToastDefaults
    },
    { provide: HTTP_INTERCEPTORS, useClass: UnauthorizedInterceptor, multi: true },
    { provide: APP_INITIALIZER, useFactory: appInitializer, multi: true, deps: [SharedResourcesService] }
  ],
  bootstrap: [AppComponent],
})
export class AppModule { }
