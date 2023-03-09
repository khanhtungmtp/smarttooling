using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartTooling_API.Models.SmartTool
{
    public class Process_Type
    {
        [Key]
        [Column(Order = 0)]
        public string factory_id { get; set; }

        [Key]
        [Column(Order = 1)]
        public string process_type_id { get; set; }
        public string process_type_name_local { get; set; }
        public string process_type_name_en { get; set; }
        public string process_type_name_zh { get; set; }
        public int sequence { get; set; }
        public bool is_active { get; set; }
    }
}