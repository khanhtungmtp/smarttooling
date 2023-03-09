import { Component, Input, OnInit } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { environment } from '../../../../../../environments/environment';


@Component({
  selector: 'app-cross-site-sharing-pdf',
  templateUrl: './cross-site-sharing-pdf.component.html',
  styleUrls: ['./cross-site-sharing-pdf.component.scss']
})
export class CrossSiteSharingPdfComponent implements OnInit {
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
  @Input('data') data: any;
  constructor( 
    private spinner: NgxSpinnerService,
) { }

  ngOnInit() {
     this.loaddata();
  }
  loaddata(){
     this.spinner.show();
    
      this.models = this.data.crossSiteSharingDTO;
      this.kaizen =this.data;
      if (this.kaizen.before_media != "") {
        if (this.kaizen.before_media.split(".").pop() == "mp4" ||
          this.kaizen.before_media.split(".").pop() == "MP4") {
          this.isvideoB4 = true;
        }
        this.url_before = this.url_before + this.kaizen.before_media;

      }
      else {
        this.url_before = this.urlImage;
      }
      if (this.kaizen.after_media != "") {
        if (this.kaizen.after_media.split(".").pop() == "mp4" ||
          this.kaizen.after_media.split(".").pop() == "MP4") {
          this.isvideoAfter = true;
        }
        this.url_after = this.url_after + this.kaizen.after_media;
      }
      else {
        this.url_after = this.urlImage;
      }
      this.spinner.hide();
  }
  back(){
    
  }
  save(){
   
  }
  cancel(){

  }
}
