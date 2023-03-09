using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartTooling_API._Repositories.Interfaces.SmartTool;
using SmartTooling_API.Data;
using SmartTooling_API.Models.SmartTool;
using SmartTooling_API.DTO.SmartTool;
using Microsoft.Extensions.Configuration;

namespace SmartTooling_API._Repositories.Repositories.SmartTool
{
    public class KaizenRepository : MainRepository<Kaizen>, IKaizenRepository
    {
        private readonly DataContext _context;
        public KaizenRepository(DataContext context, CBDataContext CBcontext, SHCDataContext SHCcontext,
                            SPCDataContext SPCcontext, TSHDataContext TSHcontext, IConfiguration configuration) :
                            base(context, CBcontext, SHCcontext, SPCcontext, TSHcontext, configuration)
        {
            _context = context;
        }

        public async Task<bool> CheckExistsKaizen(ModelOperationDTO operation)
        {
            if (await _context.Kaizen.AnyAsync(x => x.factory_id == operation.factory_id &&
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

        public async Task<bool> CheckKaizenDescriptionExist(KaizenDTO model)
        {
            if (await _context.Kaizen.AnyAsync(x => x.factory_id == model.factory_id &&
                                                    x.model_no == model.model_no &&
                                                    x.serial_no != model.serial_no &&
                                                    x.kaizen_description == model.kaizen_description))
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