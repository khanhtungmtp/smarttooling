using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartTooling_API.Models.BestLine
{
    public partial class BL_Layout_Design_Process
    {
        public BL_Layout_Design_Process()
        {
            BL_Layout_Design_Process_Data = new HashSet<BL_Layout_Design_Process_Data>();
        }

        [Key]
        public long id { get; set; }
        public long? layout_design_overall_id { get; set; }
        [Required]
        [StringLength(20)]
        public string process_type_id { get; set; }
        [StringLength(1000)]
        public string each_process_image_before { get; set; }
        [StringLength(1000)]
        public string each_process_image_after { get; set; }
        [StringLength(4000)]
        public string remarks { get; set; }
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
        [InverseProperty(nameof(BL_Layout_Design_Overall.BL_Layout_Design_Process))]
        public virtual BL_Layout_Design_Overall layout_design_overall_ { get; set; }
        [InverseProperty("layout_design_process_")]
        public virtual ICollection<BL_Layout_Design_Process_Data> BL_Layout_Design_Process_Data { get; set; }
    }
}
