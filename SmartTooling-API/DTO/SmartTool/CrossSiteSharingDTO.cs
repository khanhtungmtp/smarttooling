using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartTooling_API.DTO.SmartTool
{
    public class CrossSiteSharingDTO
    {
        public string doc_no { get; set; }
        public string model_no { get; set; }

        public string model_name { get; set; }

        public int serial_no { get; set; }

        public string kaizen_description { get; set; }

        public string to_factory_id { get; set; }

        public bool IsChoise { get; set; }
    }
}