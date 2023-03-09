import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { NgxSpinnerService } from 'ngx-spinner';
import { PbpPadPrintSetting } from '../../../../../_core/_models/production-bp/pbp-pad-print-setting';
import { PadPrintSettingReportService } from '../../../../../_core/_services/production-bp/pad-print-setting-report.service';

@UntilDestroy()
@Component({
  selector: 'app-pad-print-setting-report-detail',
  templateUrl: './pad-print-setting-report-detail.component.html',
  styleUrls: ['./pad-print-setting-report-detail.component.scss']
})
export class PadPrintSettingReportDetailComponent implements OnInit {
  noImage: string = '../../../../../../assets/img/no-image.jpg';
  pbpPadPrintSettingData: PbpPadPrintSetting = {} as PbpPadPrintSetting;
  random: number = Math.random();

  constructor(
    private spinnerService: NgxSpinnerService,
    private PadPrintService: PadPrintSettingReportService,
    private router: Router,
  ) { }

  ngOnInit() {
    this.spinnerService.show();
    this.PadPrintService.currentPBPPadPrintSetting.pipe(untilDestroyed(this))
      .subscribe(res => {
        if (res !== null) {
          this.spinnerService.hide();
          this.pbpPadPrintSettingData = res;
        } else {
          this.back();
        }
      }).unsubscribe()
  }

  back() {
    this.router.navigate(['production-bp/report/pad-print-setting-report/main']);
  }

}
