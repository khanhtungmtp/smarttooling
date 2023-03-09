import { NgModule } from "@angular/core";
import { AlertModule } from 'ngx-bootstrap/alert';
import { KaizenRFTGuard } from "../../../_core/_guards/smart-tool/kaizen-rft.guard";
import { KaizenGuard } from "../../../_core/_guards/smart-tool/kaizen.guard";
import { KaizenRFTModule } from './kaizen-rft/kaizen-rft.moudule';
import { KaizenRoutingModule } from './kaizen-routing.module';
import { KaizenChirlModule } from './kaizen/kaizen-chirl.module';
@NgModule({
  declarations: [],
  imports: [
    KaizenChirlModule,
    KaizenRFTModule,
    KaizenRoutingModule,
    AlertModule.forRoot(),
  ],
  providers:[
    KaizenGuard,
    KaizenRFTGuard,
  ]
})
export class KaizenModule { }
