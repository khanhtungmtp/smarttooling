namespace SmartTooling_API.DTO.ProductionBP
{
    public class PBP_Pad_Print_SettingDTO
    {
        public string factory_id { get; set; }
        public string model_no { get; set; }
        public string model_name { get; set; }
        public string component_name { get; set; }
        public string material_type_id { get; set; }
        public string material_type_name { get; set; }
        public string chemical_ink { get; set; }
        public string dev_season { get; set; }
        public string production_season { get; set; }
        public string material_description { get; set; }
        public string pad_shape_id { get; set; }
        public string pad_shape_name { get; set; }
        public int number_of_pad_hits { get; set; }
        public string machine_vendor_id { get; set; }
        public string machine_vendor_name { get; set; }
        public string machine_model { get; set; }
        public bool is_rotary_table_used { get; set; }
        public string is_rotary_table_used_result { get; set; }
        public string chemical_hardener { get; set; }
        public string chemical_additive { get; set; }
        public string chemical_primer { get; set; }
        public string chemical_others { get; set; }
        public bool article_no_is_general { get; set; }
        public string article_no_is_general_result { get; set; }
        public string article_no_remarks { get; set; }
        public string component_photo_url { get; set; }
        public string component_photo_url_result { get; set; }
        public string operation_video_url { get; set; }
        public string operation_video_url_result { get; set; }
        public string component_photo_url_file { get; set; }
        public string operation_video_url_file { get; set; }
        public string create_by { get; set; }
        public DateTime create_time { get; set; }
        public string update_by { get; set; }
        public DateTime update_time { get; set; }
    }
}