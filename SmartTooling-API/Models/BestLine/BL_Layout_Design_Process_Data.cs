using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartTooling_API.Models.BestLine
{
    public partial class BL_Layout_Design_Process_Data
    {
        [Key]
        public long id { get; set; }
        public long layout_design_process_id { get; set; }
        [StringLength(200)]
        public string node_name { get; set; }
        [Required]
        [StringLength(10)]
        public string before_or_after { get; set; }
        [Column(TypeName = "numeric(18, 2)")]
        public decimal? cycle_time { get; set; }
        [Column(TypeName = "numeric(18, 2)")]
        public decimal? takt_time { get; set; }
        [Column(TypeName = "numeric(18, 2)")]
        public decimal? employee_qty { get; set; }
        public int sequence { get; set; }
        [Required]
        [StringLength(50)]
        public string update_by { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime update_time { get; set; }

        [ForeignKey(nameof(layout_design_process_id))]
        [InverseProperty(nameof(BL_Layout_Design_Process.BL_Layout_Design_Process_Data))]
        public virtual BL_Layout_Design_Process layout_design_process_ { get; set; }
    }
}
