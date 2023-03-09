using Microsoft.Extensions.Configuration;
using SmartTooling_API._Repositories.Interfaces.ProductionBP;
using SmartTooling_API.Data;
using SmartTooling_API.Models.ProductionBP;

namespace SmartTooling_API._Repositories.Repositories.ProductionBP
{
    public class PBPPadShapeTypeRepository : ProductionBPRepository<PBP_Pad_Shape_Type>, IPBPPadShapeTypeRepository
    {
        private readonly DataContext _context;
        public PBPPadShapeTypeRepository(DataContext context, CBDataContext CBcontext, SHCDataContext SHCcontext, SPCDataContext SPCcontext, TSHDataContext TSHcontext, IConfiguration configuration) : base(context, CBcontext, SHCcontext, SPCcontext, TSHcontext, configuration)
        {
            _context = context;
        }
    }
}