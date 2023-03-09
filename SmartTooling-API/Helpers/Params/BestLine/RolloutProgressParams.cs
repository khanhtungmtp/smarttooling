using System;

namespace SmartTooling_API.DTO.BestLine
{
    public class Params_Rollout_ProgressDTO
    {
        public string line_id { get; set; }
        public string line_type_id { get; set; }
        public string model_no { get; set; }
        public string stage_id { get; set; }
        public string operation_id { get; set; }
        public string rollout_line_id { get; set; }
    }
}