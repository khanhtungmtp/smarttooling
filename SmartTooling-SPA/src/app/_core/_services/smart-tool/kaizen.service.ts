import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '../../../../environments/environment';
import { Kaizen } from '../../_models/smart-tool/kaizen';
import { OperationResult } from '../../_models/smart-tool/operation-result';
import { PaginatedResult } from '../../_models/smart-tool/pagination';
@Injectable({
  providedIn: 'root'
})
export class KaizenService {
  baseUrl = environment.apiUrl;
  modelNoSource = new BehaviorSubject<string>("");
  currentModelNo = this.modelNoSource.asObservable();
  modelNameSource = new BehaviorSubject<string>("");
  currentModelName = this.modelNameSource.asObservable();
  kaizenSource = new BehaviorSubject<Object>({});
  currentKaizen =this.kaizenSource.asObservable();
  modelNoRftSource = new BehaviorSubject<string>('');
  constructor(private http: HttpClient) { }
  search(page?, modelNO?): Observable<PaginatedResult<Kaizen[]>> {
    const paginatedResult: PaginatedResult<Kaizen[]> = new PaginatedResult<Kaizen[]>();
    let params = new HttpParams();
    params = params.append('pageNumber', page);
    params = params.append('pageSize', "10");
    params = params.append('modelNo', modelNO == 'All' ? '' : modelNO);

    return this.http.get<Kaizen[]>(this.baseUrl + 'kaizen/kaizen-list', { observe: 'response', params })
      .pipe(
        map(response => {
          paginatedResult.result = response.body;
          if (response.headers.get('Pagination') != null) {
            paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
          }
          return paginatedResult;
        }),
      );
  }
  geDataModelNo()
  {
    return this.http.get<any>(this.baseUrl + 'kaizen/getmodel');
  }
  changeModel( model_no:string,model_name:string ){
    this.modelNoSource.next(model_no);
    this.modelNameSource.next(model_name);
  }
  create(kaizen: Kaizen) {
    console.log("Service: ", kaizen);
    return this.http.post<OperationResult>(this.baseUrl + 'kaizen/create', kaizen);
  }
  update(kaizen: Kaizen) {
    console.log("Service: ", kaizen);
    return this.http.post<OperationResult>(this.baseUrl + 'kaizen/update', kaizen);
  }
  getDataStage(){
    return this.http.get<any>(this.baseUrl + 'kaizen/getstage');
  }
  getProcess(modelNO:string,stage:string){
    return this.http.get<any>(this.baseUrl + 'kaizen/getprocess', {  params :{modelNo:modelNO,stage:stage}})
    ;
  }
  getOpera(modelNO:string,stage:string,process:string){
    return this.http.get<any>(this.baseUrl + 'kaizen/getopera', {params :{modelNo:modelNO,stage:stage,process:process}});
  }
  getKaizenFrom(){
    return this.http.get<any>(this.baseUrl + 'kaizen/getkaizenfrom');
  }
  changeKaizen(kaizen:Kaizen)
  {
    this.kaizenSource.next(kaizen);
  }
  getKaizenEdit(modelNO:string,serialNo:string){
    return this.http.get<Kaizen[]>(this.baseUrl + 'kaizen/getKaizenEdit', {params :{modelNo:modelNO,serialNo:serialNo}});
  }
}
