import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { PilotLineSetupSummaryTrackingGroupReport } from '../../_models/smart-tool/pilot-line-setup-summary-tracking-report';

@Injectable({
  providedIn: 'root'
})
export class PilotLineReportService {

  baseUrl = `${environment.apiUrl}PilotLineSetupSummaryTracking`;
  constructor(private http: HttpClient) { }

  /**
   * Lấy danh sách Prod.Season
   */
  getSeasons(): Observable<string[]> {
    return this.http.get<string[]>(`${this.baseUrl}/GetSeason`);
  }

  /**
  * Lấy dữ liệu báo cáo Pilot Line Setup Summary Tracking
  */
  getAllPilots(prodSeason: string): Observable<PilotLineSetupSummaryTrackingGroupReport[]> {
    return this.http.get<PilotLineSetupSummaryTrackingGroupReport[]>(`${this.baseUrl}/GetAllPilots`, { params: { prodSeason } });
  }


  exportExcel(prodSeason: string): Observable<Blob> {
    return this.http.get(`${this.baseUrl}/exportExcel`, { params: { prodSeason }, responseType: "blob" });
  }
}
