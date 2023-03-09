export interface MeasumentRFT {
  factory_id: string;
  model_no: string;
  stage_id: string;
  operation_id: string;
  total_produced_qty: number;
  defect_qty: number;
  defect_reason_id: string;
  rft_percent: number;
  defect_pic: string;
  inspector: string;
  create_by: string;
  create_time: Date;
  update_by: string;
  update_time: Date;
}
