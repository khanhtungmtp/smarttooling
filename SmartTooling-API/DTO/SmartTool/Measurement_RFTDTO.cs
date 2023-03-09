using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartTooling_API.DTO.SmartTool
{
    public class Measurement_RFTDTO
    {
        public string factory_id { get; set; }

        public string model_no { get; set; }

        public string stage_id { get; set; }

        public string operation_id { get; set; }
        public int total_produced_qty { get; set; }
        public int defect_qty { get; set; }

        public string defect_reason_id { get; set; }

        public decimal? rft_percent { get; set; }

        public string defect_pic { get; set; }

        public string inspector { get; set; }
        public string create_by { get; set; }

        public DateTime create_time { get; set; }

        public string update_by { get; set; }

        public DateTime update_time { get; set; }
    }
}