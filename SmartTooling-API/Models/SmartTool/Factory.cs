using System.ComponentModel.DataAnnotations;

namespace SmartTooling_API.Models.SmartTool
{
    public class Factory
    {
        [Key]
        public string factory_id { get; set; }
        public string factory_name { get; set; }
        public string customer_name { get; set; }
        public string location { get; set; }
    }
}