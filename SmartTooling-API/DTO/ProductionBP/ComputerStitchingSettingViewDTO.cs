using System;

namespace SmartTooling_API.DTO.ProductionBP
{
    public class ComputerStitchingSettingViewDTO
    {
        public string factory_id { get; set; }
        public string model_no { get; set; }
        public string model_name { get; set; }
        public string stage_id { get; set; }
        public string stage_name { get; set; }
        public string operation_id { get; set; }
        public string operation_name_local { get; set; }
        public string operation_name_en { get; set; }
        public string operation_name_zh { get; set; }
        public string cs_type_id { get; set; }
        public string cs_type_name { get; set; }
        public string dev_season { get; set; }
        public string production_season { get; set; }
        public string cs_machine_type_id { get; set; }
        public string cs_machine_type_name { get; set; }
        public bool sop_setup { get; set; }
        public bool production_adoption { get; set; }
        public bool is_critical_process { get; set; }
        public int cs_speed_setting_rpm { get; set; }
        public string jig_photo_url { get; set; }
        public string cs_video_url { get; set; }
        public string main_upper_material_type_id { get; set; }
        public string main_upper_material_type_name { get; set; }
        public bool article_no_is_general { get; set; }
        public string article_no_remarks { get; set; }
        public string cs_machine_model { get; set; }
        public string jig_design_id { get; set; }
        public string jig_design_name { get; set; }
        public int number_of_size_group { get; set; }
        public string create_by { get; set; }
        public DateTime create_time { get; set; }
        public string update_by { get; set; }
        public DateTime update_time { get; set; }


        public string sop_setupResult { get; set; }
        public string production_adoptionResult { get; set; }
        public string is_critical_processResult { get; set; }
        public string article_no_is_generalResult { get; set; }

    }
}