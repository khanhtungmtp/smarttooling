using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartTooling_API.Models.SmartTool
{
    public class Model
    {
        [Key]
        [Column(Order = 0)]
        public string factory_id { get; set; }

        [Key]
        [Column(Order = 1)]
        public string model_no { get; set; }
        public string model_name { get; set; }
        public string model_type_id { get; set; }
        public string model_family { get; set; }
        public string upper_id { get; set; }
        public string dev_season { get; set; }
        public string prod_season { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? volume { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? volume_percent { get; set; }
        public string remarks { get; set; }
        public string model_picture { get; set; }
        public bool is_active { get; set; }
        public bool top_model { get; set; }
        public bool pilot_line { get; set; }
        public string create_by { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime create_time { get; set; }
        public string update_by { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime update_time { get; set; }

    }
}