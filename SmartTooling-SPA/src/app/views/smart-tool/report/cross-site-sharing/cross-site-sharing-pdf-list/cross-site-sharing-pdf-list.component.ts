import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { NgxSpinnerService } from "ngx-spinner";
import { CaptionConstants, MessageConstants } from "../../../../../_core/_constants/system.constants";
import { NgSnotifyService } from "../../../../../_core/_services/ng-snotify.service";
import { CrossSiteSharingService } from "../../../../../_core/_services/smart-tool/cross-site-sharing.service";


@Component({
  selector: 'app-cross-site-sharing-pdf-list',
  templateUrl: './cross-site-sharing-pdf-list.component.html',
  styleUrls: ['./cross-site-sharing-pdf-list.component.scss']
})
export class CrossSiteSharingPdfListComponent implements OnInit {
  crossSiteSharing :any;
  models :any;
  constructor(private spinner: NgxSpinnerService,
              private crosssitesharingSerivce: CrossSiteSharingService,
              private snotifyAlert: NgSnotifyService,
              private router: Router) { }

  ngOnInit() {
    this.crosssitesharingSerivce.currentModel.subscribe(
      (crossSiteSharing) => (this.crossSiteSharing = crossSiteSharing)
    );
    if(this.crossSiteSharing ==null || this.crossSiteSharing ==undefined)
    {
      this.router.navigate(['/report/cross-site-sharing/main']);
    }
    else{
      this.loaddata();
    }
  }
  loaddata(){
    this.spinner.show();
    this.crosssitesharingSerivce.getCrossSiteSharingPDF(this.crossSiteSharing).subscribe(res=>{
      this.models = res;
      console.log(this.models);
      this.spinner.hide();
    },error=>{
      this.snotifyAlert.error(MessageConstants.UN_KNOWN_ERROR, CaptionConstants.ERROR);  
      this.spinner.hide();
    })
  }
  back(){
    this.router.navigate(['/report/cross-site-sharing/main']);
  }
 
}
