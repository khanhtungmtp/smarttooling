export interface CriticalProcessReportKaizenDTO {
    factory_id: string;
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
    serial_no: number | null;
    kaizen_description: string;
    start_date: string | null;
    process_type_name_en: string;
    process_tct_sec: number | null;
    ct_before_sec: number | null;
    ct_after_sec: number | null;
    rft_before_percent: number | null;
    rft_after_percent: number | null;
    line_roll_out_percent: number | null;
    clicks_times: number | null;
    improv: number | null;
}