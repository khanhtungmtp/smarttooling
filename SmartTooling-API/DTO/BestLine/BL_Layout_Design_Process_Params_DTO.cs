using System;

namespace SmartTooling_API.DTO.BestLine
{
    public class BL_Layout_Design_Process_Params_DTO
    {
        public string line_id { get; set; }
        public string line_name { get; set; }
        public string line_type_id { get; set; }
        public string line_type_name { get; set; }
        public string model_no { get; set; }
        public string model_name { get; set; }
        public string process_type_id { get; set; }
        public int sequence { get; set; }
        public string process_type_name_en { get; set; }
        public string each_process_image_before { get; set; }
        public string each_process_image_after { get; set; }
        public string remarks { get; set; }
        public string update_by { get; set; }
        public DateTime update_time { get; set; }
    }
}