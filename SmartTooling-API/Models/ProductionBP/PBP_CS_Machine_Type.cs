﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartTooling_API.Models.ProductionBP
{
    public partial class PBP_CS_Machine_Type
    {
        [Key]
        [StringLength(50)]
        public string factory_id { get; set; }
        [Key]
        [StringLength(10)]
        public string cs_machine_type_id { get; set; }
        [Required]
        [StringLength(50)]
        public string cs_machine_type_name { get; set; }
        public int sequence { get; set; }
        public bool is_active { get; set; }
        [Required]
        [StringLength(50)]
        public string update_by { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime update_time { get; set; }
    }
}
