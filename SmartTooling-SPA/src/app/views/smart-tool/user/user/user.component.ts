import { Component, OnInit, ViewChild } from "@angular/core";
import { BsModalRef, ModalDirective, BsModalService } from "ngx-bootstrap/modal";
import { NgxSpinnerService } from "ngx-spinner";
import { CaptionConstants, MessageConstants } from "../../../../_core/_constants/system.constants";
import { RoleByUser } from "../../../../_core/_models/role-by-user";
import { Pagination } from "../../../../_core/_models/smart-tool/pagination";
import { AddUser } from "../../../../_core/_models/user";
import { NgSnotifyService } from "../../../../_core/_services/ng-snotify.service";
import { UserService } from "../../../../_core/_services/user.service";


@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss']
})
export class UserComponent implements OnInit {
  users: AddUser[] = [];
  account: string = '';
  isActive: string = 'all';
  pagination: Pagination = {
    currentPage: 1,
    itemsPerPage: 10,
    totalItems: 1,
    totalPages: 1,
  };
  addUser: AddUser = new AddUser();

  modalRef: BsModalRef;
  @ViewChild('authorizationModal', { static: false }) authorizationModal: ModalDirective;
  userAuthorizationAccount: string = '';
  userAuthorizationName: string = '';
  listRoleByUser: RoleByUser[] = [];

  editUser: AddUser = new AddUser();
  @ViewChild('modalEditUser', { static: false }) modalEditUser: ModalDirective;

  constructor(private userService: UserService,
    private spinnerService: NgxSpinnerService,
    private snotifyAlert: NgSnotifyService,
    private modalService: BsModalService) { }

  ngOnInit() {
    this.getUser();
  }

  getUser() {
    this.spinnerService.show();
    this.userService.getUsers(this.account, this.isActive, this.pagination.currentPage, this.pagination.itemsPerPage)
      .subscribe(res => {
        this.users = res.result;
        this.pagination = res.pagination;

        this.spinnerService.hide();
      });
  }

  search() {
    this.pagination.currentPage = 1;
    this.getUser();
  }

  saveAddUser() {
    this.spinnerService.show();
    this.userService.addUser(this.addUser)
      .subscribe(() => {
        this.snotifyAlert.success(MessageConstants.CREATED_OK_MSG, CaptionConstants.SUCCESS);
        this.spinnerService.hide();
        this.getUser();
      }, error => {
        this.snotifyAlert.error(MessageConstants.CREATED_ERROR_MSG, CaptionConstants.ERROR);
        this.spinnerService.hide();
      });
  }

  openModalAuthorization(account: string, name: string) {
    this.userAuthorizationAccount = account;
    this.userAuthorizationName = name;
    this.userService.getRoleByUser(this.userAuthorizationAccount).subscribe(res => {
      this.listRoleByUser = res;
      this.authorizationModal.show();
    });
  }

  saveAuthorizationUser() {
    const updateRoleByUser = this.listRoleByUser.filter(item => {
      return item.status === true;
    });
    this.spinnerService.show();
    this.userService.updateRoleByUser(this.userAuthorizationAccount, updateRoleByUser)
      .subscribe(() => {
        this.snotifyAlert.success(MessageConstants.UPDATED_OK_MSG, CaptionConstants.SUCCESS);
        this.spinnerService.hide();
        this.authorizationModal.hide();
      }, error => {
        this.snotifyAlert.error(MessageConstants.UPDATED_ERROR_MSG, CaptionConstants.ERROR);
        this.spinnerService.hide();
      });
  }

  saveEditUser() {
    this.spinnerService.show();
    this.userService.updateUser(this.editUser)
      .subscribe(() => {
        this.snotifyAlert.success(MessageConstants.UPDATED_OK_MSG, CaptionConstants.SUCCESS);
        this.spinnerService.hide();
        this.modalEditUser.hide();
        this.getUser();
      }, error => {
        this.snotifyAlert.error(MessageConstants.UPDATED_ERROR_MSG, CaptionConstants.ERROR);
        this.spinnerService.hide();
      });
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.getUser();
  }

  openModalEditUser(user: AddUser) {
    this.editUser = user;
    this.modalEditUser.show();
  }
}
