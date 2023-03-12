import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { Select2OptionData } from 'ng-select2';
import { NgxSpinnerService } from 'ngx-spinner';
import { environment } from '../../../../../../environments/environment';
import { ActionConstants, CaptionConstants, MessageConstants } from '../../../../../_core/_constants/system.constants';
import { BLLayoutDesignOverallDetail } from '../../../../../_core/_models/best-line/bl-layout-design-overall-detail';
import { LayoutDesignOverallService } from '../../../../../_core/_services/best-line/layout-design-overall.service';
import { NgSnotifyService } from '../../../../../_core/_services/ng-snotify.service';
import { MediaUploadComponent } from '../../../../commons/media-upload/media-upload.component';

@Component({
  selector: 'app-form',
  templateUrl: './form.component.html',
  styleUrls: ['./form.component.scss']
})
export class FormComponent implements OnInit {

  baseUrl: string = environment.imageUrl;
  imageUrlApi: string = environment.baseUrl
  defaultImage: string =
    this.baseUrl + localStorage.getItem('factorySmartTooling') + "/Model/no-image.jpg";
  param: BLLayoutDesignOverallDetail = <BLLayoutDesignOverallDetail>{
    line_id: '',
    line_type_id: '',
    model_no: '',
    c2b_overall_image: '',
    type: ''
  }
  @ViewChild('image') image!: MediaUploadComponent;
  currentUrl: string = ''
  modelNoList: Array<Select2OptionData> = [];
  lineNoList: Array<Select2OptionData> = [];
  lineTypeList: Array<Select2OptionData> = [];
  optionsSelectLineNo = {
    placeholder: "Select Line No...",
    allowClear: true,
    width: "100%",
  };
  optionsSelectLineType = {
    placeholder: "Select Line Type...",
    allowClear: true,
    width: "100%",
  };
  optionsSelectModelNo = {
    placeholder: "Select Model No...",
    allowClear: true,
    width: "100%",
  };
  constructor(
    private layoutDesignOverallService: LayoutDesignOverallService,
    private snotify: NgSnotifyService,
    private spinnerService: NgxSpinnerService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) { }

  ngAfterViewChecked() {
    this.cdr.detectChanges();
  }

  ngOnInit() {
    this.spinnerService.show();
    this.getAllLineNo();
    this.getAllLineType();
    this.getAllModelNo();
    let url = this.router.url.split('/');
    this.currentUrl = url[url.length - 1];
    if (this.currentUrl == 'edit') {
      this.loadData();
    }
    this.spinnerService.hide();
  }

  loadData() {
    this.spinnerService.show();
    this.layoutDesignOverallService.currentParamSearch
      .subscribe({
        next: (res) => {
          this.spinnerService.hide();
          if (!res)
            this.back()
          this.param = { ...res }
        },
        error: () => {
          this.spinnerService.hide();
          this.snotify.error(MessageConstants.SYSTEM_ERROR_MSG, CaptionConstants.ERROR);
        }
      })
  }

  save(type?: string) {
    this.spinnerService.show();
    if (this.param.type !== 'edit') {
      this.layoutDesignOverallService.create(this.param).subscribe({
        next: (res) => {
          this.spinnerService.hide();
          if (res.success) {
            this.snotify.success(MessageConstants.CREATED_OK_MSG, ActionConstants.CREATE);
            if (type == 'next')
              this.resetForm()
            else
              this.back()
          }
          else {
            if (!res.success && res.caption != null) {
              this.snotify.error(MessageConstants.CREATED_ERROR_MSG, CaptionConstants.ERROR);
            }
            else {
              this.snotify.error(res.message, CaptionConstants.ERROR);
            }
          }
        },
        error: () => {
          this.spinnerService.hide();
          this.snotify.error(MessageConstants.SYSTEM_ERROR_MSG, CaptionConstants.ERROR);
        }
      })
    } else {
      this.param.c2b_overall_image =
        this.image.url.includes('base64')
          ? this.image.url
          : "";

      this.layoutDesignOverallService.update(this.param).subscribe({
        next: (res) => {
          this.spinnerService.hide();
          this.back()
          this.snotify.success(
            MessageConstants.UPDATED_OK_MSG,
            ActionConstants.EDIT
          );
        },
        error: () => {
          this.spinnerService.hide();
          this.snotify.error(MessageConstants.SYSTEM_ERROR_MSG, CaptionConstants.ERROR);
        }
      })
    }
  }

  back() {
    this.router.navigate([
      "/best-line/transaction/layout-design-overall/main",
    ]);
  }
  getAllLineNo() {
    this.layoutDesignOverallService.getAllLineNo().subscribe({
      next: (res) => {
        this.lineNoList = res.map((item) => {
          return { id: item.line_id, text: item.line_name };
        });
        this.spinnerService.hide();
      },
      error: () => {
        this.spinnerService.hide();
        this.snotify.error(MessageConstants.SYSTEM_ERROR_MSG, CaptionConstants.ERROR);
      }
    })
  }
  getAllLineType() {
    this.layoutDesignOverallService.getAllLineType().subscribe({
      next: (res) => {
        this.lineTypeList = res.map((item) => {
          return {
            id: item.line_type_id,
            text: item.line_type_name,
          };
        });
        this.spinnerService.hide();
      },
      error: () => {
        this.spinnerService.hide();
        this.snotify.error(MessageConstants.SYSTEM_ERROR_MSG, CaptionConstants.ERROR);
      }
    })
  }
  getAllModelNo() {
    this.layoutDesignOverallService.getAllModelNo().subscribe({
      next: (res) => {
        this.modelNoList = res.map((item) => {
          return {
            id: item.model_no,
            text: item.model_no,
            additional: item.model_name,
          };
        });
        this.spinnerService.hide();
      },
      error: () => {
        this.spinnerService.hide();
        this.snotify.error(MessageConstants.SYSTEM_ERROR_MSG, CaptionConstants.ERROR);
      }
    })
  }
  changeModelName(e: any) {
    this.param.model_name = this.modelNoList.find(x => x.id == e)?.additional
  }

  resetForm() {
    this.param = <BLLayoutDesignOverallDetail>{
      line_id: '',
      line_type_id: '',
      model_no: '',
      c2b_overall_image: ''
    };
    this.image.reset();
  }
  onSelectFile(event) {
    if (event.target.files && event.target.files[0]) {
      const reader = new FileReader();
      reader.readAsDataURL(event.target.files[0]);
      const file = event.target.files[0];
      const title = event.target.files[0].name.split(".").pop();
      const fileZise = event.target.files[0].size;
      if (
        title === "jpg" ||
        title === "jpeg" ||
        title === "png" ||
        title === "JPG" ||
        title === "JPEG" ||
        title === "PNG"
      ) {
        if (fileZise <= 5242880) {
          reader.onload = (e) => {
            this.param.c2b_overall_image = e.target.result.toString()
          };
        } else {
          this.snotify.error(
            MessageConstants.FILE_IMAGE_SIZE,
            CaptionConstants.ERROR
          );
        }
      } else {
        return this.snotify.warning(
          MessageConstants.INVALID_FILE + "'.jpg', '.jpeg' or '.png'"
        );
      }
    }
  }
}
