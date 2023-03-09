using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartTooling_API.Models.SmartTool
{
    public class Kaizen_Benefits_Application_Form
    {
        [Key]
        public string factory_id { get; set; }
        [Key]
        public string model_no { get; set; }
        [Key]
        public int serial_no { get; set; }
        [Key]
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
        [Column(TypeName = "numeric(18, 2)")]
        public decimal? estimated_savings_per_month { get; set; }
        [Column(TypeName = "numeric(18, 2)")]
        public decimal? estimated_roi { get; set; }
        public string create_by { get; set; }
        public DateTime create_time { get; set; }
        public string update_by { get; set; }
        public DateTime update_time { get; set; }
    }
}