using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartTooling_API.Models.SmartTool
{
    public class Model_Operation
    {
        [Key]
        [Column(Order = 0)]
        public string factory_id { get; set; }

        [Key]
        [Column(Order = 1)]
        public string model_no { get; set; }

        [Key]
        [Column(Order = 2)]
        public string stage_id { get; set; }

        [Key]
        [Column(Order = 3)]
        public string operation_id { get; set; }
        public string process_type_id { get; set; }
        public string operation_name_local { get; set; }
        public string operation_name_en { get; set; }
        public string operation_name_zh { get; set; }
        public string sop_no { get; set; }
        public bool critical_quality { get; set; }
        public bool critical_efficiency { get; set; }
        public int sequence { get; set; }
        public string create_by { get; set; }

        public DateTime create_time { get; set; }
        public string update_by { get; set; }
        public DateTime update_time { get; set; }
    }
}