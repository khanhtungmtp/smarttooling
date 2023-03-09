import { Injectable } from "@angular/core";
import { INavData } from "@coreui/angular";

export const navItems: INavData[] = [];

@Injectable({
  providedIn: "root", // <- ADD THIS
})
export class NavItem {
  // smart-tool
  hasMaintain: boolean;
  hasUserList: boolean;
  hasKaizen: boolean;
  hasReport: boolean;
  hasMeasurement: boolean;
  hasQuery: boolean;
  // best-line
  hasMaintainBestLine: boolean;
  hasTransaction: boolean;
  hasKanban: boolean;
  hasReportBestLine: boolean;
  hasQueryBestLine: boolean;
  //Production BP
  hasMaintainProductionBP: boolean;
  hasTransactionProductionBP: boolean;
  hasKanbanProductionBP: boolean;
  hasReportProductionBP: boolean;
  hasQueryProductionBP: boolean;

  navItems: INavData[] = [];
  constructor() { }

  getNav(user: any) {
    this.navItems = [];
    if (user == null) return [];

    //==========================================smart-tool==========================================
    const navSmartTool = {
      name: "SMART TOOLING",
      url: "",
      children: [],
      icon:"fa fa-gear",
      attributes: { class: 'font-weight-bold' }
    };

    this.hasUserList = false;
    this.hasMaintain = false;
    this.hasKaizen = false;
    this.hasReport = false;
    this.hasMeasurement = false;
    this.hasQuery = false;

    //====Setup Menu cha Maintain======
    const navItemMaintain = {
      name: "1. Maintain",
      url: "/maintain",
      icon: "fa fa-cogs",
      children: [],
    };
    //=================================
    //===Setup Menu cha Transaction=========
    const navItemKaizen = {
      name: "2. Transaction",
      url: "/transaction",
      icon: "fa fa-balance-scale",
      children: [],
    };
    //=================================
    //===Setup Menu cha Kanban====
    const navItemMeasurement = {
      name: "3. Kanban",
      url: "/kanban",
      icon: "fa fa-desktop",
      children: [],
    };
    //=================================
    //===Setup Menu cha Report=========
    const navItemReport = {
      name: "4. Report",
      url: "/url",
      icon: "fa fa-newspaper-o",
      children: [],
    };
    //=================================
    //===Setup Menu cha Query====
    const navItemQuery = {
      name: "5. Query",
      url: "/query",
      icon: "fa fa-search",
      children: [],
    };
    //=================================
    if (user != null) {
      user.role.forEach((element) => {
        //====Setup Menu con cho Maintain

        if (element === "ksmt.DefectReason") {
          const children = {
            name: "1.1 Defect Reason",
            url: "/defect-reason/list",
            class: "menu-margin",
          };
          this.hasMaintain = true;
          navItemMaintain.children.push(children);
        }
        if (element === "ksmt.UserList") {
          const children = {
            name: "1.2 User List",
            url: "/user",
            class: "menu-margin",
          };
          this.hasUserList = true;
          navItemMaintain.children.push(children);
        }
        //=================================

        //====Setup Menu con cho Transaction====
        if (element === "ksmt.Model") {
          const children = {
            name: "2.1 Model",
            url: "/model/list",
            class: "menu-margin",
          };
          this.hasKaizen = true;
          navItemKaizen.children.push(children);
        }
        if (element === "ksmt.ModelOperation") {
          const children = {
            name: "2.2 Model Operation",
            url: "/model-operation/list",
            class: "menu-margin",
          };
          this.hasKaizen = true;
          navItemKaizen.children.push(children);
        }
        if (element === "ksmt.ModelEfficiency") {
          const children = {
            name: "2.3 Model Efficiency",
            url: "/model-efficiency/edit",
            class: "menu-margin",
          };
          this.hasKaizen = true;
          navItemKaizen.children.push(children);
        }
        if (element === "ksmt.Kaizen") {
          const children = {
            name: "2.4 Kaizen",
            url: "/kaizen/kaizen/",
            class: "menu-margin",
          };
          this.hasKaizen = true;
          navItemKaizen.children.push(children);
        }
        if (element === "ksmt.KaizenRFT") {
          const children = {
            name: "2.5 Kaizen RFT",
            url: "/kaizen/kaizen-rft/",
            class: "menu-margin",
          };
          this.hasKaizen = true;
          navItemKaizen.children.push(children);
        }
        if (element === "ksmt.RFT") {
          const children = {
            name: "2.6 RFT ",
            url: "/measurement/list",
            class: "menu-margin",
          };
          this.hasKaizen = true;
          navItemKaizen.children.push(children);
        }
        //====================================

        //====Setup Menu con cho Kanban====

        //====================================

        //====Setup menu con cho Report=======
        if (element === "ksmt.KaizenReport") {
          const children = {
            name: "4.1 Kaizen Report",
            url: "/report/kaizen-report/main",
            class: "menu-margin",
          };
          this.hasReport = true;
          navItemReport.children.push(children);
        }
        if (element === "ksmt.GroupKaizenReport") {
          const children = {
            name: "4.2 Group Kaizen Report",
            url: "/report/group-kaizen-report/main",
            class: "menu-margin",
          };
          this.hasReport = true;
          navItemReport.children.push(children);
        }
        if (element === "ksmt.RFTReport") {
          const children = {
            name: "4.3 RFT Report",
            url: "/report/rft-report/main",
            class: "menu-margin",
          };
          this.hasReport = true;
          navItemReport.children.push(children);
        }
        if (element === "ksmt.GroupRFTReport") {
          const children = {
            name: "4.4 Group RFT Report",
            url: "/report/group-rft-report/main",
            class: "menu-margin",
          };
          this.hasReport = true;
          navItemReport.children.push(children);
        }
        if (element === "ksmt.CrossSiteSharingApplication") {
          const children1 = {
            name: "4.5 Cross Site Sharing Application",
            url: "/report/cross-site-sharing/main",
            class: "menu-margin",
          };
          this.hasReport = true;
          navItemReport.children.push(children1);
        }

        //====================================
        //Setup menu con cho Query

      });
    }

    //kiểm tra xem có Maintain không
    if (this.hasMaintain) {
      navSmartTool.children.push(navItemMaintain);
    }
    //kiểm tra xem có Transaction không
    if (this.hasKaizen) {
      navSmartTool.children.push(navItemKaizen);
    }
    //kiểm tra xem có Kanban không
    if (this.hasMeasurement) {
      navSmartTool.children.push(navItemMeasurement);
    }
    //kiểm tra xem có Report không
    if (this.hasReport) {
      navSmartTool.children.push(navItemReport);
    }
    //thêm user sau cùng nếu có quyền user
    if (this.hasQuery) {
      navSmartTool.children.push(navItemQuery);
    }
    this.navItems.push(navSmartTool);
    //==========================================end-smart-tool==========================================

    //==========================================best-line==========================================
    const navBestLine = {
      name: "PILOT LINE",
      url: "",
      children: [],
      icon:"fa fa-male",
      attributes: { class: 'font-weight-bold' }
    };

    this.hasMaintainBestLine = false;
    this.hasTransaction = false;
    this.hasKanban = false;
    this.hasReportBestLine = false;
    this.hasQueryBestLine = false;

    //====Setup Menu cha Maintain======
    const navMaintainBestLine = {
      name: "1. Maintain",
      url: "/best-line/",
      icon: "fa fa-cogs",
      children: [],
    };
    //=================================
    //===Setup Menu cha Transaction=========
    const navTransaction = {
      name: "2. Transaction",
      url: "/best-line/transaction",
      icon: "fa fa-balance-scale",
      children: [],
    };
    //=================================
    //===Setup Menu cha Kanban====
    const navKanban = {
      name: "3. Kanban",
      url: "/best-line/",
      icon: "fa fa-desktop",
      children: [],
    };
    //=================================
    //===Setup Menu cha Report=========
    const navItemReportBestLine = {
      name: "4. Report",
      url: "/best-line/report/",
      icon: "fa fa-newspaper-o",
      children: [],
    };
    //=================================
    //===Setup Menu cha Query====
    const navQueryBestLine = {
      name: "5. Query",
      url: "/best-line/",
      icon: "fa fa-search",
      children: [],
    };

    if (user != null) {
      user.role.forEach((element) => {
        //====Setup Menu con cho Maintain

        //====Setup Menu con cho Transaction====
        if (element === "bl.C2B.OverallLayout") {
          const children = {
            name: "2.1 C2B Overall Layout",
            url: "/best-line/transaction/layout-design-overall/main",
            class: "menu-margin",
          };
          this.hasTransaction = true;
          navTransaction.children.push(children);
        }
        if (element === "bl.C2B.OverallLayoutAttachment") {
          const children = {
            name: "2.2 C2B Layout Attachment",
            url: "/best-line/transaction/layout-attachment/main",
            class: "menu-margin",
          };
          this.hasTransaction = true;
          navTransaction.children.push(children);
        }

        if (element === "bl.CriticalProcessAnalysis") {
          const children = {
            name: "2.5 Critical Process Analysis",
            url: "/best-line/transaction/critical-process-analysis/main",
            class: "menu-margin",
          };
          this.hasTransaction = true;
          navTransaction.children.push(children);
        }
        if (element === 'bl.C2B.LayoutByProcess') {
          const hasC2BLLayoutByProcess = {
            name: "2.3 C2B Layout By Process",
            url: "/best-line/transaction/c2b-layout-by-process",
            class: "menu-margin",
          };
          this.hasTransaction = true;
          navTransaction.children.push(hasC2BLLayoutByProcess);
        }
        if (element === 'bl.LineBalancing') {
          const hasLineBalancing = {
            name: "2.4 Line Balancing",
            url: "/best-line/transaction/line-balancing",
            class: "menu-margin",
          };
          this.hasTransaction = true;
          navTransaction.children.push(hasLineBalancing);
        }
        if (element === "bl.RolloutProgress") {
          const children = {
            name: "2.6 Rollout Progress",
            url: "/best-line/transaction/rollout-progress/list",
            class: "menu-margin",
          };
          this.hasTransaction = true;
          navTransaction.children.push(children);
        }
        if (element === "bl.RolloutAudit") {
          const children = {
            name: "2.7 Rollout Audit",
            url: "/best-line/transaction/rollout-audit/list",
            class: "menu-margin",
          };
          this.hasTransaction = true;
          navTransaction.children.push(children);
        }
        // ====Setup menu con cho Report=======
        if (element === "bl.C2B.OverallLayoutReport") {
          const children = {
            name: "4.1 C2B Overall Layout Report",
            url: "/best-line/report/overall-layout-report/main",
            class: "menu-margin",
          };
          this.hasReportBestLine = true;
          navItemReportBestLine.children.push(children);
        }
        if (element === "bl.C2B.LayoutByProcessReport") {
          const children = {
            name: "4.2 C2B Layout by Process Report",
            url: "/best-line/report/layout-by-process-report/main",
            class: "menu-margin",
          };
          this.hasReportBestLine = true;
          navItemReportBestLine.children.push(children);
        }
        if (element === "bl.CriticalProcessReport") {
          const children = {
            name: "4.3 Critical Process Report",
            url: "/best-line/report/critical-process-report/main",
            class: "menu-margin",
          };
          this.hasReportBestLine = true;
          navItemReportBestLine.children.push(children);
        }
        if (element === "bl.RolloutReport") {
          const children = {
            name: "4.4 Rollout Report",
            url: "/best-line/report/rollout-report",
            class: "menu-margin"
          };
          this.hasReportBestLine = true;
          navItemReportBestLine.children.push(children);
        }
        if (element === "bl.AuditReport") {
          const children = {
            name: "4.5 Audit Report",
            url: "/best-line/report/audit-report",
            class: "menu-margin"
          };
          this.hasReportBestLine = true;
          navItemReportBestLine.children.push(children);
        }
        if (element === "bl.PilotLineSetupSummaryTracking") {
          const children = {
            name: "4.6 Pilot Line Setup Summary Tracking Report",
            url: "/best-line/report/pilot-line-setup-summary-tracking-report",
            class: "menu-margin"
          };
          this.hasReportBestLine = true;
          navItemReportBestLine.children.push(children);
        }
      });
    }
    //kiểm tra xem có Maintain không
    if (this.hasMaintainBestLine) {
      navBestLine.children.push(navMaintainBestLine);
    }
    //kiểm tra xem có Transaction không
    if (this.hasTransaction) {
      navBestLine.children.push(navTransaction);
    }
    //kiểm tra xem có Kanban không
    if (this.hasKanban) {
      navBestLine.children.push(navKanban);
    }
    //kiểm tra xem có Report không
    if (this.hasReportBestLine) {
      navBestLine.children.push(navItemReportBestLine);
    }
    //thêm query
    if (this.hasQueryBestLine) {
      navBestLine.children.push(navQueryBestLine);
    }

    this.navItems.push(navBestLine);
    //==========================================best-line==========================================



    //==========================================Begin Production-BP==========================================
    const navProductionBP = {
      name: "PRODUCTION BP",
      url: "",
      children: [],
      icon:"fa fa-product-hunt",
      attributes: { class: 'font-weight-bold' }
    };

    this.hasMaintainProductionBP = false;
    this.hasTransactionProductionBP = false;
    this.hasKanbanProductionBP = false;
    this.hasReportProductionBP = false;
    this.hasQueryProductionBP = false;

    //====Setup Menu cha Maintain======
    const navMaintainProductionBP = {
      name: "1. Maintain",
      url: "/production-bp/maitain",
      icon: "fa fa-cogs",
      children: [],
    };
    //=================================
    //===Setup Menu cha Transaction=========
    const navTransactionProductionBP = {
      name: "2. Transaction",
      url: "/production-bp/transaction/",
      icon: "fa fa-balance-scale",
      children: [],
    };
    //=================================
    //===Setup Menu cha Kanban====
    const navKanbanProductionBP = {
      name: "3. Kanban",
      url: "/production-bp/kaban",
      icon: "fa fa-desktop",
      children: [],
    };
    //=================================
    //===Setup Menu cha Report=========
    const navItemReportProductionBP = {
      name: "4. Report",
      url: "/production-bp/report",
      icon: "fa fa-newspaper-o",
      children: [],
    };
    //=================================
    //===Setup Menu cha Query====
    const navQueryProductionBP = {
      name: "5. Query",
      url: "/production-bp/query",
      icon: "fa fa-search",
      children: [],
    };

    if (user != null) {
      user.role.forEach((element) => {
        //====Setup Menu con cho Maintain

        //====Setup Menu con cho Transaction====
        if (element === "pbp.BondingProgramSetting") {
          const children = {
            name: "2.1 Bonding Program Setting",
            url: "/production-bp/transaction/bonding-program-setting/main",
            class: "menu-margin",
          };
          this.hasTransactionProductionBP = true;
          navTransactionProductionBP.children.push(children);
        }
        if (element === "pbp.ComputerStitchingSetting") {
          const children = {
            name: "2.2 Computer Stitching Setting",
            url: "/production-bp/transaction/computer-stitching-setting",
            class: "menu-margin",
          };
          this.hasTransactionProductionBP = true;
          navTransactionProductionBP.children.push(children);
        }
        if (element === "pbp.PadPrintSetting") {
          const children = {
            name: "2.3 Pad Print Setting",
            url: "/production-bp/transaction/pad-print-setting/main",
            class: "menu-margin",
          };
          this.hasTransactionProductionBP = true;
          navTransactionProductionBP.children.push(children);
        }

        // ====Setup menu con cho Report=======
        if (element === "pbp.BondingProgramSettingReport") {
          const children = {
            name: "4.1 Bonding Program Setting Report",
            url: "/production-bp/report/bonding-program-report/main",
            class: "menu-margin",
          };
          this.hasReportProductionBP = true;
          navItemReportProductionBP.children.push(children);
        }
        if (element === "pbp.ComputerStitchingSettingReport") {
          const children = {
            name: "4.2 Computer Stitching Setting Report",
            url: "/production-bp/report/computer-report/main",
            class: "menu-margin",
          };
          this.hasReportProductionBP = true;
          navItemReportProductionBP.children.push(children);
        }
        if (element === "pbp.PadPrintSettingReport") {
          const children = {
            name: "4.3 Pad Print Setting Report",
            url: "/production-bp/report/pad-print-setting-report/main",
            class: "menu-margin",
          };
          this.hasReportProductionBP = true;
          navItemReportProductionBP.children.push(children);
        }
      });
    }
    //kiểm tra xem có Maintain không
    if (this.hasMaintainProductionBP) {
      navProductionBP.children.push(navMaintainProductionBP);
    }
    // //kiểm tra xem có Transaction không
    if (this.hasTransactionProductionBP) {
      navProductionBP.children.push(navTransactionProductionBP);
    }
    // //kiểm tra xem có Kanban không
    if (this.hasKanbanProductionBP) {
      navProductionBP.children.push(navKanbanProductionBP);
    }
    // //kiểm tra xem có Report không
    if (this.hasReportProductionBP) {
      navProductionBP.children.push(navItemReportProductionBP);
    }
    // //thêm query
    if (this.hasQueryProductionBP) {
      navProductionBP.children.push(navQueryProductionBP);
    }

    this.navItems.push(navProductionBP);
    //==========================================End Production-BP==========================================

    return this.navItems;
  }
}
