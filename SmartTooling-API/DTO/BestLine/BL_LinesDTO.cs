using System;

namespace SmartTooling_API.DTO.BestLine
{
    public class BL_LinesDTO
    {
        public string factory_id { get; set; }
        public string line_id { get; set; }
        public string line_name { get; set; }
        public int sequence { get; set; }
        public bool is_active { get; set; }
        public string update_by { get; set; }
        public DateTime update_time { get; set; }

        //
        public string rollout_line_id { get; set; }
    }
}