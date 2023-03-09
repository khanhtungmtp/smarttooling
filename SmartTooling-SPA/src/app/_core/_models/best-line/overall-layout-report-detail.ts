export interface C2BOverallLayoutReportDetail {
    factory_id: string;
    line_name: string;
    line_type_name: string;
    model_no: string;
    model_name: string;
    line_id: string;
    line_type_id: string;
    no_of_process_before: number | null;
    no_of_process_after: number | null;
    tct_before: number;
    tct_after: number;
    cps_mp_before: number;
    cps_mp_after: number;
    assembly_mp_before: number;
    assembly_mp_after: number;
    eolr_before: number;
    eolr_after: number;
    ller_before_percent: number;
    ller_after_percent: number;
    tentative_pph_before: number;
    tentative_pph_after: number;
    tentative_efficiency_before_percent: number | null;
    tentative_efficiency_after_percent: number | null;
    c2b_overall_image: string;
    update_by: string;
    update_time: string;

    c2b_overall_image_result: string;
}
