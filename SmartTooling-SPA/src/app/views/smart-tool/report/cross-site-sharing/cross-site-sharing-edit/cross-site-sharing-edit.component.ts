import { Component, OnInit } from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";
import { NgxSpinnerService } from "ngx-spinner";
import { environment } from "../../../../../../environments/environment";
import { CaptionConstants, MessageConstants } from "../../../../../_core/_constants/system.constants";
import { NgSnotifyService } from "../../../../../_core/_services/ng-snotify.service";
import { CrossSiteSharingService } from "../../../../../_core/_services/smart-tool/cross-site-sharing.service";


@Component({
  selector: 'app-cross-site-sharing-edit',
  templateUrl: './cross-site-sharing-edit.component.html',
  styleUrls: ['./cross-site-sharing-edit.component.scss']
})
export class CrossSiteSharingEditComponent implements OnInit {
  urlImage: any = environment.imageUrl + "images/no-image.jpg";
  url_after: any = environment.imageUrl;
  url_before: any = environment.imageUrl;
  model_no:string="";
  serialNo:string ="";
  factory:string ="";
  models:any ={};
  kaizen:any={};
  isvideoB4: boolean = false;
  isvideoAfter: boolean = false;
  constructor(    private router: Router,
    private route: ActivatedRoute,
    private spinner: NgxSpinnerService,
    private snotifyAlert: NgSnotifyService,
    private crosssitesharingSerivce: CrossSiteSharingService,) { }

  ngOnInit() {
    this.factory = this.route.snapshot.params['factory'];
    this.model_no = this.route.snapshot.params['modelNo'];
    this.serialNo = this.route.snapshot.params['serial_no'];
    this.loaddata();
  }
  loaddata(){
     this.spinner.show();
    this.crosssitesharingSerivce.getCrossSiteSharingEdit(this.factory,this.model_no,this.serialNo).subscribe(res=>{
      this.models = res.crossSiteSharingDTO;
      this.kaizen =res;
      if (this.kaizen.before_media != "") {
        if (this.kaizen.before_media.split(".").pop() == "mp4" ||
          this.kaizen.before_media.split(".").pop() == "MP4") {
          this.isvideoB4 = true;
        }
        this.url_before = this.url_before + this.kaizen.before_media + '?v=' + Math.random();

      }
      else {
        this.url_before = this.urlImage;
      }
      if (this.kaizen.after_media != "") {
        if (this.kaizen.after_media.split(".").pop() == "mp4" ||
          this.kaizen.after_media.split(".").pop() == "MP4") {
          this.isvideoAfter = true;
        }
        this.url_after = this.url_after + this.kaizen.after_media + '?v=' + Math.random();
      }
      else {
        this.url_after = this.urlImage;
      }
      this.spinner.hide();
    },error=>{
      this.snotifyAlert.error(MessageConstants.UN_KNOWN_ERROR, CaptionConstants.ERROR);
    })
  }
  back(){
    this.router.navigate(['/report/cross-site-sharing/main']);
  }
  save(){
    if(this.models.benefits_category_hse==false && this.models.benefits_category_quality ==false
      && this.models.benefits_category_delivery ==false && this.models.benefits_category_efficiency==false
      && this.models.benefits_category_others == false)
    {
      this.snotifyAlert.error("Please Choise Benefits Category", CaptionConstants.ERROR)
    }
    else
    {
      this.crosssitesharingSerivce.UpdateCrossSiteSharing(this.models).subscribe(
        res => {
          if(res.success) {
            this.snotifyAlert.success(res.message, CaptionConstants.SUCCESS);
            this.router.navigate(['/report/cross-site-sharing/main']);
          } else {
            this.snotifyAlert.error(res.message, CaptionConstants.ERROR);
            return;
          }
        },
        (error) => {
          this.snotifyAlert.error(error, CaptionConstants.ERROR);
        }
      );
    }
  }
  cancel(){
    this.router.navigate(['/report/cross-site-sharing/main']);
  }
}
