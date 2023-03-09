using SmartTooling_API._Repositories.Interfaces.SmartTool;
using SmartTooling_API.Models.BestLine;
using SmartTooling_API.DTO.SmartTool;

namespace SmartTooling_API._Repositories.Interfaces.BestLine
{
    public interface IBLCriticalProcessAnalysisRepository : IBestLineRepository<BL_Critical_Process_Analysis>
    {
        //        public Task<bool> CheckExistsBLCriticalProcessAnalysis(ModelOperationDTO operation);
    }
}