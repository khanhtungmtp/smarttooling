import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
// Import Containers
import { DefaultLayoutComponent } from "./containers";
import { P404Component } from "./views/error/404.component";
import { P500Component } from "./views/error/500.component";
import { LoginComponent } from "./views/login/login.component";
import { AuthGuard } from "./_core/_guards/auth.guard";
import { AuditReportNavGuard } from "./_core/_guards/best-line/audit-report-nav.guard";
import { C2bLayoutByProcessReportNavGuard } from "./_core/_guards/best-line/c2b-layout-by-process-report-nav.guard";
import { C2bOverallLayoutReportNavGuard } from "./_core/_guards/best-line/c2b-overall-layout-report-nav.guard";
import { CriticalProcessReportNavGuard } from "./_core/_guards/best-line/critical-process-report-nav.guard";
import { RolloutReportNavGuard } from "./_core/_guards/best-line/rollout-report-nav.guard";
import { ComputerStitchingSettingNavGuard } from "./_core/_guards/production-bp/computer-stitching-setting-nav.guard";
import { PadPrintSettingNavGuard } from "./_core/_guards/production-bp/pad-print-setting-nav.guard";
import { CriticalProcessAnalysisGuard } from "./_core/_guards/best-line/critical-process-analysis.guard";
import { BondingProgramSettingGuard } from "./_core/_guards/production-bp/bonding-program-setting.guard";
import { PadPrintSettingReportGuard } from "./_core/_guards/production-bp/pad-print-setting-report.guard";
import { C2BLayoutByProcessGuard } from "./_core/_guards/best-line/c2b-layout-by-process.guard";
import { LineBalancingGuard } from "./_core/_guards/best-line/line-balancing.guard";
import { C2BOverallLayoutGuard } from "./_core/_guards/best-line/C2BOverallLayout.guard";
import { C2BOverallLayoutAttachmentGuard } from "./_core/_guards/best-line/C2BOverallLayoutAttachment.guard";
import { RolloutProgressGuard } from "./_core/_guards/best-line/rollout-progress.guard";
import { RolloutAuditGuard } from "./_core/_guards/best-line/rollout-audit.guard";
import { BondingProgramSettingReportGuard } from "./_core/_guards/production-bp/BondingProgramSettingReport.guard";
import { ComputerStitchingSettingReportGuard } from "./_core/_guards/production-bp/ComputerStitchingSettingReport.guard";
import { DefectReasonGuard } from "./_core/_guards/smart-tool/defect-reason.guard";
import { UserListGuard } from "./_core/_guards/smart-tool/user-list.guard";
import { ModelGuard } from "./_core/_guards/smart-tool/model.guard";
import { ModelOperationGuard } from "./_core/_guards/smart-tool/model-operation.guard";
import { ModelEfficiencyGuard } from "./_core/_guards/smart-tool/model-efficiency.guard";
import { PilotLineSetupSummaryTrackingGuard } from "./_core/_guards/best-line/pilot-line-setup-summary-tracking.guard";

