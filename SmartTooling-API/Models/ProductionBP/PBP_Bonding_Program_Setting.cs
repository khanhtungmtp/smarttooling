using SmartTooling_API.Models.SmartTool;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartTooling_API.Models.ProductionBP
{
    public partial class PBP_Bonding_Program_Setting
    {
        [Key]
        [StringLength(50)]
        public string factory_id { get; set; }
        [Key]
        [StringLength(8)]
        public string model_no { get; set; }

        [Key]
        [StringLength(10)]
        public string chemical_process_type_id { get; set; }
        [Key]
        [StringLength(10)]
        public string auto_tech_id { get; set; }
        [Key]
        [StringLength(200)]
        public string chemical_name { get; set; }
        [Key]
        [StringLength(10)]
        public string adoption_component_id { get; set; }
        [Required]
        [StringLength(10)]
        public string chemical_supplier_id { get; set; }
        [Required]
        [StringLength(10)]
        public string process_adoption_scope_id { get; set; }
        [Column(TypeName = "date")]
        public DateTime first_month_of_production_adoption { get; set; }
        [Required]
        [StringLength(10)]
        public string main_upper_material_type_id { get; set; }
        [Required]
        [StringLength(10)]
        public string main_bottom_material_type_id { get; set; }
        public bool article_no_is_general { get; set; }
        [StringLength(500)]
        public string article_no_remarks { get; set; }
        [StringLength(400)]
        public string shoes_photo_url { get; set; }
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
