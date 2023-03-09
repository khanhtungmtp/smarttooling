using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using SmartTooling_API._Repositories.Interfaces.ProductionBP;
using SmartTooling_API.Data;
using SmartTooling_API.Models.ProductionBP;
using SmartTooling_API.DTO.SmartTool;

namespace SmartTooling_API._Repositories.Repositories.ProductionBP
{
    public class PBPComputerStitchingSettingRepository : ProductionBPRepository<PBP_ComputerStitchingSetting>, IPBPComputerStitchingSettingRepository
    {
        private readonly DataContext _context;
        public PBPComputerStitchingSettingRepository(DataContext context, CBDataContext CBcontext, SHCDataContext SHCcontext, SPCDataContext SPCcontext, TSHDataContext TSHcontext, IConfiguration configuration) : base(context, CBcontext, SHCcontext, SPCcontext, TSHcontext, configuration)
        {
            _context = context;
        }

        public async Task<bool> CheckExistsPBPComputerStitchingSetting(ModelOperationDTO operation)
        {
            if (await _context.PBP_ComputerStitchingSetting.AnyAsync(x => x.factory_id == operation.factory_id &&
                                                                          x.model_no == operation.model_no &&
                                                                          x.stage_id == operation.stage_id &&
                                                                          x.operation_id == operation.operation_id))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}