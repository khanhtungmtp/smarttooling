namespace SmartTooling_API.DTO.BestLine
{
    public class CriticalProcessReportDTO
    {
        public string factory_id { get; set; }
        public string line_id { get; set; }
        public string line_name { get; set; }
        public string line_type_id { get; set; }
        public string line_type_name { get; set; }
        public string model_no { get; set; }
        public string model_name { get; set; }
        public string stage_id { get; set; }
        public string stage_name { get; set; }
        public string operation_id { get; set; }
        public string operation_name_zh { get; set; }
        public string operation_name_en { get; set; }
        public string operation_name_local { get; set; }

    }
}