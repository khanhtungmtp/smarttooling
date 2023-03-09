import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { CaptionConstants, MessageConstants } from "../../../../../_core/_constants/system.constants";
import { NgSnotifyService } from "../../../../../_core/_services/ng-snotify.service";
import { DefectReasonService } from "../../../../../_core/_services/smart-tool/defect-reason.service";


@Component({
  selector: "app-defect-reason-add",
  templateUrl: "./defect-reason-add.component.html",
  styleUrls: ["./defect-reason-add.component.scss"],
})
export class DefectReasonAddComponent implements OnInit {
  defectreason: any = {};
  flag = "100";
  constructor(
    private defectreasonService: DefectReasonService,
    private snotifyAlert: NgSnotifyService,
    private router: Router
  ) {}

  ngOnInit() {

    this.defectreasonService.currentdefectreason.subscribe(
      (defectreason) => (this.defectreason = defectreason)
    );
    // flag 1=add, 0=edit
    this.defectreasonService.currentFlag.subscribe(
      (flag) => (this.flag = flag)
    );

    if(this.flag=== '0')  this.defectreason.sequence = 0;
  }

  backList() {
    this.router.navigate(["/defect-reason/list"]);
  }

  saveAndNext() {
    console.log("save and next");
    if (this.flag === "0") {
      this.defectreasonService.createDefectReason(this.defectreason).subscribe(
        () => {
          this.snotifyAlert.success(MessageConstants.CREATED_OK_MSG, CaptionConstants.SUCCESS);
          this.defectreason = {};
          // save page
        },
        (error) => {
          this.snotifyAlert.error(error, CaptionConstants.ERROR);
        }
      );
    }
  }

  save() {
    if (this.flag === "0") {
      console.log("save add");
      this.defectreasonService.createDefectReason(this.defectreason).subscribe(
        () => {
          this.snotifyAlert.success(MessageConstants.CREATED_OK_MSG, CaptionConstants.SUCCESS);
          this.router.navigate(["/defect-reason/list"]);
        },
        (error) => {
          this.snotifyAlert.error(error, CaptionConstants.ERROR);
        }
      );
    } else {
      console.log("save edit");
      this.defectreasonService.updateDefectReason(this.defectreason).subscribe(
        () => {
          this.snotifyAlert.success(MessageConstants.UPDATED_OK_MSG, CaptionConstants.SUCCESS);
          this.router.navigate(["//defect-reason/list"]);
        },
        (error) => {
          this.snotifyAlert.error(error, CaptionConstants.ERROR);
        }
      );
    }
  }

  clear() {
    this.defectreason = {};
  }

  // // for Sequence column
  // numberOnly(event): boolean {
  //   const charCode = event.which ? event.which : event.keyCode;
  //   if (charCode > 31 && (charCode < 48 || charCode > 57)) {
  //     return false;
  //   }
  //   return true;
  // }
}
