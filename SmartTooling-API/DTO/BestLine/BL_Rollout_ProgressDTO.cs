using System;

namespace SmartTooling_API.DTO.BestLine
{
    public class BL_Rollout_ProgressDTO
    {
        public string factory_id { get; set; }
        public string line_id { get; set; }
        public string line_type_id { get; set; }
        public string model_no { get; set; }
        public string stage_id { get; set; }
        public string operation_id { get; set; }
        public string rollout_line_id { get; set; }
        public DateTime rollout_date { get; set; }
        public string rollout_date_convert { get; set; }
        public int? mp_allocated { get; set; }
        public string machines_name { get; set; }
        public int? machines_qty { get; set; }
        public string tool_name { get; set; }
        public int? tool_qty { get; set; }
        public string operation_descriptions { get; set; }
        public string operation_video_url { get; set; }
        public string rollout_operation_layout { get; set; }
        public string create_by { get; set; }
        public DateTime create_time { get; set; }
        public string update_by { get; set; }
        public DateTime update_time { get; set; }

        //
        public string line_name { get; set; }
        public string line_type_name { get; set; }
        public string stage_name { get; set; }
        public string rollout_line_name { get; set; }
        public string operation_name_local { get; set; }
        public string operation_name_en { get; set; }
        public string operation_name_zh { get; set; }
        public int takt_time { get; set; }
        public decimal ct_after_sec { get; set; }
        public decimal? hourly_output { get; set; }
        public decimal pph { get; set; }
    }
}