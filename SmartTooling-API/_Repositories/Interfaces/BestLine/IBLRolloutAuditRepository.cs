using SmartTooling_API._Repositories.Interfaces.SmartTool;
using SmartTooling_API.Models.BestLine;
using SmartTooling_API.DTO.SmartTool;

namespace SmartTooling_API._Repositories.Interfaces.BestLine
{
    public interface IBLRolloutAuditRepository : IBestLineRepository<BL_Rollout_Audit>
    {
        // Task<bool> CheckExistsBLRolloutAudit(ModelOperationDTO operation);
    }
}