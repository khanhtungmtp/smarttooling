using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartTooling_API.Models.SmartTool
{
    public class VW_ModelKaizen
    {
        public string factory_id { get; set; }
        public string model_no { get; set; }
        public string model_name { get; set; }
        public string model_type_id { get; set; }
        public string model_type_name { get; set; }
        public string model_family { get; set; }
        public string dev_season { get; set; }
        public string prod_season { get; set; }
        [Column(Order = 8, TypeName = "numeric")]
        public decimal? volume { get; set; }
        [Column(Order = 9, TypeName = "numeric")]
        public decimal? volume_percent { get; set; }
        [Column(Order = 10)]
        public string remarks { get; set; }
        public int serial_no { get; set; }
        public string kaizen_description { get; set; }
        public bool kaizen_type_eliminate { get; set; }
        public bool kaizen_type_reduce { get; set; }
        public bool kaizen_type_combine { get; set; }
        public bool kaizen_type_smart_tool { get; set; }
        public string stage_id { get; set; }
        public string stage_name { get; set; }
        [Column(Order = 19, TypeName = "date")]
        public DateTime start_date { get; set; }
        public string process_type_id { get; set; }
        public string process_type_name_en { get; set; }

        [Column(Order = 22, TypeName = "numeric")]
        public decimal? process_tct_sec { get; set; }
        [Column(Order = 23, TypeName = "numeric")]
        public decimal? ct_before_sec { get; set; }
        [Column(Order = 24, TypeName = "numeric")]
        public decimal? ct_after_sec { get; set; }
        public int? Improv { get; set; }
        [Column(TypeName = "numeric")]
        public decimal? rft_before_percent { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? rft_after_percent { get; set; }
        [Column(Order = 25, TypeName = "numeric")]
        public decimal? line_roll_out_percent { get; set; }
        public string operation_id { get; set; }
        public string operation_name_en { get; set; }
        public bool critical_quality { get; set; }
        public bool critical_efficiency { get; set; }
        public string before_remarks { get; set; }
        public string after_remarks { get; set; }
    }
}