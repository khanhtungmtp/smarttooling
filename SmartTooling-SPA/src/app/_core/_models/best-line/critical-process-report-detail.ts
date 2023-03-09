export interface CriticalProcessReportDetailDTO {
    factory: string;
    line_id: string;
    line_name: string;
    line_type_id: string;
    line_type_name: string;
    model_no: string;
    model_name: string;
    stage_id: string;
    stage_name: string;
    operation_id: string;
    operation_name_zh: string;
    operation_name_en: string;
    operation_name_local: string;
    man_media_url: string;
    man_remarks: string;
    machine_media_url: string;
    machine_remarks: string;
    method_media_url: string;
    method_remarks: string;
    material_media_url: string;
    material_remarks: string;

    man_media_url_result: string;
    machine_media_url_result: string;
    method_media_url_result: string;
    material_media_url_result: string;
}