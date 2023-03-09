using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartTooling_API.Models.ProductionBP
{
    public partial class PBP_Pad_Print_Setting
    {
        [Key]
        [StringLength(50)]
        public string factory_id { get; set; }
        [Key]
        [StringLength(8)]
        public string model_no { get; set; }
        [Key]
        [StringLength(100)]
        public string component_name { get; set; }
        [Key]
        [StringLength(10)]
        public string material_type_id { get; set; }
        [Key]
        [StringLength(100)]
        public string chemical_ink { get; set; }
        [Required]
        [StringLength(200)]
        public string material_description { get; set; }
        [Required]
        [StringLength(10)]
        public string pad_shape_id { get; set; }
        public int number_of_pad_hits { get; set; }
        [Required]
        [StringLength(10)]
        public string machine_vendor_id { get; set; }
        [Required]
        [StringLength(200)]
        public string machine_model { get; set; }
        public bool is_rotary_table_used { get; set; }
        [Required]
        [StringLength(200)]
        public string chemical_hardener { get; set; }
        [Required]
        [StringLength(200)]
        public string chemical_additive { get; set; }
        [Required]
        [StringLength(200)]
        public string chemical_primer { get; set; }
        [Required]
        [StringLength(200)]
        public string chemical_others { get; set; }
        public bool article_no_is_general { get; set; }
        [StringLength(500)]
        public string article_no_remarks { get; set; }
        [Required]
        [StringLength(400)]
        public string component_photo_url { get; set; }
        [Required]
        [StringLength(400)]
        public string operation_video_url { get; set; }
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
