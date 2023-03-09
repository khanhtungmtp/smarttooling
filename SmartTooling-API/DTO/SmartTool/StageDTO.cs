namespace SmartTooling_API.DTO.SmartTool
{
    public class StageDTO
    {

        public string factory_id { get; set; }

        public string stage_id { get; set; }

        public string stage_name { get; set; }
        public int sequence { get; set; }
        public bool is_active { get; set; }
    }
}