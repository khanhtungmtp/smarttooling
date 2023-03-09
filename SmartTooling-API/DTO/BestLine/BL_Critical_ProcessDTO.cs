using System;

namespace SmartTooling_API.DTO.BestLine
{
    public class BL_Critical_ProcessDTO
    {
        public string factory_id { get; set; }
        public string update_by { get; set; }
        public DateTime update_time { get; set; }
        public string line_id { get; set; }
        public string line_type_id { get; set; }
        public string model_no { get; set; }
        public string stage_id { get; set; }

        public string line_type_name { get; set; }
        public string line_name { get; set; }
        public string stage_name { get; set; }
        public string operation_name_local { get; set; }
        public string operation_name_en { get; set; }
        public string operation_name_zh { get; set; }
        public string operation_id { get; set; }
        public int takt_time { get; set; }
        public string best_practice_url { get; set; }
        public decimal ct_before_sec { get; set; }
        public decimal ct_after_sec { get; set; }
        public string man_remarks { get; set; }
        public string man_media_url { get; set; }
        public string machine_remarks { get; set; }
        public string machine_media_url { get; set; }
        public string method_remarks { get; set; }
        public string method_media_url { get; set; }
        public string material_remarks { get; set; }
        public string material_media_url { get; set; }
    }
}