export const routes: Routes = [
  { path: "",
    redirectTo: "/dashboard",
    pathMatch: "full" },
  {
    path: "404",
    component: P404Component,
    data: {
      title: "Page 404",
    },
  },
  {
    path: "500",
    component: P500Component,
    data: {
      title: "Page 500",
    },
  },
  {
    path: "login",
    component: LoginComponent,
    data: {
      title: "Login Page",
    },
  },
  {
    path: "",
    canActivate: [AuthGuard],
    component: DefaultLayoutComponent,
    data: {
      title: "Home",
    },
    children: [
      {
        path: "dashboard",
        runGuardsAndResolvers: "always",
        loadChildren: () =>
          import("./views/smart-tool/dashboard/dashboard.module").then(
            (m) => m.DashboardModule
          ),
      },
      {
        canLoad: [DefectReasonGuard],
        path: "defect-reason",
        loadChildren: () =>
          import(
            "./views/smart-tool/maintain/defect-reason/defect-reason.module"
          ).then((m) => m.DefectReasonModule),
      },
      {
        path: "report",
        loadChildren: () =>
          import("./views/smart-tool/report/report.module").then(
            (m) => m.ReportModule
          ),
      },
      {
        canLoad: [UserListGuard],
        path: "user",
        loadChildren: () =>
          import("./views/smart-tool/user/user.module").then(
            (m) => m.UserModule
          ),
      },
      {
        path: "measurement",
        loadChildren: () =>
          import("./views/smart-tool/measurement/measurement.module").then(
            (m) => m.MeasurementModule
          ),
      },
      {
        canLoad: [ModelGuard],
        path: "model",
        loadChildren: () =>
          import("./views/smart-tool/maintain/model/model.module").then(
            (m) => m.ModelModule
          ),
      },
      {
        canLoad: [ModelOperationGuard],
        path: "model-operation",
        loadChildren: () =>
          import(
            "./views/smart-tool/maintain/model-operation/model-operation.module"
          ).then((m) => m.ModelOperationModule),
      },
      {
        canLoad: [ModelEfficiencyGuard],
        path: "model-efficiency",
        loadChildren: () =>
          import(
            "./views/smart-tool/maintain/model-efficiency/model-efficiency.module"
          ).then((m) => m.ModelEfficiencyModule),
      },
      {
        path: "kaizen",
        loadChildren: () =>
          import("./views/smart-tool/kaizen/kaizen.module").then(
            (m) => m.KaizenModule
          ),
      },

      // ====================================best-line====================================
      {
        path: "best-line/transaction/layout-design-overall",
        canLoad: [C2BOverallLayoutGuard],
        loadChildren: () =>
          import(
            "./views/best-line/transaction/layout-design-overall/layout-design-overall.module"
          ).then((m) => m.LayoutDesignOverallModule),
      },
      {
        path: "best-line/transaction/layout-attachment",
        canLoad: [C2BOverallLayoutAttachmentGuard],
        loadChildren: () =>
          import(
            "./views/best-line/transaction/c2b-layout-attachment/c2b-layout-attachment.module"
          ).then((m) => m.C2BLayoutAttachmentModule),
      },
      {
        path: 'best-line/report/overall-layout-report',
        canLoad: [C2bOverallLayoutReportNavGuard],
        loadChildren: () => import('./views/best-line/report/c2b-overall-layout-report/c2b-overall-layout-report.module').then(m => m.C2bOverallLayoutReportModule)
      },
      {
        path: 'best-line/report/layout-by-process-report',
        canLoad: [C2bLayoutByProcessReportNavGuard],
        loadChildren: () => import('./views/best-line/report/c2b-layout-by-process-report/c2b-layout-by-process-report.module').then(m => m.C2bLayoutByProcessReportModule)
      },
      {
        path: 'best-line/report/critical-process-report',
        canLoad: [CriticalProcessReportNavGuard],
        loadChildren: () => import('./views/best-line/report/critical-process-report/critical-process-report.module').then(m => m.CriticalProcessReportModule)
      },
      {
        path: 'best-line/transaction/rollout-progress',
        canLoad: [RolloutProgressGuard],
        loadChildren: () => import('./views/best-line/transaction/rollout-progress/rollout-progress.module').then(m => m.RolloutProgressModule)
      },
      {
        path: 'best-line/transaction/rollout-audit',
        canLoad: [RolloutAuditGuard],
        loadChildren: () => import('./views/best-line/transaction/rollout-audit/rollout-audit.module').then(m => m.RolloutAuditModule)
      },
      {
        canLoad: [CriticalProcessAnalysisGuard],
        path: 'best-line/transaction/critical-process-analysis',
        loadChildren: () => import('./views/best-line/transaction/critical-process-analysis/critical-process-analysis.module').then(m => m.CriticalProcessAnalysisModule)
      },

      {
        canLoad: [C2BLayoutByProcessGuard],
        path: 'best-line/transaction/c2b-layout-by-process',
        loadChildren: () => import('./views/best-line/transaction/c2blayout-by-process/c2blayout-by-process.module').then(m => m.C2blayoutByProcessModule)
      },
      {
        canLoad: [LineBalancingGuard],
        path: 'best-line/transaction/line-balancing',
        loadChildren: () => import('./views/best-line/transaction/line-balancing/line-balancing.module').then(m => m.LineBalancingModule)
      },
      {
        path: 'best-line/report/rollout-report',
        canLoad: [RolloutReportNavGuard],
        loadChildren: () => import('./views/best-line/report/rollout-report/rollout-report.module').then(m => m.RolloutReportModule)
      },
      {
        path: 'best-line/report/audit-report',
        canLoad: [AuditReportNavGuard],
        loadChildren: () => import('./views/best-line/report/audit-report/audit-report.module').then(m => m.AuditReportModule)
      },
      {
        path: 'best-line/report/pilot-line-setup-summary-tracking-report',
        canLoad: [PilotLineSetupSummaryTrackingGuard],
        loadChildren: () => import('./views/best-line/report/pilot-line-setup-summary-tracking-report/pilot-line-setup-summary-tracking-report.module').then(m => m.PilotLineSetupSummaryTrackingReportModule)
      },

      // ====================================Begin Production-BP==================================
      {
        canLoad: [BondingProgramSettingGuard],
        path: 'production-bp/transaction/bonding-program-setting',
        loadChildren: () => import('./views/production-bp/transaction/bonding-program-setting/bonding-program-setting.module').then(m => m.BondingProgramSettingModule)
      },
      {
        path: 'production-bp/transaction/computer-stitching-setting',
        canLoad: [ComputerStitchingSettingNavGuard],
        loadChildren: () => import('./views/production-bp/transaction/computer-stitching-setting/computer-stitching-setting.module').then(m => m.ComputerStitchingSettingModule)
      },
      {
        canLoad: [PadPrintSettingNavGuard],
        path: 'production-bp/transaction/pad-print-setting',
        loadChildren: () => import('./views/production-bp/transaction/pad-print-setting/pad-print-setting.module').then(m => m.PadPrintSettingModule)
      },
      {
        path: "production-bp/report/bonding-program-report",
        canLoad: [BondingProgramSettingReportGuard],
        loadChildren: () => import('./views/production-bp/report/bonding-report/bonding-report.module').then(m => m.BondingReportModule)
      },
      {
        path: "production-bp/report/computer-report",
        canLoad: [ComputerStitchingSettingReportGuard],
        loadChildren: () => import('./views/production-bp/report/computer-report/computer-report.module').then(m => m.ComputerReportModule)
      },
      {
        canLoad: [PadPrintSettingReportGuard],
        path: 'production-bp/report/pad-print-setting-report',
        loadChildren: () => import('./views/production-bp/report/pad-print-setting-report/pad-print-setting-report.module').then(m => m.PadPrintSettingReportModule)
      },
      // ====================================End Production-BP====================================
    ]
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { relativeLinkResolution: 'legacy' })],
  exports: [RouterModule],
})
export class AppRoutingModule { }
