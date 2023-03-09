using System;

namespace SmartTooling_API.DTO.SmartTool
{
    public class VW_RFT_AVGDTO
    {
        public string factory_id { get; set; }
        public string model_no { get; set; }
        public double? CR2 { get; set; }
        public double? SMS { get; set; }
        public double? CS1 { get; set; }
        public double? CS2 { get; set; }
        public double? CS3 { get; set; }
        public double? PROD1 { get; set; }
        public double? PROD2 { get; set; }
    }
}