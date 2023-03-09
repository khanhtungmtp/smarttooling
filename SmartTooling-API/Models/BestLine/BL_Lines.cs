using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartTooling_API.Models.BestLine
{
    public partial class BL_Lines
    {
        public BL_Lines()
        {
            BL_Layout_Design_Overall = new HashSet<BL_Layout_Design_Overall>();
        }

        [Key]
        [StringLength(50)]
        public string factory_id { get; set; }
        [Key]
        [StringLength(10)]
        public string line_id { get; set; }
        [Required]
        [StringLength(50)]
        public string line_name { get; set; }
        public int sequence { get; set; }
        public bool is_active { get; set; }
        [Required]
        [StringLength(50)]
        public string update_by { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime update_time { get; set; }

        public virtual ICollection<BL_Layout_Design_Overall> BL_Layout_Design_Overall { get; set; }
    }
}
