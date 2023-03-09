namespace SmartTooling_API.DTO.SmartTool
{
    public class CrossSiteSharingEditDTO
    {
        public Kaizen_Benefits_Application_FormDTO crossSiteSharingDTO { get; set; }
        public bool kaizen_type_eliminate { get; set; }
        public bool kaizen_type_reduce { get; set; }
        public bool kaizen_type_combine { get; set; }
        public bool kaizen_type_smart_tool { get; set; }
        public string kaizen_description { get; set; }
        public string before_media { get; set; }
        public string after_media { get; set; }
        public decimal ct_before_sec { get; set; }
        public decimal ct_after_sec { get; set; }
        public decimal? rft_before_percent { get; set; }
        public decimal? rft_after_percent { get; set; }
        public string model_name { get; set; }
        public string operation_name_en { get; set; }
        public string operation_name_zh { get; set; }
        public string before_remarks { get; set; }
        public string after_remarks { get; set; }
    }
}