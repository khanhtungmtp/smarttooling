import { Component, ElementRef, Input, OnChanges, ViewChild } from '@angular/core';
import { SnotifyService } from 'ng-snotify';

@Component({
  selector: "media-upload",
  templateUrl: "./media-upload.component.html",
  styleUrls: ["./media-upload.component.css"],
})
export class MediaUploadComponent implements OnChanges {
  @Input() url: string;
  @Input() isImageOrVideo: boolean;
  @Input() acceptKey: string = '';
  noImage: string = '../../../../assets/img/no-image.jpg'
  @ViewChild('imageRef') imageRef: ElementRef;
  @ViewChild('videoRef') videoRef: ElementRef;
  @ViewChild('inputRef') inputRef: ElementRef;
  accpet: string = '';

  constructor(private snotifyAlert: SnotifyService) { }

  ngOnChanges() {
    this.url = this.url == '' ? this.noImage : this.url;
    this.accpet = this.acceptKey == 'image' ? 'image/jpg,image/png,image/jpeg' : this.acceptKey == 'video' ? 'video/mp4' : 'image/jpg,image/png,image/jpeg,video/mp4';

  }

  onSelectFile(event) {
    if (event.target.files && event.target.files[0]) {
      const reader = new FileReader();

      // check file name extension
      const fileNameExtension = event.target.files[0].name.split(".").pop();
      const fileZise = event.target.files[0].size;
      if (
        fileNameExtension !== "jpg" &&
        fileNameExtension !== "jpeg" &&
        fileNameExtension !== "png" &&
        fileNameExtension !== "JPG" &&
        fileNameExtension !== "JPEG" &&
        fileNameExtension !== "PNG" &&
        fileNameExtension !== "mp4" &&
        fileNameExtension !== "MP4"
      ) {
        this.snotifyAlert.warning(
          "Please select a file .jpg, .png, .jpeg .mp4"
        );
        return;
      }
      // Image cannot be larger than 5MB
      if (fileZise > 5242880 && (fileNameExtension === 'jpg' || fileNameExtension === 'jpeg' ||
        fileNameExtension === 'png' || fileNameExtension === 'JPG' ||
        fileNameExtension === 'JPEG' || fileNameExtension === 'PNG')
      ) {
        this.snotifyAlert.warning(
          "Please select a File image cannot be larger than 5MB"
        );
        return;
      }
      // Video cannot be larger than 20MB
      if (
        fileZise > 20971520 &&
        (fileNameExtension === "mp4" || fileNameExtension === "MP4")
      ) {
        this.snotifyAlert.warning(
          "Please select a File video cannot be larger than 20MB"
        );
        return;
      }

      reader.readAsDataURL(event.target.files[0]); // read file as data url
      reader.onload = (e) => {
        // called once readAsDataURL is completed
        this.url = e.target.result.toString();
        this.isImageOrVideo ? this.imageRef.nativeElement.attributes.src.nodeValue = this.url : this.videoRef.nativeElement.attributes.src.nodeValue = this.url;
        this.isImageOrVideo = this.chkImage(this.url);
      };
    }
  }

  chkImage(uploadPicture) {
    if (uploadPicture != null) {
      if (uploadPicture.includes("data:video/mp4;base64")) {
        return false;
      } else {
        return true;
      }
    } else {
      return true;
    }
  }

  reset() {
    if (this.imageRef != undefined)
      this.imageRef.nativeElement.attributes.src.nodeValue = this.noImage;
    if (this.videoRef != undefined && !this.isImageOrVideo)
      this.videoRef.nativeElement.attributes.src.nodeValue = '';
    if (this.videoRef != undefined && this.isImageOrVideo)
      this.videoRef.nativeElement.attributes.src.nodeValue = this.noImage;
    this.inputRef.nativeElement.value = "";
  }
}
