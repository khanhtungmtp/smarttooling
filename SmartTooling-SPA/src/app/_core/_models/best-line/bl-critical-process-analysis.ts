export interface BLCriticalProcessAnalysis {
    factory_id: string;
    line_id: string;
    line_type_id: string;
    model_no: string;
    stage_id: string;
    operation_id: string;
    takt_time: number;
    best_practice_url: string;
    ct_before_sec: number;
    ct_after_sec: number;
    man_remarks: string;
    man_media_url: string;
    machine_remarks: string;
    machine_media_url: string;
    method_remarks: string;
    method_media_url: string;
    material_remarks: string;
    material_media_url: string;
    update_by: string;
    update_time: string;
    invalid: string;
}