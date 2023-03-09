using System;

namespace SmartTooling_API.DTO.SmartTool
{
    public class ModelDTO
    {
        public string factory_id { get; set; }
        public string model_no { get; set; }
        public string model_name { get; set; }
        public string model_type_id { get; set; }
        public string model_family { get; set; }
        public string upper_id { get; set; }
        public string dev_season { get; set; }
        public string prod_season { get; set; }
        public decimal? volume { get; set; }
        public decimal? volume_percent { get; set; }
        public string remarks { get; set; }
        public string model_picture { get; set; }
        public bool top_model { get; set; }
        public bool pilot_line { get; set; }
        public bool is_active { get; set; }
        public string create_by { get; set; }
        public DateTime create_time { get; set; }
        public string update_by { get; set; }
        public DateTime update_time { get; set; }
        public ModelDTO()
        {
            this.update_time = DateTime.Now;
        }

        public string model_picture_result { get; set; }
    }
}