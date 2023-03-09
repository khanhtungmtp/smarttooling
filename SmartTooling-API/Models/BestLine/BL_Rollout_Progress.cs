using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartTooling_API.Models.BestLine
{
    public partial class BL_Rollout_Progress
    {
        public BL_Rollout_Progress()
        {
            BL_Rollout_Audit = new HashSet<BL_Rollout_Audit>();
        }

        [Key]
        public long id { get; set; }
        public long critical_process_analysis_id { get; set; }
        [Required]
        [StringLength(10)]
        public string rollout_line_id { get; set; }
        [Column(TypeName = "date")]
        public DateTime rollout_date { get; set; }
        public int? mp_allocated { get; set; }
        [StringLength(200)]
        public string machines_name { get; set; }
        public int? machines_qty { get; set; }
        [StringLength(200)]
        public string tool_name { get; set; }
        public int? tool_qty { get; set; }
        [Required]
        [StringLength(4000)]
        public string operation_descriptions { get; set; }
        [StringLength(200)]
        public string operation_video_url { get; set; }
        [StringLength(200)]
        public string rollout_operation_layout { get; set; }
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

        [ForeignKey(nameof(critical_process_analysis_id))]
        [InverseProperty(nameof(BL_Critical_Process_Analysis.BL_Rollout_Progress))]
        public virtual BL_Critical_Process_Analysis critical_process_analysis_ { get; set; }
        [InverseProperty("rollout_progress_")]
        public virtual ICollection<BL_Rollout_Audit> BL_Rollout_Audit { get; set; }
    }
}
