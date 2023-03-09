using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartTooling_API.Models.SmartTool
{
    public class Kaizen
    {
        [Key]
        [Column(Order = 0)]
        public string factory_id { get; set; }

        [Key]
        [Column(Order = 1)]
        public string model_no { get; set; }


        public int serial_no { get; set; }

        [Key]
        [Column(Order = 2)]
        public string kaizen_description { get; set; }
        public string stage_id { get; set; }
        public string operation_id { get; set; }

        [Column(TypeName = "date")]
        public DateTime? start_date { get; set; }

        public bool kaizen_type_eliminate { get; set; }

        public bool kaizen_type_reduce { get; set; }

        public bool kaizen_type_combine { get; set; }

        public bool kaizen_type_smart_tool { get; set; }

        [Column(TypeName = "numeric")]
        public decimal process_tct_sec { get; set; }

        [Column(TypeName = "numeric")]
        public decimal ct_before_sec { get; set; }

        [Column(TypeName = "numeric")]
        public decimal ct_after_sec { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? rft_before_percent { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? rft_after_percent { get; set; }

        [Column(TypeName = "numeric")]
        public decimal line_roll_out_percent { get; set; }
        public string before_media { get; set; }
        public string after_media { get; set; }
        public string before_remarks { get; set; }
        public string after_remarks { get; set; }
        public string kaizen_from { get; set; }
        public int clicks_times { get; set; }
        public string create_by { get; set; }
        public DateTime? create_time { get; set; }
        public string update_by { get; set; }
        public DateTime? update_time { get; set; }
    }
}