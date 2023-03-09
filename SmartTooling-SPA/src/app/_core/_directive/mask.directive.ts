import {
  Directive,
  ElementRef,
  HostListener,
  Input,
  Optional,
} from '@angular/core';
import { NgControl } from '@angular/forms';

@Directive({
  selector: '[mask]'
})
export class MaskDirective {
  @Input()
  set mask(value) {
    this.regExpr = new RegExp(value);
  }

  private _oldValue: string = "";
  private regExpr: RegExp;
  constructor(@Optional() private control: NgControl) { }
  @HostListener('input', ['$event'])
  change($event) {
    let item = $event.target;
    let value = item.value;
    let pos = item.selectionStart; //vị trí của kí tự vừa nhập
    let matchvalue = value;
    let noMatch: boolean = value && !this.regExpr.test(matchvalue);
    if (noMatch) {
      if (pos - 1 == 0) {
        this._oldValue = ''; //temp fix for saveAndNext
      }
      if (this.control)
        this.control.control.setValue(this._oldValue, { emit: false }); //set back last good string
      item.value = this._oldValue;
    } else this._oldValue = value;
  }
}
