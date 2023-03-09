using System;
namespace SmartTooling_API.DTO.SmartTool
{
    public class KaizenDTO
    {
        public string factory_id { get; set; }
        public string model_no { get; set; }
        public int serial_no { get; set; }
        public string kaizen_description { get; set; }
        public string stage_id { get; set; }
        public string operation_id { get; set; }
        public DateTime? start_date { get; set; }
        public bool kaizen_type_eliminate { get; set; }
        public bool kaizen_type_reduce { get; set; }
        public bool kaizen_type_combine { get; set; }
        public bool kaizen_type_smart_tool { get; set; }
        public decimal process_tct_sec { get; set; }
        public decimal ct_before_sec { get; set; }
        public decimal ct_after_sec { get; set; }
        public decimal? rft_before_percent { get; set; }
        public decimal? rft_after_percent { get; set; }
        public decimal line_roll_out_percent { get; set; }
        public string before_media { get; set; }
        public string after_media { get; set; }
        public string before_remarks { get; set; }
        public string after_remarks { get; set; }
        public string kaizen_from { get; set; }
        public int clicks_times { get; set; }
        public string create_by { get; set; }
        public DateTime? create_time { get; set; }
        public string update_by { get; set; }
        public DateTime? update_time { get; set; }
        public string process { get; set; }
        public KaizenDTO()
        {
            update_time = DateTime.Now;
        }

        public string before_media_result { get; set; }
        public string after_media_result { get; set; }
    }
}