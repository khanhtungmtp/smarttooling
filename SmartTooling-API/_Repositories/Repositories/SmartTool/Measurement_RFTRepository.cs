using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartTooling_API._Repositories.Interfaces.SmartTool;
using SmartTooling_API.Data;
using SmartTooling_API.Models.SmartTool;
using SmartTooling_API.DTO.SmartTool;
using Microsoft.Extensions.Configuration;

namespace SmartTooling_API._Repositories.Repositories.SmartTool
{
    public class Measurement_RFTRepository : MainRepository<Measurement_RFT>, IMeasurement_RFTRepository
    {
        private readonly DataContext _context;
        public Measurement_RFTRepository(DataContext context, CBDataContext CBcontext, SHCDataContext SHCcontext,
                            SPCDataContext SPCcontext, TSHDataContext TSHcontext, IConfiguration configuration) :
                            base(context, CBcontext, SHCcontext, SPCcontext, TSHcontext, configuration)
        {
            _context = context;
        }

        public async Task<bool> CheckExistsRTF(ModelOperationDTO operation)
        {
            if (await _context.Measurement_RFT.AnyAsync(x => x.factory_id == operation.factory_id &&
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