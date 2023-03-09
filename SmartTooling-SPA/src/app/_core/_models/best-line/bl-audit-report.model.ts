export interface BLAuditReportDTO {
  factory_id: string;
  line_id: string;
  line_name: string;
  line_type_id: string;
  model_no: string;
  model_name: string;
  stage_name: string;
  stage_id: string;
  rollout_line_id: string;
  operation_id: string;
  operation_name_local: string;
  operation_name_en: string;
  operation_name_zh: string;
  rollout_date: string;
  first_audit_date: string;
  latest_audit_date: string;
  audit_conducted: number;
  layout_audit: boolean;
  audit_pass: number;
  audit_fail: number;
  countBeingAudit: number;
}
