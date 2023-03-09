using Microsoft.Extensions.Configuration;
using SmartTooling_API._Repositories.Interfaces.ProductionBP;
using SmartTooling_API.Data;
using SmartTooling_API.Models.ProductionBP;

namespace SmartTooling_API._Repositories.Repositories.ProductionBP
{
    public class PBPMainBottomMaterialTypeRepository : ProductionBPRepository<PBP_Main_Bottom_Material_Type>, IPBPMainBottomMaterialTypeRepository
    {
        private readonly DataContext _context;
        public PBPMainBottomMaterialTypeRepository(DataContext context, CBDataContext CBcontext, SHCDataContext SHCcontext, SPCDataContext SPCcontext, TSHDataContext TSHcontext, IConfiguration configuration) : base(context, CBcontext, SHCcontext, SPCcontext, TSHcontext, configuration)
        {
            _context = context;
        }
    }
}