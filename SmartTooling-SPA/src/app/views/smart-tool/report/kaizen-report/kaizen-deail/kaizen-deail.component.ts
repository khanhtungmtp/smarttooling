import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { environment } from "../../../../../../environments/environment";
import { KaizenReportService } from "../../../../../_core/_services/smart-tool/kaizen-report.service";
import { FunctionUtility } from "../../../../../_core/_utility/function-utility";


@Component({
  selector: 'app-kaizen-deail',
  templateUrl: './kaizen-deail.component.html',
  styleUrls: ['./kaizen-deail.component.scss']
})
export class KaizenDeailComponent implements OnInit {
  kaizen: any;
  kaizenDetail: any = null;
  improv: string;
  rftDiff: string;
  url: any = environment.imageUrl;
  isMediaBefore: string = '';
  isMediaAfter: string = '';
  constructor(private router: Router,
              private functionUtility: FunctionUtility,
              private kaizenService: KaizenReportService) { }

  ngOnInit() {
    this.kaizenService.currentKaizen.subscribe(res => this.kaizen = res);
    if(this.kaizen !== null) {
      this.getKaizenDetail();
    } else {
      this.router.navigate(["/report/kaizen-report/main"]);
    }
  }
  getKaizenDetail() {
    this.kaizenService.getKaizenDetail(this.kaizen.model_no,this.kaizen.serial_no).subscribe(res => {
      console.log(res);
      this.kaizenDetail = res;
      this.kaizenDetail.kaizen.before_remarks = this.functionUtility.replaceLineBreak(this.kaizenDetail.kaizen.before_remarks);
      this.kaizenDetail.kaizen.after_remarks = this.functionUtility.replaceLineBreak(this.kaizenDetail.kaizen.after_remarks);

      // ----------------------------Show media video or picture--------------------//
      if(!this.functionUtility.isEmpty(this.kaizenDetail.kaizen.before_media)) {
        let mediaBefore = this.kaizenDetail.kaizen.before_media.trim();
        if(mediaBefore.split('.')[1] === 'mp4' || mediaBefore.split('.')[1] === 'MP4') {
          this.isMediaBefore = 'mp4';
        } else {
          this.isMediaBefore = 'picture';
        }
      }

      if(!this.functionUtility.isEmpty(this.kaizenDetail.kaizen.after_media)) {
        let mediaAfter = this.kaizenDetail.kaizen.after_media.trim();
        if(mediaAfter.split('.')[1] === 'mp4' || mediaAfter.split('.')[1] === 'MP4') {
          this.isMediaAfter = 'mp4';
        } else {
          this.isMediaAfter = 'picture';
        }
      }
      
      this.improv = (100*(this.kaizenDetail.kaizen.ct_before_sec - this.kaizenDetail.kaizen.ct_after_sec) / this.kaizenDetail.kaizen.ct_before_sec).toFixed(2);
      this.rftDiff = (this.kaizenDetail.kaizen.rft_after_percent - this.kaizenDetail.kaizen.rft_before_percent).toFixed(2);
    });
  }
  backForm() {
    this.router.navigate(['/report/kaizen-report/model-detail']);
  }
}
