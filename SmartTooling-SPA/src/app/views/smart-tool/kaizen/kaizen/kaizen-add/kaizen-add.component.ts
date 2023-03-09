import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { Select2OptionData } from "ng-select2";
import { NgxSpinnerService } from "ngx-spinner";
import { environment } from "../../../../../../environments/environment";
import { CaptionConstants } from "../../../../../_core/_constants/system.constants";
import { Kaizen } from "../../../../../_core/_models/smart-tool/kaizen";
import { NgSnotifyService } from "../../../../../_core/_services/ng-snotify.service";
import { KaizenService } from "../../../../../_core/_services/smart-tool/kaizen.service";



@Component({
  selector: 'app-kaizen-add',
  templateUrl: './kaizen-add.component.html',
  styleUrls: ['./kaizen-add.component.scss']
})
export class KaizenAddComponent implements OnInit {
  urlImage: any = environment.imageUrl + "images/no-image.jpg";
  url_after: any = environment.imageUrl + "images/no-image.jpg";
  url_before: any = environment.imageUrl + "images/no-image.jpg";
  stages: Array<Select2OptionData>;
  processList: Array<Select2OptionData>;
  Operations: Array<Select2OptionData>;
  kaizenFrom: Array<Select2OptionData>;
  Model_no: string;
  modelName: string;
  dataKaizen: Kaizen = {
    ct_before_sec: 1,
    ct_after_sec: 1
  } as Kaizen;
  imgBase64Before: any = null;
  imgBase64After: any = null;
  stage: string = '';
  process: string = '';
  isvideoB4 = false;
  isvideoAfter = false;
  constructor(private _kaizenService: KaizenService,
    private snotifyAlert: NgSnotifyService,
    private router: Router,
    private spinner: NgxSpinnerService) { }

  ngOnInit() {
    this._kaizenService.modelNameSource.subscribe((modelName) => (this.modelName = modelName));
    this._kaizenService.modelNoSource.subscribe((modelno) => (this.Model_no = modelno));
    this.getListStage();
    this.getListKaizenFrom();
  }
  backList() {

    this.router.navigate(['/kaizen/kaizen/']);
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
          title == "mp4"
        ) {

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
  saveAndNext() {

    this.setData();
    if (this.dataKaizen.kaizen_type_combine == false && this.dataKaizen.kaizen_type_eliminate == false
      && this.dataKaizen.kaizen_type_reduce == false && this.dataKaizen.kaizen_type_smart_tool == false) {
      this.snotifyAlert.error("Please Chosise Kaizen Type", CaptionConstants.ERROR);
      return;
    }
    this.spinner.show();
    this._kaizenService.create(this.dataKaizen).subscribe(
      res => {
        if (res.success) {
          this.snotifyAlert.success(res.message, CaptionConstants.SUCCESS);
          this.dataKaizen = {
            ct_before_sec: 1,
            ct_after_sec: 1
          } as Kaizen;
          this.stage = '';
          this.process = '';
          this.url_after = this.urlImage;
          this.url_before = this.urlImage;
          this.spinner.hide();
        } else {
          this.snotifyAlert.error(res.message, CaptionConstants.ERROR);
          this.spinner.hide();
          return;
        }
      },
      (error) => {
        this.spinner.hide();
        this.snotifyAlert.error(error, CaptionConstants.ERROR);
      }
    );
  }
  save() {
    this.setData();
    if (this.dataKaizen.kaizen_type_combine == false && this.dataKaizen.kaizen_type_eliminate == false
      && this.dataKaizen.kaizen_type_reduce == false && this.dataKaizen.kaizen_type_smart_tool == false) {
      this.snotifyAlert.error("Please Chosise Kaizen Type", CaptionConstants.ERROR);
      return;
    }

    console.log(this.dataKaizen);
    this.spinner.show();
    this._kaizenService.create(this.dataKaizen).subscribe(
      res => {
        if (res.success) {
          this.spinner.hide();
          this.snotifyAlert.success(res.message, CaptionConstants.SUCCESS);
          this.router.navigate(['/kaizen/kaizen/']);
        } else {
          this.snotifyAlert.error(res.message, CaptionConstants.ERROR);
          this.spinner.hide();
          return;
        }
      },
      (error) => {
        this.snotifyAlert.error("Have Error!!!", CaptionConstants.ERROR);
        this.spinner.hide();
      }
    );

  }
  setData() {
    this.dataKaizen.model_no = this.Model_no;
    this.dataKaizen.before_media = this.imgBase64Before;
    this.dataKaizen.after_media = this.imgBase64After;
    this.dataKaizen.clicks_times = 0;
    this.dataKaizen.factory_id = "";
    this.dataKaizen.kaizen_type_combine = this.dataKaizen.kaizen_type_combine == undefined ? false : this.dataKaizen.kaizen_type_combine;
    this.dataKaizen.kaizen_type_eliminate = this.dataKaizen.kaizen_type_eliminate == undefined ? false : this.dataKaizen.kaizen_type_eliminate;
    this.dataKaizen.kaizen_type_reduce = this.dataKaizen.kaizen_type_reduce == undefined ? false : this.dataKaizen.kaizen_type_reduce;
    this.dataKaizen.kaizen_type_smart_tool = this.dataKaizen.kaizen_type_smart_tool == undefined ? false : this.dataKaizen.kaizen_type_smart_tool;
  }
  cancel() {
    this.dataKaizen = {
      ct_before_sec: 1,
      ct_after_sec: 1
    } as Kaizen;
    this.stage = '';
    this.process = '';
    this.url_after = this.urlImage;
    this.url_before = this.urlImage;
  }
  getListStage() {
    this._kaizenService.getDataStage().subscribe(res => {
      this.stages = res.map(item => {
        return { id: item.stage_id, text: item.stage_name };
      });
      this.dataKaizen.stage_id = '';
      this.getListProcess();
    })
  }
  getListKaizenFrom() {
    this._kaizenService.getKaizenFrom().subscribe(res => {
      this.kaizenFrom = res.map(item => {
        return { id: item.factory_id, text: item.factory_name };
      });
      this.dataKaizen.kaizen_from = '';
    })
  }
  getListProcess() {
    this._kaizenService.getProcess(this.Model_no, this.stage).subscribe(res => {
      this.processList = res.map(item => {
        return { id: item.process_type_id, text: item.process_type_name_en };
      });
      this.getListOpera();
    })
  }
  getListOpera() {
    this._kaizenService.getOpera(this.Model_no, this.stage, this.process).subscribe(res => {
      this.Operations = res.map(item => {
        return { id: item.operation_id, text: item.operation_name_en };
      });
      this.dataKaizen.operation_id = '';
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
}
