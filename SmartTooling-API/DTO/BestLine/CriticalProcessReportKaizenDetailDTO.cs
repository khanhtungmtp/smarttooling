using SmartTooling_API.DTO.SmartTool;

namespace SmartTooling_API.DTO.BestLine
{
    public class CriticalProcessReportKaizenDetailDTO
    {
        public KaizenDTO Kaizen { get; set; }
        public ModelDTO Model { get; set; }
        public ModelOperationDTO ModelOperation { get; set; }
    }
}