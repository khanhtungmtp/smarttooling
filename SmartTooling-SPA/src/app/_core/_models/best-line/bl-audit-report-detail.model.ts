export interface BLAuditReportDetailDTO {
  audit_date: string;
  line_name: string;
  line_type_name: string;
  audit_count: number;
  model_no: string;
  model_name: string;
  stage_name: string;
  operation_name_local: string;
  operation_name_en: string;
  operation_name_zh: string;
  audit_result_line_is_pass: boolean;
  audit_result_operation_is_pass: boolean;
  operation_gap_description: string;
}
