export interface BLSearchAuditReportParam {
  factory: string;
  lineID: string;
  rolloutLineID: string;
  lineTypeID: string;
  modelNo: string;
  stageID: string;
  operationID: string;
  lang: string;
  flag: boolean;
  currentPage: number;
  countRolloutTotal: number;
  countBeingAudit: number;
}
