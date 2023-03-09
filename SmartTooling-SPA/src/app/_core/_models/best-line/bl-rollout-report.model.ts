export interface BLRolloutReportModel {
  line_name: string;
  rollout_date: string;
  model_no: string;
  model_name: string;
  stage_id: string;
  operation_name_local: string;
  operation_name_en: string;
  operation_name_zh: string;
  takt_time: number;
  ct_after_sec: number;
  mp_allocated: number | null;
  hourly_output: string;
  machines_qty: number | null;
  tool_qty: number | null;
  operation_descriptions: string;
  operation_video_url: string;
  rollout_operation_layout: string;

  operation_video_url_result: string;
  rollout_operation_layout_result: string;
  countBeingAudit: number;
  criticalOperationsTotal: number;
}
