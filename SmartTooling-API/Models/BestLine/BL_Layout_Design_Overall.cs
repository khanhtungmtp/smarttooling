using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartTooling_API.Models.BestLine
{
    public partial class BL_Layout_Design_Overall
    {
        public BL_Layout_Design_Overall()
        {
            BL_Attachments = new HashSet<BL_Attachments>();
            BL_Critical_Process_Analysis = new HashSet<BL_Critical_Process_Analysis>();
            BL_Layout_Design_Process = new HashSet<BL_Layout_Design_Process>();
        }

        [Key]
        public long id { get; set; }
        [Required]
        [StringLength(50)]
        public string factory_id { get; set; }
        [Required]
        [StringLength(10)]
        public string line_id { get; set; }
        [Required]
        [StringLength(20)]
        public string line_type_id { get; set; }
        [Required]
        [StringLength(8)]
        public string model_no { get; set; }
        [Required]
        [StringLength(4)]
        public string prod_season { get; set; }
        [Column(TypeName = "numeric(18, 0)")]
        public decimal? no_of_process_before { get; set; }
        [Column(TypeName = "numeric(18, 0)")]
        public decimal? no_of_process_after { get; set; }
        [Column(TypeName = "numeric(18, 0)")]
        public decimal tct_before { get; set; }
        [Column(TypeName = "numeric(18, 0)")]
        public decimal tct_after { get; set; }
        [Column(TypeName = "numeric(18, 0)")]
        public decimal cps_mp_before { get; set; }
        [Column(TypeName = "numeric(18, 0)")]
        public decimal cps_mp_after { get; set; }
        [Column(TypeName = "numeric(18, 0)")]
        public decimal assembly_mp_before { get; set; }
        [Column(TypeName = "numeric(18, 0)")]
        public decimal assembly_mp_after { get; set; }
        [Column(TypeName = "numeric(18, 0)")]
        public decimal eolr_before { get; set; }
        [Column(TypeName = "numeric(18, 0)")]
        public decimal eolr_after { get; set; }
        [Column(TypeName = "numeric(18, 2)")]
        public decimal ller_before_percent { get; set; }
        [Column(TypeName = "numeric(18, 2)")]
        public decimal ller_after_percent { get; set; }
        [Column(TypeName = "numeric(18, 2)")]
        public decimal tentative_pph_before { get; set; }
        [Column(TypeName = "numeric(18, 2)")]
        public decimal tentative_pph_after { get; set; }
        [Column(TypeName = "numeric(18, 2)")]
        public decimal? tentative_efficiency_before_percent { get; set; }
        [Column(TypeName = "numeric(18, 2)")]
        public decimal? tentative_efficiency_after_percent { get; set; }
        [StringLength(200)]
        public string c2b_overall_image { get; set; }
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

        public virtual BL_Lines line_ { get; set; }
        [InverseProperty("layout_design_overall_")]
        public virtual ICollection<BL_Attachments> BL_Attachments { get; set; }
        [InverseProperty("layout_design_overall_")]
        public virtual ICollection<BL_Critical_Process_Analysis> BL_Critical_Process_Analysis { get; set; }
        [InverseProperty("layout_design_overall_")]
        public virtual ICollection<BL_Layout_Design_Process> BL_Layout_Design_Process { get; set; }
    }
}
