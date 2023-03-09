using System;

namespace SmartTooling_API.DTO.BestLine
{
    public class BL_RolloutReportDTO
    {
        public string line_name { get; set; }
        public DateTime rollout_date { get; set; }
        public string model_no { get; set; }
        public string model_name { get; set; }
        public string stage_id { get; set; }
        public string operation_name_local { get; set; }
        public string operation_name_en { get; set; }
        public string operation_name_zh { get; set; }
        public int takt_time { get; set; }
        public decimal ct_after_sec { get; set; }
        public int? mp_allocated { get; set; }
        public string hourly_output { get; set; }
        public int? machines_qty { get; set; }
        public int? tool_qty { get; set; }
        public string operation_descriptions { get; set; }
        public string operation_video_url { get; set; }
        public string rollout_operation_layout { get; set; }

        public string operation_video_url_result { get; set; }
        public string rollout_operation_layout_result { get; set; }
        public int criticalOperationsTotal { get; set; }
        public int countBeingAudit {get;set;}
    }
}