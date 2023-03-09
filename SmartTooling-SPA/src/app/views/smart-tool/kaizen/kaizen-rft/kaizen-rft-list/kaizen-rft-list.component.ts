import { Component, OnInit, SimpleChanges } from "@angular/core";
import { Router } from "@angular/router";
import { Select2OptionData } from "ng-select2";
import { NgxSpinnerService } from "ngx-spinner";
import { Kaizen } from "../../../../../_core/_models/smart-tool/kaizen";
import { Pagination } from "../../../../../_core/_models/smart-tool/pagination";
import { KaizenService } from "../../../../../_core/_services/smart-tool/kaizen.service";


@Component({
  selector: 'app-kaizen-rft-list',
  templateUrl: './kaizen-rft-list.component.html',
  styleUrls: ['./kaizen-rft-list.component.scss']
})
export class KaizenRftListComponent implements OnInit {

  listdataModelNo: any;
  listModelNo: Array<Select2OptionData>;
  modelNo: string = '';
  upperID: string = '';
  modelName: string = '';
  pagination: Pagination = {
    currentPage: 1,
    itemsPerPage: 10,
    totalItems: 1,
    totalPages: 1,
  };
  checkAddnew: boolean = false;
  dataKaizen: Kaizen[] = [];
  constructor(private _kaizenService: KaizenService,
    private router: Router,
    private spinner: NgxSpinnerService) { }
  ngOnChanges(changes: SimpleChanges): void {
    // @Input()
    console.log("changee", changes);
  }
  ngOnInit() {
    this._kaizenService.modelNoRftSource.asObservable().subscribe(res => {
      this.modelNo = res;
    });
    this.getListModelNo();
  }
  pageChanged(event) {
    this.pagination.currentPage = event.page;
    this.getData();
  }
  changeModelNo(event: any) {
    this.upperID = '';
    this.modelName = '';
    if (event != '') {
      this.upperID = this.listdataModelNo.find(x => x.model_no == event).upper_id;
      this.modelName = this.listdataModelNo.find(x => x.model_no == event).model_name;
      this.checkAddnew = true;
    }
    else {
      this.checkAddnew = false;
    }
    this.getData();
  }
  getListModelNo() {
    this._kaizenService.geDataModelNo().subscribe(res => {
      this.listdataModelNo = res;
      this.listModelNo = res.map(item => {
        return { id: item.model_no, text: item.model_no };
      })
    })
  }
  getData() {
    this.spinner.show();
    this._kaizenService.modelNoRftSource.next(this.modelNo);
    this._kaizenService.search(this.pagination.currentPage, this.modelNo).subscribe(res => {
      this.pagination = res.pagination;
      this.dataKaizen = res.result;
      this.spinner.hide();
    })
  }
  edit(kaizen) {
    // this._kaizenService.changeKaizen(kaizen);
    this.router.navigate(['/kaizen/kaizen-rft/kaizen-rft-edit/' + kaizen.model_no + "/" + kaizen.serial_no]);
  }
}
