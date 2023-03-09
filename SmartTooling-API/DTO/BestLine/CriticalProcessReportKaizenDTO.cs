using System;

namespace SmartTooling_API.DTO.BestLine
{
    public class CriticalProcessReportKaizenDTO
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
        public int? serial_no { get; set; }
        public string kaizen_description { get; set; }
        public DateTime? start_date { get; set; }
        public string process_type_name_en { get; set; }
        public decimal? process_tct_sec { get; set; }
        public decimal? ct_before_sec { get; set; }
        public decimal? ct_after_sec { get; set; }
        public decimal? rft_before_percent { get; set; }
        public decimal? rft_after_percent { get; set; }
        public decimal? line_roll_out_percent { get; set; }
        public int? clicks_times { get; set; }

    }
}