import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../../../../../../environments/environment';
import { CaptionConstants, MessageConstants } from '../../../../../_core/_constants/system.constants';
import { CriticalProcessReportDetailDTO } from '../../../../../_core/_models/best-line/critical-process-report-detail';
import { CriticalProcessReportKaizenDTO } from '../../../../../_core/_models/best-line/critical-process-report-kaizen';
import { CriticalProcessReportParam } from '../../../../../_core/_models/best-line/critical-process-report-param';
import { Pagination } from '../../../../../_core/_models/smart-tool/pagination';
import { CriticalProcessReportService } from '../../../../../_core/_services/best-line/critical-process-report.service';
import { NgSnotifyService } from '../../../../../_core/_services/ng-snotify.service';

@Component({
  selector: 'app-critical-process-report-detail',
  templateUrl: './critical-process-report-detail.component.html',
  styleUrls: ['./critical-process-report-detail.component.scss']
})
export class CriticalProcessReportDetailComponent implements OnInit {
  message = MessageConstants;
  flag: number = 0;
  dataDetail: CriticalProcessReportDetailDTO = {} as CriticalProcessReportDetailDTO;
  manImageOrVideo: boolean;
  machineImageOrVideo: boolean;
  methodImageOrVideo: boolean;
  materialImageOrVideo: boolean;
  dataKaizen: CriticalProcessReportKaizenDTO[] = [];
  model: CriticalProcessReportParam;
  url = environment.imageUrl;
  random: number = Math.random();
  pagination: Pagination = {
    currentPage: 1,
    itemsPerPage: 10,
    totalItems: 0,
    totalPages: 0,
  };
  constructor(
    private snotify: NgSnotifyService,
    private router: Router,
    private criticalReportService: CriticalProcessReportService,
  ) { }

  async ngOnInit() {
    this.criticalReportService.currentModel.subscribe(model => {
      if (model === null)
        this.back();
      else
        this.model = model;
    });
    await this.getDetail();
    await this.getKaizen();
  }

  back() {
    this.router.navigate(['/best-line/report/critical-process-report/main']);
  }

  async getDetail() {
    let data = await this.criticalReportService.getDetail(this.model).pipe(catchError(e => {
      this.snotify.error(MessageConstants.SYSTEM_ERROR_MSG, CaptionConstants.ERROR);
      return of(null);
    })).toPromise();
    if (data) {
      this.dataDetail = data;
      this.manImageOrVideo = this.dataDetail.man_media_url?.includes('mp4');
      this.machineImageOrVideo = this.dataDetail.machine_media_url?.includes('mp4');
      this.methodImageOrVideo = this.dataDetail.method_media_url?.includes('mp4');
      this.materialImageOrVideo = this.dataDetail.material_media_url?.includes('mp4');
    }
  }

  async getKaizen() {
    let res = await this.criticalReportService.getKaizen(this.pagination, this.model).pipe(catchError(e => {
      this.snotify.error(MessageConstants.SYSTEM_ERROR_MSG, CaptionConstants.ERROR);
      return of(null);
    })).toPromise();
    if (res) {
      this.dataKaizen = res.result;
      this.dataKaizen.map(item => {
        item.improv = Math.round(100 * (item.ct_before_sec - item.ct_after_sec) / item.ct_before_sec);
        item.improv = Number.isNaN(item.improv) ? null : item.improv;
        if (!!item.serial_no == false)
          this.flag++;
      });
      this.pagination = res.pagination;
    }
  }

  kaizenDetail(item: CriticalProcessReportKaizenDTO) {
    this.criticalReportService.changeKaizen(item);
    this.router.navigate(['/best-line/report/critical-process-report/kaizen-detail']);
  }

  async pageChanged(event: any) {
    this.pagination.currentPage = event.page;
    await this.getKaizen();
  }
}
