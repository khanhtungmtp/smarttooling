import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { RoleByUser } from '../_models/role-by-user';
import { OperationResult } from '../_models/smart-tool/operation-result';
import { PaginatedResult } from '../_models/smart-tool/pagination';
import { AddUser } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getUsers(account: string, isActive: string, page?, itemsPerPage?): Observable<PaginatedResult<AddUser[]>> {
    const paginatedResult: PaginatedResult<AddUser[]> = new PaginatedResult<AddUser[]>();
    let params = new HttpParams();

    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }
    isActive = isActive === 'all' ? '' : isActive;
    return this.http.get<any>(this.baseUrl + 'user?account=' + account + '&isActive=' + isActive, { observe: 'response', params })
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

  addUser(addUser: AddUser) {
    return this.http.post(this.baseUrl + 'User', addUser);
  }

  updateUser(updateUser: AddUser) {
    return this.http.post(this.baseUrl + 'User/update', updateUser);
  }
  getRoleByUser(account: string) {
    return this.http.get<RoleByUser[]>(this.baseUrl + 'User/roleuser/' + account);
  }
  updateRoleByUser(account: string, listRoleByUser: RoleByUser[]) {
    return this.http.post(this.baseUrl + 'User/roleuser/' + account, listRoleByUser);
  }

  changePassword(account: string, oldPassword: string, password: string) {
    const user = {
      Account: account,
      OldPassword: oldPassword,
      Password: password
    };
    return this.http.post<OperationResult>(this.baseUrl + 'User/changepassword', user);
  }
}
