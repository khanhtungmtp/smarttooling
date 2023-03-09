using System;

namespace SmartTooling_API.DTO.BestLine
{
    public class BL_Line_TypeDTO
    {
        public string factory_id { get; set; }
        public string line_type_id { get; set; }
        public string line_type_name { get; set; }
        public int sequence { get; set; }
        public bool is_active { get; set; }
        public string update_by { get; set; }
        public DateTime update_time { get; set; }
    }
}