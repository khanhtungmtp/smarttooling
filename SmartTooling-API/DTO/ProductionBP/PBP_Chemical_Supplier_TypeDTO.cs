using System;

namespace SmartTooling_API.DTO.ProductionBP
{
    public class PBP_Chemical_Supplier_TypeDTO
    {
        public string factory_id { get; set; }
        public string chemical_supplier_id { get; set; }
        public string chemical_supplier_name { get; set; }
        public int sequence { get; set; }
        public bool is_active { get; set; }
        public string update_by { get; set; }
        public DateTime update_time { get; set; }
    }
}