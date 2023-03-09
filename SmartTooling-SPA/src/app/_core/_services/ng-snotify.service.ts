import { Injectable } from '@angular/core';
import { SnotifyPosition, SnotifyService, SnotifyToastConfig } from 'ng-snotify';

@Injectable({ providedIn: 'root' })
export class NgSnotifyService {
  constructor(
    private snotifyService: SnotifyService
  ) { }

  getConfig(position: SnotifyPosition): SnotifyToastConfig {
    if(position === undefined) {
      position = null;
    }
    this.snotifyService.setDefaults({
      global: {
        newOnTop: true,
        maxAtPosition: 10,
        maxOnScreen: 10
      }
    });
    return {
      bodyMaxLength: 300,
      titleMaxLength: 100,
      backdrop: -1,
      position: position === null ? SnotifyPosition.rightTop : position,
      timeout: 3000,
      showProgressBar: true,
      closeOnClick: true,
      pauseOnHover: true
    };
  }

  success(body: string, title?: string, position?: SnotifyPosition) {
    this.snotifyService.success(body, title, this.getConfig(position));
  }

  info(body: string, title?: string, position?: SnotifyPosition) {
    this.snotifyService.info(body, title, this.getConfig(position));
  }

  error(body: string, title?: string, position?: SnotifyPosition) {
    this.snotifyService.error(body, title, this.getConfig(position));
  }

  warning(body: string, title?: string, position?: SnotifyPosition) {
    this.snotifyService.warning(body, title, this.getConfig(position));
  }

  simple(body: string, title?: string, position?: SnotifyPosition) {
    this.snotifyService.simple(body, title, this.getConfig(position));
  }

  confirm(body: string, title: string, okCallback: () => any) {
    const { timeout, closeOnClick, ...config } = this.getConfig(SnotifyPosition.centerCenter);
    this.snotifyService.confirm(body, title, {
      ...config,
      buttons: [
        { text: 'OK', action: toast => { this.snotifyService.remove(toast.id); okCallback(); }, bold: true },
        { text: 'Cancel', action: toast => { this.snotifyService.remove(toast.id); } }
      ]
    });
  }

  clear() {
    this.snotifyService.clear();
  }
}
