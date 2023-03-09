export interface BL_Rollout_Audit {
  factory_id: string;
  rollout_line_id: string;
  line_type_id: string;
  model_no: string;
  stage_id: string;
  operation_id: string;
  audit_count: number;
  audit_date: Date;
  audit_result_line_is_pass: boolean;
  audit_result_operation_is_pass: boolean;
  operation_gap_description: string;
  update_by: string;
  update_time: Date;
  create_by: string;
  create_time: Date;
  //
  line_id: string;
  line_name: string;
  line_type_name: string;
  model_name: string;
  stage_name: string;
  operation_name_local: string;
  operation_name_en: string;
  operation_name_zh: string;
  audit_date_today: string
}
