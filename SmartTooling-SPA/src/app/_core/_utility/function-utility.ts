import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root',
})
export class FunctionUtility {

    /**
     *Hàm tiện ích
     */
    constructor() { }

    /**
     * Nhận vào DateTime
     * Trả ra ngày không lấy giờ
     */
    returnDayNotTime(date: Date) {
        debugger
        const tmp = Date.UTC(date.getFullYear(), date.getMonth(), date.getDate(), 0, 0, 0);
        return new Date(tmp);
    }

    /**
     * Nhận vào DateTime
     * Trả ra chuỗi chỉ có ngày tháng
     */
    returnStringDay(date: Date) {
        const tmp = String(date).substr(0, 15);
        return tmp;
    }

    /**
     * Nhận vào DateTime
     * Trả ra chuỗi có ngày tháng giờ phút mà ko lấy múi giờ
     */
    returnStringDateTime(date: Date) {
        const tmp = String(date).substr(0, 24);
        return tmp;
    }

    /**
     *Trả ra ngày với tham số truyền vào là ngày muốn format, chỉ lấy năm tháng ngày: yyyy/MM/dd
     */
    getDateFormat(day: Date) {
        const dateFormat = day.getFullYear().toString() +
            '/' + (day.getMonth() + 1).toString() +
            '/' + day.getDate().toString();
        return dateFormat;
    }

    /**
     *Trả ra ngày đầu và cuối tháng hiện tại, chỉ lấy năm tháng ngày: yyyy/MM/dd
     */
    getTheFirstDayAndLastDayOfTheThisMonth() {
        const date = new Date();
        const firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
        const lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);
        return { firstDay: this.getDateFormat(firstDay), lastDay: this.getDateFormat(lastDay) };
    }
    
    
    /**
     * Nhập vào kiểu chuỗi hoặc số dạng 123456789 sẽ trả về 123,456,789
     */
    convertNumber(amount) {
        return String(amount).replace(/(?<!\..*)(\d)(?=(?:\d{3})+(?:\.|$))/g, '$1,')
    }

    /**
     * Check String Empty Or Null Or undefined
     */
    isEmpty(str) {
        return (!str || /^\s*$/.test(str));
    }

    /**
     * Thay thế kí tự Enter (xuống dòng) bằng thẻ <br> để show html
     */
    replaceLineBreak(str: string) {
        return str.replace(/(?:\r\n|\r|\n)/g, '<br>');
    }
}
