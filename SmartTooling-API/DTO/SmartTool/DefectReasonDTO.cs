using System;

namespace SmartTooling_API.DTO.SmartTool
{
    public class DefectReasonDTO
    {
        public string factory_id { get; set; }
        public string defect_reason_id { get; set; }
        public string defect_reason_name { get; set; }
        public int sequence { get; set; }
        public bool is_active { get; set; }
        public string update_by { get; set; }
        public DateTime update_time { get; set; }

        public DefectReasonDTO()
        {
            this.update_time = DateTime.Now;
        }
    }
}