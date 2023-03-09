using System;

namespace SmartTooling_API.DTO.SmartTool
{
    public class Kaizen_Benefits_Application_FormDTO
    {
        public string factory_id { get; set; }

        public string model_no { get; set; }

        public int serial_no { get; set; }
        public string to_factory_id { get; set; }
        public string doc_no { get; set; }
        public DateTime? fill_in_date { get; set; }
        public string proposed_by_facility { get; set; }
        public string proposed_by_dept { get; set; }
        public string proposed_by_name_id { get; set; }
        public string team_member { get; set; }
        public bool? benefits_category_hse { get; set; }
        public bool? benefits_category_delivery { get; set; }
        public bool? benefits_category_quality { get; set; }
        public bool? benefits_category_efficiency { get; set; }
        public bool? benefits_category_others { get; set; }
        public decimal? estimated_savings_per_month { get; set; }
        public decimal? estimated_roi { get; set; }
        public string create_by { get; set; }
        public DateTime create_time { get; set; }
        public string update_by { get; set; }
        public DateTime update_time { get; set; }
    }
}