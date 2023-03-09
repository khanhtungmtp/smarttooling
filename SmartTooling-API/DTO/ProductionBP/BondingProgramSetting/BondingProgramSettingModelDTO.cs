using System;

namespace SmartTooling_API.DTO.ProductionBP.BondingProgramSetting
{
    public class BondingProgramSettingModelDTO
    {
        public string factory_id { get; set; }
        public string model_no { get; set; }
        public string model_name { get; set; }
        public string chemical_process_type_id { get; set; }
        public string chemical_process_type_name { get; set; }
        public string auto_tech_id { get; set; }
        public string auto_tech_name { get; set; }
        public string chemical_name { get; set; }
        public string adoption_component_id { get; set; }
        public string adoption_component_name { get; set; }
        public string chemical_supplier_id { get; set; }
        public string process_adoption_scope_id { get; set; }
        public string process_adoption_scope_name { get; set; }
        public DateTime first_month_of_production_adoption { get; set; }
        public string main_upper_material_type_id { get; set; }
        public string main_bottom_material_type_id { get; set; }
        public bool article_no_is_general { get; set; }
        public string article_no_remarks { get; set; }
        public string shoes_photo_url { get; set; }
        public string create_by { get; set; }
        public DateTime create_time { get; set; }
        public string update_by { get; set; }
        public DateTime update_time { get; set; }
        public string dev_season { get; set; }
        public string production_season { get; set; }
    }
}
