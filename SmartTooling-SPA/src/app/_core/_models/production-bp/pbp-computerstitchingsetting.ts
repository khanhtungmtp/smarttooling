export interface PbpComputerstitchingsetting {
  factory_id: string;
  model_no: string;
  stage_id: string;
  operation_id: string;
  cs_type_id: string;
  cs_machine_type_id: string;
  sop_setup: boolean;
  production_adoption: boolean;
  is_critical_process: boolean;
  main_upper_material_type_id: string;
  cs_machine_model: string;
  cs_speed_setting_rpm: number;
  number_of_size_group: number;
  jig_design_id: string;
  dev_season: string;
  production_season: string;
  article_no_is_general: boolean;
  article_no_remarks: string;
  jig_photo_url: string;
  cs_video_url: string;
  create_by: string;
  create_time: string;
  update_by: string;
  update_time: string;
  model_name: string;
  stage_name: string;
  operation_name_local: string;
  operation_name_en: string;
  operation_name_zh: string;
  cs_type_name: string;
  cs_machine_type_name: string;
  main_upper_material_type_name: string;
  jig_design_name: string;
  sop_setupResult: string;
  production_adoptionResult: string;
  is_critical_processResult: string;
  article_no_is_generalResult: string;
  jig_photo_urlResult: string;
  cs_video_urlResult: string;
}
