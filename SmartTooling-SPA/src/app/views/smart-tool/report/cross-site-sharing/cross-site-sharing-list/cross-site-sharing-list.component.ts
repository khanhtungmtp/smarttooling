import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { NgxSpinnerService } from "ngx-spinner";
import { CaptionConstants, MessageConstants } from "../../../../../_core/_constants/system.constants";
import { Factory } from "../../../../../_core/_models/smart-tool/factory";
import { ModelCrossSiteSharing } from "../../../../../_core/_models/smart-tool/model-cross-site-sharing";
import { Pagination } from "../../../../../_core/_models/smart-tool/pagination";
import { NgSnotifyService } from "../../../../../_core/_services/ng-snotify.service";
import { CrossSiteSharingService } from "../../../../../_core/_services/smart-tool/cross-site-sharing.service";


@Component({
  selector: 'app-cross-site-sharing-list',
  templateUrl: './cross-site-sharing-list.component.html',
  styleUrls: ['./cross-site-sharing-list.component.scss']
})
export class CrossSiteSharingListComponent implements OnInit {
  pagination: Pagination = {
    currentPage: 1,
    itemsPerPage: 10,
    totalItems: 0,
    totalPages: 0,
  };
  factories: Factory[];
  factory: string ="";
  model_no: string = "";
  models:ModelCrossSiteSharing[]=[];
  filterParam: any;
  constructor(private crosssitesharingSerivce: CrossSiteSharingService,
    private spinnerService: NgxSpinnerService,
    private snotifyAlert: NgSnotifyService,
    private router: Router) { }

  ngOnInit() {
    this.crosssitesharingSerivce.paramSearchSource.asObservable().subscribe(res => {
      if(res !== null) {
        this.factory = res.factory_id;
        this.model_no = res.model_No;
      }
    });
    this.getAllFactory();
    this.getData();
  }
  getAllFactory() {
    this.crosssitesharingSerivce.getAllFactory().subscribe((res) => {
      this.factories = res;
    });
  }
  getData() {

      this.filterParam = {
        factory_id: this.factory,
        model_No: this.model_no.toUpperCase(),
      };
      this.spinnerService.show();
      this.crosssitesharingSerivce
        .search(
          this.pagination.currentPage,
          this.pagination.itemsPerPage,
          this.filterParam
        )
        .subscribe((res) => {
          this.spinnerService.hide();
          this.models = res.result;
          this.pagination = res.pagination;
        },
        (error)=>{
          this.spinnerService.hide();
          this.snotifyAlert.error(MessageConstants.UN_KNOWN_ERROR, CaptionConstants.ERROR);
        });
  }
  search() {
    this.pagination.currentPage = 1;
     this.getData();
  }
  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.getData();
  }
  clear() {
    this.models.length = 0;
    this.model_no = '';
    this.factory = '';
    this.crosssitesharingSerivce.paramSearchSource.next(null);
  }
  changeChoise(item :any){
    item.isChoise = !item.isChoise;
  }
  edit(item:any){
    this.changeParamSearch();
    this.router.navigate(['/report/cross-site-sharing/Cross-detail/'+item.to_factory_id+'/'+item.model_no+"/"+item.serial_no]);
  }
  changeParamSearch() {
    let filterParam = {
      factory_id: this.factory,
      model_No: this.model_no,
    };
    this.crosssitesharingSerivce.paramSearchSource.next(filterParam);
  }
  exportPDF(){
    let print = false;
    this.models.forEach(res=>{
      if(res.isChoise ==true)
      {
        print = true;
      }
    });
    if(print)
    {
      this.changeParamSearch();
      this.crosssitesharingSerivce.changeCrossSiteSharing(this.models);
      this.router.navigate(['/report/cross-site-sharing/Cross-pdf/']);
    }
  }
}
