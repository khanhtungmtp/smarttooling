import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ServerInfo } from '../_models/server-info';
import { ModelList } from '../_models/smart-tool/modelList';
import { KeyValuePair } from '../_utility/key-value-pair';

@Injectable({
  providedIn: 'root'
})
export class SharedResourcesService {
  apiUrl: string = environment.apiUrl;
  constructor(
    private http: HttpClient,
  ) { }

  getAllFactory() {
    return this.http.get<KeyValuePair[]>(`${this.apiUrl}SharedResources/getAllFactory`);
  }

  getAllLine() {
    return this.http.get<KeyValuePair[]>(`${this.apiUrl}SharedResources/GetAllLine`);
  }

  getAllLineType() {
    return this.http.get<KeyValuePair[]>(`${this.apiUrl}SharedResources/GetAllLineType`);
  }

  getAllModel() {
    return this.http.get<ModelList[]>(`${this.apiUrl}SharedResources/GetAllModel`);
  }

  getStage() {
    return this.http.get<KeyValuePair[]>(`${this.apiUrl}SharedResources/GetStage`);
  }

  getServerInfo(): Observable<ServerInfo> {
    return this.http.get<ServerInfo>(this.apiUrl + "SharedResources/ServerInfo");
  }
}
