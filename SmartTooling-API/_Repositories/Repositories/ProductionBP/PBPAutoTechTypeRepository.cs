using Microsoft.Extensions.Configuration;
using SmartTooling_API._Repositories.Interfaces.ProductionBP;
using SmartTooling_API.Data;
using SmartTooling_API.Models.ProductionBP;

namespace SmartTooling_API._Repositories.Repositories.ProductionBP
{
    public class PBPAutoTechTypeRepository : ProductionBPRepository<PBP_Auto_Tech_Type>, IPBPAutoTechTypeRepository
    {
        private readonly DataContext _context;
        public PBPAutoTechTypeRepository(DataContext context, CBDataContext CBcontext, SHCDataContext SHCcontext, SPCDataContext SPCcontext, TSHDataContext TSHcontext, IConfiguration configuration) : base(context, CBcontext, SHCcontext, SPCcontext, TSHcontext, configuration)
        {
            _context = context;
        }
    }
}