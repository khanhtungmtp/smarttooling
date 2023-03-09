using System;

namespace SmartTooling_API.DTO.SmartTool
{
    public class VW_ModelKaizen_Dto
    {
        public string factory_id { get; set; }
        public string model_no { get; set; }
        public string model_name { get; set; }
        public string model_type_id { get; set; }
        public string model_type_name { get; set; }
        public string model_family { get; set; }
        public string dev_season { get; set; }
        public string prod_season { get; set; }
        public decimal volume { get; set; }
        public decimal volume_percent { get; set; }
        public int Improv { get; set; }
        public string remarks { get; set; }
        public int serial_no { get; set; }
        public string kaizen_description { get; set; }

        // -------------KaizenDTO Type bool-------------//
        public bool kaizen_type_eliminate { get; set; }
        public bool kaizen_type_reduce { get; set; }
        public bool kaizen_type_combine { get; set; }
        public bool kaizen_type_smart_tool { get; set; }

        // -------------KaizenDTO Type String-------------//
        public string kaizen_type_eliminate_string { get; set; }
        public string kaizen_type_reduce_string { get; set; }
        public string kaizen_type_combine_string { get; set; }
        public string kaizen_type_smart_tool_string { get; set; }

        public string stage_id { get; set; }
        public string stage_name { get; set; }
        public DateTime? start_date { get; set; }
        public string start_date_string { get; set; }
        public string process_type_id { get; set; }
        public string process_type_name_en { get; set; }
        public decimal process_tct_sec { get; set; }
        public decimal ct_before_sec { get; set; }
        public decimal ct_after_sec { get; set; }
        public decimal? rft_before_percent { get; set; }
        public decimal? rft_after_percent { get; set; }
        public decimal line_roll_out_percent { get; set; }
        public string operation_id { get; set; }
        public string operation_name_en { get; set; }


        // ---------------Critical bool-----------------//
        public bool critical_quality { get; set; }
        public bool critical_efficiency { get; set; }

        // ---------------Critical string-----------------//
        public string critical_quality_string { get; set; }
        public string critical_efficiency_string { get; set; }

        public string before_remarks { get; set; }
        public string after_remarks { get; set; }
    }
}