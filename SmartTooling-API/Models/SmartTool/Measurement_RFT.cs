using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartTooling_API.Models.SmartTool
{
    public partial class Measurement_RFT
    {
        [Key]
        [StringLength(50)]
        public string factory_id { get; set; }
        [Key]
        [StringLength(8)]
        public string model_no { get; set; }
        [Key]
        [StringLength(8)]
        public string stage_id { get; set; }
        [Key]
        [StringLength(50)]
        public string operation_id { get; set; }
        public int total_produced_qty { get; set; }
        public int defect_qty { get; set; }
        [Required]
        [StringLength(50)]
        public string defect_reason_id { get; set; }

        [Column(TypeName = "numeric(18, 2)")]
        public decimal rft_percent { get; set; }
        [Required]
        [StringLength(100)]
        public string defect_pic { get; set; }
        [Required]
        [StringLength(100)]
        public string inspector { get; set; }
        [Required]
        [StringLength(50)]
        public string create_by { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime create_time { get; set; }
        [Required]
        [StringLength(50)]
        public string update_by { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime update_time { get; set; }
    }
}