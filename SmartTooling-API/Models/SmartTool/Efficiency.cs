using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartTooling_API.Models.SmartTool
{
    public class Efficiency
    {
        [Key]
        [Column(Order = 0)]
        public string factory_id { get; set; }

        [Key]
        [Column(Order = 1)]
        public string upper_id { get; set; }

        [Key]
        [Column(Order = 2)]
        public string season { get; set; }

        [Key]
        [Column(Order = 3)]
        public int month { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal? efficiency_target { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal? efficiency_actual { get; set; }

        public int sequence { get; set; }



        public string create_by { get; set; }

        public DateTime create_time { get; set; }

        public string update_by { get; set; }

        public DateTime update_time { get; set; }
    }
}