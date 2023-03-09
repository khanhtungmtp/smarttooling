export class Kaizen {
    factory_id: string;
    model_no: string;
    serial_no: number;
    kaizen_description: string;
    stage_id: string;
    operation_id: string;
    start_date: Date;
    kaizen_type_eliminate: boolean;
    kaizen_type_reduce: boolean;
    kaizen_type_combine: boolean;
    kaizen_type_smart_tool: boolean;
    process_tct_sec: number;
    ct_before_sec: number;
    ct_after_sec: number;
    rft_before_percent: number;
    rft_after_percent: number;
    line_roll_out_percent: number;
    before_media: string;
    after_media: string;
    before_remarks: string;
    after_remarks: string;
    kaizen_from: string;
    clicks_times: number
    create_by: string;
    create_time: Date
    update_by: string;
    update_time: Date
    process: string;

    before_media_result: string;
    after_media_result: string;
}