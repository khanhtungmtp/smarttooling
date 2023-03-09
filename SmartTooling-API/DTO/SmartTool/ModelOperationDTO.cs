namespace SmartTooling_API.DTO.SmartTool
{
    public class ModelOperationDTO
    {
        public string factory_id { get; set; }
        public string model_no { get; set; }
        public string stage_id { get; set; }
        public string operation_id { get; set; }
        public string process_type_id { get; set; }
        public string process_type_name { get; set; }
        public string operation_name_local { get; set; }
        public string operation_name_en { get; set; }
        public string operation_name_zh { get; set; }
        public string sop_no { get; set; }
        public bool critical_quality { get; set; }
        public bool critical_efficiency { get; set; }
        public int sequence { get; set; }
        public string create_by { get; set; }
        public DateTime create_time { get; set; }
        public string update_by { get; set; }
        public DateTime update_time { get; set; }

        public string model_name { get; set; }
        public string stage_name { get; set; }
        public string process_type_name_en { get; set; }
        public int critical_quality_bit { get; set; }
        public int critical_efficiency_bit { get; set; }

        public ModelOperationDTO()
        {
            this.update_time = DateTime.Now;
        }
    }
}