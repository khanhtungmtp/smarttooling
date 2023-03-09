import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { NgxSpinnerService } from 'ngx-spinner';
import { PBPBondingProgramSetting } from '../../../../../_core/_models/production-bp/pbp-bonding-program-setting';
import { BondingReportService } from '../../../../../_core/_services/production-bp/bonding-report.service';
@UntilDestroy()
@Component({
  selector: 'app-bonding-report-detail',
  templateUrl: './bonding-report-detail.component.html',
  styleUrls: ['./bonding-report-detail.component.css']
})
export class BondingReportDetailComponent implements OnInit {

  dataBondingProgramSetting: PBPBondingProgramSetting = {} as PBPBondingProgramSetting;
	noImage: string = "../../../../../../assets/img/no-image.jpg";
  random: number = Math.random();
  constructor(
    private spinnerService: NgxSpinnerService,
    private bondingReportService: BondingReportService,
    private router: Router,
  ) { }

  async ngOnInit() {
    this.spinnerService.show();
    this.bondingReportService.currentModel.pipe(untilDestroyed(this))
      .subscribe(res => {
        if(res !== null)
        {
          this.spinnerService.hide();
          this.dataBondingProgramSetting = res;
        }else{
          this.router.navigate(['production-bp/report/bonding-program-report/main']);
        }
      }).unsubscribe();
  }
  backList(){
    this.router.navigate(['production-bp/report/bonding-program-report/main']);
  }
}
