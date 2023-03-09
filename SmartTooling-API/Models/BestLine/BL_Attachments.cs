using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartTooling_API.Models.BestLine
{
    public partial class BL_Attachments
    {
        [Key]
        public long id { get; set; }
        public long? layout_design_overall_id { get; set; }
        [Required]
        [StringLength(10)]
        public string attachment_type_id { get; set; }
        [Required]
        [StringLength(200)]
        public string attachment_name { get; set; }
        [Required]
        [StringLength(500)]
        public string attachment_file_url { get; set; }
        [Required]
        [StringLength(50)]
        public string update_by { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime update_time { get; set; }

        public virtual BL_Attachment_Type attachment_type_ { get; set; }
        [ForeignKey(nameof(layout_design_overall_id))]
        [InverseProperty(nameof(BL_Layout_Design_Overall.BL_Attachments))]
        public virtual BL_Layout_Design_Overall layout_design_overall_ { get; set; }
    }
}
