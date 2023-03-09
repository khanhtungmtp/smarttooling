export interface PbpPadPrintSetting {
    factory_id: string;
    model_no: string;
    model_name: string;
    component_name: string;
    material_type_id: string;
    material_type_name: string;
    chemical_ink: string;
    dev_season: string;
    production_season: string;
    material_description: string;
    pad_shape_id: string;
    pad_shape_name: string;
    number_of_pad_hits: number;
    machine_vendor_id: string;
    machine_vendor_name: string;
    machine_model: string;
    is_rotary_table_used: boolean;
    is_rotary_table_used_result: string;
    chemical_hardener: string;
    chemical_additive: string;
    chemical_primer: string;
    chemical_others: string;
    article_no_is_general: boolean;
    article_no_is_general_result: string;
    article_no_remarks: string;
    component_photo_url: string;
    component_photo_url_result: string;
    operation_video_url: string;
    operation_video_url_result: string;
    component_photo_url_file: string;
    operation_video_url_file: string;
    create_by: string;
    create_time: string;
    update_by: string;
    update_time: string;
}