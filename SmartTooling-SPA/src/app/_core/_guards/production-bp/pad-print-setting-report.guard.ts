import { Injectable } from "@angular/core";
import { CanLoad, Router } from "@angular/router";
import { RoleByUser } from "../../_models/role-by-user";

@Injectable({
    providedIn: 'root'
})
export class PadPrintSettingReportGuard implements CanLoad {

    constructor(private router: Router) { }
    canLoad(): boolean {
        const user: RoleByUser = JSON.parse(localStorage.getItem('userSmartTooling'));
        if (user.role.includes("pbp.PadPrintSettingReport")) {
            return true;
        } else {
            this.router.navigate(['/dashboard']);
        }
    }
}
