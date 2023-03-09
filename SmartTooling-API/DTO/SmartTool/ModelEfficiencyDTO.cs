using System;

namespace SmartTooling_API.DTO.SmartTool
{
    public class ModelEfficiencyDTO
    {
        public string factory_id { get; set; }
        public string upper_id { get; set; }
        public string season { get; set; }
        public int month { get; set; }
        public decimal? efficiency_target { get; set; }
        public decimal? efficiency_actual { get; set; }
        public int sequence { get; set; }
        public string season_year { get; set; }
        public string create_by { get; set; }
        public DateTime? create_time { get; set; }
        public string update_by { get; set; }
        public DateTime? update_time { get; set; }

        public ModelEfficiencyDTO()
        {
            this.update_time = DateTime.Now;
        }
    }
}