namespace SmartTooling_API.DTO.ProductionBP
{
    public class PBP_Pad_Print_Setting_DTO
    {
        public string factory_id { get; set; }
        public string model_no { get; set; }
        public string component_name { get; set; }
        public string material_type_id { get; set; }
        public string chemical_ink { get; set; }
        public string model_name { get; set; }
        public string article_no_remarks { get; set; }
        public string material_description { get; set; }
        public string pad_shape_name { get; set; }
        public int number_of_pad_hits { get; set; }
        public string dev_season { get; set; }
        public string production_season { get; set; }
    }
}