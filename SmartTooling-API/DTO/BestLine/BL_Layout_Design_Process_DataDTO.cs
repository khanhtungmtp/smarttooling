using System;

namespace SmartTooling_API.DTO.BestLine
{
    public class BL_Layout_Design_Process_DataDTO
    {
        public string factory_id { get; set; }
        public string line_id { get; set; }
        public string line_type_id { get; set; }
        public string model_no { get; set; }
        public string process_type_id { get; set; }
        public string node_name { get; set; }
        public string before_or_after { get; set; }
        public decimal cycle_time { get; set; }
        public decimal takt_time { get; set; }
        public decimal employee_qty { get; set; }
        public int sequence { get; set; }
        public string update_by { get; set; }
        public DateTime update_time { get; set; }
    }
}