using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartTooling_API.Models.BestLine
{
    public partial class BL_Attachment_Type
    {
        public BL_Attachment_Type()
        {
            BL_Attachments = new HashSet<BL_Attachments>();
        }

        [Key]
        [StringLength(50)]
        public string factory_id { get; set; }
        [Key]
        [StringLength(10)]
        public string attachment_type_id { get; set; }
        [Required]
        [StringLength(50)]
        public string attachment_type_name { get; set; }
        public int sequence { get; set; }
        public bool is_active { get; set; }
        [Required]
        [StringLength(50)]
        public string update_by { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime update_time { get; set; }

        public virtual ICollection<BL_Attachments> BL_Attachments { get; set; }
    }
}
