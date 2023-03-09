import { BLAuditReportDetailDTO } from './../../../../../_core/_models/best-line/bl-audit-report-detail.model';
import { BLSearchAuditReportParam } from '../../../../../_core/_helpers/params/best-line/bl-search-audit-report.param';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BlAuditReportService } from '../../../../../_core/_services/best-line/bl-audit-report.service';
import { Pagination } from '../../../../../_core/_models/smart-tool/pagination';
import { environment } from '../../../../../../environments/environment';

@Component({
  selector: 'app-audit-report-detail',
  templateUrl: './audit-report-detail.component.html',
  styleUrls: ['./audit-report-detail.component.scss']
})
export class AuditReportDetailComponent implements OnInit {
  imageUrl: string = environment.imageUrl;
  noImage: string = "../../../../../../assets/img/no-image.jpg";
  auditDetails: BLAuditReportDetailDTO[] = [];
  searchParam: BLSearchAuditReportParam = {
    factory: "",
    lineID: "",
    rolloutLineID: "",
    lineTypeID: "",
    modelNo: "",
    stageID: "",
    operationID: "",
    lang: ""
  } as BLSearchAuditReportParam;
  bestPracticeUrl: string = '';
  layoutLink: string = '';
  lang: string;
  pagination: Pagination = {
    currentPage: 1,
    itemsPerPage: 10
  } as Pagination;
  random: number = Math.random();
  constructor(
    private auditService: BlAuditReportService,
    private router: Router,
    private route: ActivatedRoute,
  ) { }

  ngOnInit() {
    this.loadData();
    this.getBestPracticeUrl();
    this.getlayoutLink();
  }

  loadData() {
    this.route.data.subscribe(res => {
      this.searchParam = res.auditDetail[0];
      this.auditDetails = res.auditDetail[1].result;
      this.pagination = res.auditDetail[1].pagination;
    });
    this.lang = this.searchParam.lang;
  }

  getBestPracticeUrl() {
    this.auditService.bestPracticeUrl(this.searchParam).subscribe(res => {
      this.bestPracticeUrl = res.best_practice_url;
    });
  }

  getlayoutLink() {
    this.auditService.layoutLink(this.searchParam).subscribe(res => {
      this.layoutLink = res.c2b_overall_image;
    });
  }

  back() {
    this.router.navigate(['/best-line/report/audit-report']);
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadData();
  }

}
