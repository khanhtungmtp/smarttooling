using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartTooling_API.Models.SmartTool
{
    public class VW_RFTReportDetail
    {
        public string factory_id { get; set; }
        public string model_no { get; set; }
        public int sequence { get; set; }
        public string operation_name_local { get; set; }
        [Column(TypeName = "numeric")]
        public double? CR2 { get; set; }
        [Column(TypeName = "numeric")]
        public double? SMS { get; set; }
        [Column(TypeName = "numeric")]
        public double? CS1 { get; set; }
        [Column(TypeName = "numeric")]
        public double? CS2 { get; set; }
        [Column(TypeName = "numeric")]
        public double? CS3 { get; set; }
        [Column(TypeName = "numeric")]
        public double? PROD1 { get; set; }
        [Column(TypeName = "numeric")]
        public double? PROD2 { get; set; }
        [Column(TypeName = "numeric")]
        public double? MP1 { get; set; }
        [Column(TypeName = "numeric")]
        public double? MP2 { get; set; }
        [Column(TypeName = "numeric")]
        public double? MP3 { get; set; }
    }
}