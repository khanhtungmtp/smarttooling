import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { NgxSpinnerService } from 'ngx-spinner';
import { ComputerReportParam } from '../../../../../_core/_helpers/params/production-bp/computer-report-param';
import { PbpComputerstitchingsetting } from '../../../../../_core/_models/production-bp/pbp-computerstitchingsetting';
import { ComputerReportService } from '../../../../../_core/_services/production-bp/computer-report.service';

@UntilDestroy()
@Component({
  selector: 'app-computer-report-detail',
  templateUrl: './computer-report-detail.component.html',
  styleUrls: ['./computer-report-detail.component.css']
})
export class ComputerReportDetailComponent implements OnInit {

  dataComputerstitchingsetting: PbpComputerstitchingsetting = {} as PbpComputerstitchingsetting;
  noImage: string = "../../../../../../assets/img/no-image.jpg";
  factory: string = localStorage.getItem('factorySmartTooling');
  searchParam: ComputerReportParam = {
    factory: this.factory,
    dev_season: '',
    prod_season: '',
    model: '',
    production_adoption: '',
    changeLang: '0',
  } as ComputerReportParam;
  random: number = Math.random();
  constructor(
    private spinnerService: NgxSpinnerService,
    private computerReportService: ComputerReportService,
    private router: Router,
  ) { }

  async ngOnInit() {
    this.spinnerService.show();
    this.computerReportService.currentParamSearch.pipe(untilDestroyed(this))
      .subscribe(res => {
        this.searchParam.changeLang = res.changeLang;
      }).unsubscribe();
    this.computerReportService.currentModel.pipe(untilDestroyed(this))
      .subscribe(res => {
        if (res !== null) {
          this.spinnerService.hide();
          this.dataComputerstitchingsetting = res;
        } else {
          this.router.navigate(['production-bp/report/computer-report/main']);
        }
      }).unsubscribe();
  }
  backList() {
    this.router.navigate(['production-bp/report/computer-report/main']);
  }

}
