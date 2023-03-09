using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartTooling_API.Models.BestLine
{
    public partial class BL_Rollout_Audit
    {
        [Key]
        public long id { get; set; }
        public long rollout_progress_id { get; set; }
        [Column(TypeName = "date")]
        public DateTime audit_date { get; set; }
        public bool audit_result_line_is_pass { get; set; }
        public bool audit_result_operation_is_pass { get; set; }
        [StringLength(4000)]
        public string operation_gap_description { get; set; }
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

        [ForeignKey(nameof(rollout_progress_id))]
        [InverseProperty(nameof(BL_Rollout_Progress.BL_Rollout_Audit))]
        public virtual BL_Rollout_Progress rollout_progress_ { get; set; }
    }
}
