using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartTooling_API.Models.ProductionBP
{
    public partial class PBP_ComputerStitchingSetting
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
        [Key]
        [StringLength(10)]
        public string cs_type_id { get; set; }
        [Key]
        [StringLength(10)]
        public string cs_machine_type_id { get; set; }
        public bool sop_setup { get; set; }
        public bool production_adoption { get; set; }
        public bool is_critical_process { get; set; }
        [Required]
        [StringLength(10)]
        public string main_upper_material_type_id { get; set; }
        [Required]
        [StringLength(200)]
        public string cs_machine_model { get; set; }
        public int cs_speed_setting_rpm { get; set; }
        public int number_of_size_group { get; set; }
        [Required]
        [StringLength(10)]
        public string jig_design_id { get; set; }
        public bool article_no_is_general { get; set; }
        [StringLength(500)]
        public string article_no_remarks { get; set; }
        [StringLength(400)]
        public string jig_photo_url { get; set; }
        [StringLength(400)]
        public string cs_video_url { get; set; }
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
