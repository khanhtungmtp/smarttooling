using System;
namespace SmartTooling_API.DTO.ProductionBP
{
    public class PBP_Material_TypeDTO
    {
        public string factory_id { get; set; }
        public string material_type_id { get; set; }
        public string material_type_name { get; set; }
        public int sequence { get; set; }
        public bool is_active { get; set; }
        public string update_by { get; set; }
        public DateTime update_time { get; set; }
    }
}