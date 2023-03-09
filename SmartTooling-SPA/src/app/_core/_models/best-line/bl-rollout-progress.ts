export interface BL_Rollout_Progress {
  factory_id: string;
  line_id: string;
  line_type_id: string;
  model_no: string;
  stage_id: string;
  operation_id: string;
  rollout_line_id: string;
  rollout_date: Date;
  rollout_date_convert: string;
  mp_allocated: number;
  machines_name: string;
  machines_qty: number;
  tool_name: string;
  tool_qty: number;
  operation_descriptions: string;
  operation_video_url: string;
  rollout_operation_layout: string;
  update_by: string;
  update_time: Date;
  //
  line_name: string;
  line_type_name: string;
  stage_name: string;
  operation_name_local: string;
  operation_name_en: string;
  operation_name_zh: string;
  takt_time: number;
  ct_after_sec: number;
  hourly_output: number;
  pph: number;
  rollout_line_name: string;
}
