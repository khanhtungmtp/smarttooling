using System;

namespace SmartTooling_API.DTO.BestLine
{
    public class C2BOverallLayoutReportFilesDTO
    {
        public string factory_id { get; set; }
        public string line_id { get; set; }
        public string line_name { get; set; }
        public string line_type_id { get; set; }
        public string line_type_name { get; set; }
        public string model_no { get; set; }
        public string attachment_type_id { get; set; }
        public string attachment_name { get; set; }
        public string attachment_file_url { get; set; }
        public string update_by { get; set; }
        public DateTime? update_time { get; set; }
    }
}