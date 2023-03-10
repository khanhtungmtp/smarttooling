using System;

namespace SmartTooling_API.DTO.BestLine
{
    public class BL_AttachmentsParams
    {
        public string line_id { get; set; }
        public string line_type_id { get; set; }
        public string model_no { get; set; }
        public string model_name { get; set; }
        public string prod_season { get; set; }
        public string attachment_type_id { get; set; }
        public string attachment_name { get; set; }
        public string attachment_file_url { get; set; }
        public string update_by { get; set; }
        public DateTime? update_time { get; set; }
    }
    public class BL_AttachmentsDTO
    {
        public long layout_design_overall_id { get; set; }
        public string attachment_type_id { get; set; }
        public string attachment_name { get; set; }
        public string attachment_file_url { get; set; }
        public string update_by { get; set; }
        public DateTime? update_time { get; set; }
    }
}