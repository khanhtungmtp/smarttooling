using System;

namespace SmartTooling_API.DTO.BestLine
{
    public class BL_AuditReportDetailDTO
    {
        public DateTime audit_date { get; set; }
        public string line_name { get; set; }
        public string line_type_name { get; set; }
        public int audit_count { get; set; }
        public string model_no { get; set; }
        public string model_name { get; set; }
        public string stage_name { get; set; }
        public string operation_name_local { get; set; }
        public string operation_name_en { get; set; }
        public string operation_name_zh { get; set; }
        public bool audit_result_line_is_pass { get; set; }
        public bool audit_result_operation_is_pass { get; set; }
        public string operation_gap_description { get; set; }
    }
}