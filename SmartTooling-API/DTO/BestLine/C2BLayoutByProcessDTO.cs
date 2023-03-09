using System;

namespace SmartTooling_API.DTO.BestLine
{
    public class C2BLayoutByProcessDTO
    {
        public string factory_id { get; set; }
        public string line_id { get; set; }
        public string line_name { get; set; }
        public string line_type_id { get; set; }
        public string line_type_name { get; set; }
        public string model_no { get; set; }
        public string model_name { get; set; }
        public string process_type_id { get; set; }
        public string process_type_name_en { get; set; }
        public string update_by { get; set; }
        public DateTime update_time { get; set; }
    }
}