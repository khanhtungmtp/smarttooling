using System;

namespace SmartTooling_API.DTO.SmartTool
{
    public class KaizenModelDetail
    {
        public string factory_id { get; set; }
        public string model_no { get; set; }
        public int serial_no { get; set; }
        public string kaizen_description { get; set; }
        public string stage_id { get; set; }
        public string process_type_id { get; set; }
        public string operation_id { get; set; }
        public DateTime? start_date { get; set; }
        public decimal process_tct_sec { get; set; }
        public decimal ct_before_sec { get; set; }
        public decimal ct_after_sec { get; set; }
        public decimal improv { get; set; }
        public decimal? rft_before_percent { get; set; }
        public decimal? rft_after_percent { get; set; }
        public decimal line_roll_out_percent { get; set; }
        public int clicks_times { get; set; }
        public int? No { get; set; }
    }
}