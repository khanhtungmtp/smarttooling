using System;

namespace SmartTooling_API.DTO.BestLine
{
    public class BL_Layout_Design_ProcessDTO
    {

        public string factory_id { get; set; }
        public string line_id { get; set; }
        public string line_type_id { get; set; }
        public string model_no { get; set; }
        public string process_type_id { get; set; }
        public string each_process_image_before { get; set; }
        public string each_process_image_after { get; set; }
        public string remarks { get; set; }
        public string create_by { get; set; }
        public DateTime create_time { get; set; }
        public string update_by { get; set; }
        public DateTime update_time { get; set; }

        public string each_process_image_before_result { get; set; }
        public string each_process_image_after_result { get; set; }
    }
}