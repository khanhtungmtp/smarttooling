import { Component, OnInit } from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";
import { Select2OptionData } from "ng-select2";
import { NgxSpinnerService } from "ngx-spinner";
import { environment } from "../../../../../../environments/environment";
import { CaptionConstants } from "../../../../../_core/_constants/system.constants";
import { NgSnotifyService } from "../../../../../_core/_services/ng-snotify.service";
import { KaizenService } from "../../../../../_core/_services/smart-tool/kaizen.service";
import { FunctionUtility } from "../../../../../_core/_utility/function-utility";


@Component({
  selector: 'app-kaizen-edit',
  templateUrl: './kaizen-edit.component.html',
  styleUrls: ['./kaizen-edit.component.scss']
})
export class KaizenEditComponent implements OnInit {
  urlImage: any = environment.imageUrl + "images/no-image.jpg";
  url_after: any = environment.imageUrl;
  url_before: any = environment.imageUrl;
  stages: Array<Select2OptionData>;
  processList: Array<Select2OptionData>;
  Operations: Array<Select2OptionData>;
  kaizenFrom: Array<Select2OptionData>;
  imgBase64Before: any = null;
  imgBase64After: any = null;
  dataKaizen: any = {};
  process: string = '';
  stage: string = '';
  modelName: string = "";
  listdataModelNo: any;
  isvideoB4: boolean = false;
  isvideoAfter: boolean = false;
  model_no: string = "";
  serialNo: string = "";
  isLoaddata = false;
  constructor(private _kaizenService: KaizenService,
    private snotifyAlert: NgSnotifyService,
    private router: Router,
    private route: ActivatedRoute,
    private spinner: NgxSpinnerService,
    private functionUtility: FunctionUtility) { }

  ngOnInit() {
    this.model_no = this.route.snapshot.params['modelNo'];
    this.serialNo = this.route.snapshot.params['serial_no'];
    this.loaddata();
  }
  loaddata() {
    this.spinner.show();
    this._kaizenService.getKaizenEdit(this.model_no, this.serialNo).subscribe(res => {
      this.dataKaizen = res;
      console.log(res);
      if (this.dataKaizen.before_media != "") {
        if (this.dataKaizen.before_media.split(".").pop() == "mp4" ||
          this.dataKaizen.before_media.split(".").pop() == "MP4") {
          this.isvideoB4 = true;
        }
        this.url_before = this.url_before + this.dataKaizen.before_media + '?v=' + Math.random();

      }
      else {
        this.url_before = this.urlImage;
      }
      if (this.dataKaizen.after_media != "") {
        if (this.dataKaizen.after_media.split(".").pop() == "mp4" ||
          this.dataKaizen.after_media.split(".").pop() == "MP4") {
          this.isvideoAfter = true;
        }
        this.url_after = this.url_after + this.dataKaizen.after_media + '?v=' + Math.random();
      }
      else {
        this.url_after = this.urlImage;
      }
      this.getListStage();
      this.getListModelNo();
      this.getListKaizenFrom();
      this.isLoaddata = true;
      this.spinner.hide();
    }, error => {
      this.snotifyAlert.error("Can not load Kaizen", CaptionConstants.ERROR);
      this.spinner.hide();
    });
  }
  backList() {
    this.router.navigate(['/kaizen/kaizen/']);
  }
  cancel() {
  }
  onSelectFile(event, number) {
    if (event.target.files && event.target.files[0]) {
      const reader = new FileReader();

      reader.readAsDataURL(event.target.files[0]); // read file as data url
      var title = event.target.files[0].name.split(".").pop();
      var fileZise = event.target.files[0].size;
      reader.onload = (event) => {
        // called once readAsDataURL is completed
        // console.log("file: ", event.target)
        if (title == "jpg" ||
          title == "jpeg" ||
          title == "png" ||
          title == "JPG" ||
          title == "JPEG" ||
          title == "PNG" ||
          title == "MP4" ||
          title == "mp4") {
          if (fileZise <= 20971200) {
            if (number === 1) {
              this.isvideoB4 = false;
              if (title == "MP4" ||
                title == "mp4") {
                this.isvideoB4 = true;
              }
              this.url_before = event.target.result;
              this.imgBase64Before = event.target.result;
            } else {
              this.isvideoAfter = false;
              if (title == "MP4" ||
                title == "mp4") {
                this.isvideoAfter = true;
              }
              this.url_after = event.target.result;
              this.imgBase64After = event.target.result;
            }
          }
          else {
            this.snotifyAlert.error("Size too big", CaptionConstants.ERROR)
          }
        }
        else {
          this.snotifyAlert.error("Format error", CaptionConstants.ERROR)
        }
      }
    }
  }
  save() {
    if (this.imgBase64Before != null) {
      this.dataKaizen.before_media = this.imgBase64Before;
    }
    if (this.imgBase64After != null) {
      this.dataKaizen.after_media = this.imgBase64After;
    }
    if (this.dataKaizen.kaizen_type_combine == false && this.dataKaizen.kaizen_type_eliminate == false
      && this.dataKaizen.kaizen_type_reduce == false && this.dataKaizen.kaizen_type_smart_tool == false) {
      this.snotifyAlert.error("Please Chosise Kaizen Type", CaptionConstants.ERROR);
      return;
    }
    if (this.dataKaizen.ct_before_sec <= 0 || this.dataKaizen.ct_after_sec <= 0) {
      this.snotifyAlert.error("CT_Before and CT_After must be greater than 0", CaptionConstants.ERROR);
      return;
    }
    this.spinner.show();
    this._kaizenService.update(this.dataKaizen).subscribe(
      res => {
        if (res.success) {
          this.snotifyAlert.success(res.message, CaptionConstants.SUCCESS);
          this.spinner.hide();
          this.router.navigate(['/kaizen/kaizen/']);
        } else {
          this.snotifyAlert.error(res.message, CaptionConstants.ERROR);
          this.spinner.hide();
          return;
        }
      },
      (error) => {
        this.snotifyAlert.error(error, CaptionConstants.ERROR);
        this.spinner.hide();
      }
    );

  }
  getListModelNo() {
    this._kaizenService.geDataModelNo().subscribe(res => {
      this.listdataModelNo = res;
      this.modelName = this.listdataModelNo.find(x => x.model_no == this.dataKaizen.model_no).model_name;
    })
  }
  getListStage() {
    this._kaizenService.getDataStage().subscribe(res => {
      this.stages = res.map(item => {
        return { id: item.stage_id, text: item.stage_name };
      });
      this.getListProcess();
    })
  }
  getListKaizenFrom() {
    this._kaizenService.getKaizenFrom().subscribe(res => {
      this.kaizenFrom = res.map(item => {
        return { id: item.factory_id, text: item.factory_name };
      })
    })
  }
  getListProcess() {
    this._kaizenService.getProcess(this.dataKaizen.model_no, this.stage).subscribe(res => {
      this.processList = res.map(item => {
        return { id: item.process_type_id, text: item.process_type_name_en };
      });
      this.getListOpera();
    })
  }
  getListOpera() {
    this._kaizenService.getOpera(this.dataKaizen.model_no, this.stage, this.process).subscribe(res => {
      this.Operations = res.map(item => {
        return { id: item.operation_id, text: item.operation_name_en };
      })
    })
  }
  stageChange() {
    this.stage = this.dataKaizen.stage_id;
    this.process = '';
    this.getListProcess();
  }
  processChange() {
    this.getListOpera();
  }
  changedate(event: Date) {
    this.dataKaizen.start_date = this.functionUtility.returnDayNotTime(event);
  }
}
