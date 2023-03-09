export interface BLCriticalProcess {
    factory_id: string
    update_by: string;
    update_time: string;
    line_id: string;
    line_type_id: string;
    model_no: string;
    stage_id: string;
    line_type_name: string;
    line_name: string;
    stage_name: string;
    operation_name_local: string;
    operation_name_en: string;
    operation_name_zh: string;
    operation_id: string;
    man_remarks: string;
    man_media_url: string;
    machine_remarks: string;
    machine_media_url: string;
    method_remarks: string;
    method_media_url: string;
    material_remarks: string;
    material_media_url: string;
    takt_time: number;
    best_practice_url: string;
    ct_before_sec: number;
    ct_after_sec: number;

}
