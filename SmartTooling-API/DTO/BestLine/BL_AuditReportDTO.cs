using System;

namespace SmartTooling_API.DTO.BestLine
{
    public class BL_AuditReportDTO
    {
        public string factory_id { get; set; }
        public string line_id { get; set; }
        public string line_name { get; set; }
        public string line_type_id { get; set; }
        public string model_no { get; set; }
        public string model_name { get; set; }
        public string stage_name { get; set; }
        public string stage_id { get; set; }
        public string rollout_line_id { get; set; }
        public string operation_id { get; set; }
        public string operation_name_local { get; set; }
        public string operation_name_en { get; set; }
        public string operation_name_zh { get; set; }
        public DateTime rollout_date { get; set; }
        public DateTime first_audit_date { get; set; }
        public DateTime latest_audit_date { get; set; }
        public int audit_conducted { get; set; }
        public bool layout_audit { get; set; }
        public int audit_pass { get; set; }
        public int audit_fail { get; set; }
        public int countBeingAudit {get;set;}
    }
}