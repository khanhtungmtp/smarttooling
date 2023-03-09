using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartTooling_API.Models.BestLine
{
    public partial class BL_Critical_Process_Analysis
    {
        public BL_Critical_Process_Analysis()
        {
            BL_Rollout_Progress = new HashSet<BL_Rollout_Progress>();
        }

        [Key]
        public long id { get; set; }
        public long layout_design_overall_id { get; set; }
        [Required]
        [StringLength(8)]
        public string stage_id { get; set; }
        [Required]
        [StringLength(50)]
        public string operation_id { get; set; }
        public int takt_time { get; set; }
        [StringLength(200)]
        public string best_practice_url { get; set; }
        [Column(TypeName = "numeric(18, 0)")]
        public decimal ct_before_sec { get; set; }
        [Column(TypeName = "numeric(18, 0)")]
        public decimal ct_after_sec { get; set; }
        [StringLength(4000)]
        public string man_remarks { get; set; }
        [StringLength(200)]
        public string man_media_url { get; set; }
        [StringLength(4000)]
        public string machine_remarks { get; set; }
        [StringLength(200)]
        public string machine_media_url { get; set; }
        [StringLength(4000)]
        public string method_remarks { get; set; }
        [StringLength(200)]
        public string method_media_url { get; set; }
        [StringLength(4000)]
        public string material_remarks { get; set; }
        [StringLength(200)]
        public string material_media_url { get; set; }
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

        [ForeignKey(nameof(layout_design_overall_id))]
        [InverseProperty(nameof(BL_Layout_Design_Overall.BL_Critical_Process_Analysis))]
        public virtual BL_Layout_Design_Overall layout_design_overall_ { get; set; }
        [InverseProperty("critical_process_analysis_")]
        public virtual ICollection<BL_Rollout_Progress> BL_Rollout_Progress { get; set; }

    }
}
