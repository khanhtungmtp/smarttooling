import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { Efficiency } from '../../_models/smart-tool/efficiency';
import { ModelEfficiencyEditParam } from '../../_models/smart-tool/efficiency-editParam';

@Injectable({
  providedIn: 'root'
})
export class ModelEfficiencyService {
baseUrl = environment.apiUrl;
constructor(private http: HttpClient) { }


getUpperID() {
  return this.http.get<any>(this.baseUrl + 'modelEfficiency/upperId', {});
}

getModelName(upperId: string) {
  return this.http.get<any>(this.baseUrl + 'modelEfficiency/modelName', { params : {
    upperId: upperId}});
}

getModelEfficiency(modelEfficiencyParram: ModelEfficiencyEditParam) {
  console.log("service", modelEfficiencyParram)
  return this.http.post<Efficiency[]>(this.baseUrl + 'modelEfficiency/getModelEfficiency/', modelEfficiencyParram);
}

updateModelEfficiency(modelEfficiency: any) {
  console.log("modelEF", modelEfficiency)
  return this.http.post(this.baseUrl + 'modelEfficiency/updateModelEfficiency/', modelEfficiency);
}}
