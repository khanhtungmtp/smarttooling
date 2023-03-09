import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../../../../../../environments/environment';
import { CaptionConstants, MessageConstants } from '../../../../../_core/_constants/system.constants';
import { C2BOverallLayoutReport } from '../../../../../_core/_models/best-line/overall-layout-report';
import { C2BOverallLayoutReportDetail } from '../../../../../_core/_models/best-line/overall-layout-report-detail';
import { C2BOverallLayoutReportFiles } from '../../../../../_core/_models/best-line/overall-layout-report-files';
import { C2BOverallLayoutReportParam } from '../../../../../_core/_models/best-line/overall-layout-report-param';
import { Pagination } from '../../../../../_core/_models/smart-tool/pagination';
import { C2bOverallLayoutReportService } from '../../../../../_core/_services/best-line/c2b-overall-layout-report.service';
import { NgSnotifyService } from '../../../../../_core/_services/ng-snotify.service';

@Component({
  selector: 'app-overall-layout-report-detail',
  templateUrl: './overall-layout-report-detail.component.html',
  styleUrls: ['./overall-layout-report-detail.component.scss']
})
export class OverallLayoutReportDetailComponent implements OnInit {
  message = MessageConstants;
  editModel: C2BOverallLayoutReport = {} as C2BOverallLayoutReport;
  dataModel: C2BOverallLayoutReportDetail = {} as C2BOverallLayoutReportDetail;
  files: C2BOverallLayoutReportFiles[] = [];
  searchModel: C2BOverallLayoutReportParam = {} as C2BOverallLayoutReportParam;
  baseUrl = environment.imageUrl;
  random: number = Math.random();
  pagination: Pagination = {
    currentPage: 1,
    itemsPerPage: 10,
    totalItems: 0,
    totalPages: 0,
  };
  constructor(
    private router: Router,
    private service: C2bOverallLayoutReportService,
    private snotify: NgSnotifyService
  ) { }

  async ngOnInit() {
    this.service.currentModel.subscribe(data => {
      if (data === null)
        this.back();
      else {
        this.editModel = data;
        this.searchModel = {
          factory: this.editModel.factory,
          line_no: this.editModel.line_id,
          line_type: this.editModel.line_type_id,
          model: this.editModel.model_no
        }
      }
    }).unsubscribe();
    await this.getDetail();
    await this.getFile();
  }

  async getDetail() {
    let data = await this.service.getDetail(this.searchModel).pipe(catchError(e => {
      this.snotify.error(MessageConstants.SYSTEM_ERROR_MSG, CaptionConstants.ERROR);
      return of(null);
    })).toPromise();
    if (data) {
      this.dataModel = data;
    }
  }

  back() {
    this.router.navigate(['/best-line/report/overall-layout-report/main']);
  }

  async getFile() {
    let data = await this.service.getFile(this.pagination, this.searchModel).pipe(catchError(e => {
      this.snotify.error(MessageConstants.SYSTEM_ERROR_MSG, CaptionConstants.ERROR);
      return of(null);
    })).toPromise();
    if (data) {
      this.files = data.result;
      this.pagination = data.pagination;
      this.files.forEach(element => {
        element.attachment_file_url = this.baseUrl + element.attachment_file_url;
      });
    }
  }

  async pageChanged(event: any) {
    this.pagination.currentPage = event.page;
    await this.getFile();
  }
}
