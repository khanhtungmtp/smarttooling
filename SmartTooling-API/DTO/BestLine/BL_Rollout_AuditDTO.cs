using System;

namespace SmartTooling_API.DTO.BestLine
{
    public class BL_Rollout_AuditDTO
    {
        public string factory_id { get; set; }
        public string rollout_line_id { get; set; }
        public string line_type_id { get; set; }
        public string model_no { get; set; }
        public string stage_id { get; set; }
        public string operation_id { get; set; }
        public int audit_count { get; set; }
        public DateTime audit_date { get; set; }
        public bool audit_result_line_is_pass { get; set; }
        public bool audit_result_operation_is_pass { get; set; }
        public string operation_gap_description { get; set; }
        public string create_by { get; set; }
        public DateTime create_time { get; set; }
        public string update_by { get; set; }
        public DateTime update_time { get; set; }

        //
        public string line_id { get; set; }
        public string line_name { get; set; }
        public string line_type_name { get; set; }
        public string model_name { get; set; }
        public string stage_name { get; set; }
        public string operation_name_local { get; set; }
        public string operation_name_en { get; set; }
        public string operation_name_zh { get; set; }
        public string audit_date_today {get; set;}
    }
}