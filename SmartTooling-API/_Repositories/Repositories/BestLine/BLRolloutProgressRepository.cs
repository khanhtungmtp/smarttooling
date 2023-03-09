using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using SmartTooling_API._Repositories.Interfaces.BestLine;
using SmartTooling_API._Repositories.Repositories.SmartTool;
using SmartTooling_API.Data;
using SmartTooling_API.Models.BestLine;
using SmartTooling_API.DTO.SmartTool;

namespace SmartTooling_API._Repositories.Repositories.BestLine
{
    public class BLRolloutProgressRepository : BestLineRepository<BL_Rollout_Progress>, IBLRolloutProgressRepository
    {
        private readonly DataContext _context;
        public BLRolloutProgressRepository(DataContext context, CBDataContext CBcontext, SHCDataContext SHCcontext, SPCDataContext SPCcontext, TSHDataContext TSHcontext, IConfiguration configuration) : base(context, CBcontext, SHCcontext, SPCcontext, TSHcontext, configuration)
        {
            _context = context;
        }

        // public async Task<bool> CheckExistsBLRolloutProgress(ModelOperationDTO operation)
        // {
        //     if (await _context.BL_Rollout_Progress.AnyAsync(x => x.factory_id == operation.factory_id &&
        //                                                          x.model_no == operation.model_no &&
        //                                                          x.stage_id == operation.stage_id &&
        //                                                          x.operation_id == operation.operation_id))
        //     {
        //         return true;
        //     }
        //     else
        //     {
        //         return false;
        //     }
        // }
    }
}