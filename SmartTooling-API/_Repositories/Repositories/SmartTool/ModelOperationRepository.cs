
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SmartTooling_API._Repositories.Interfaces.SmartTool;
using SmartTooling_API.Data;
using SmartTooling_API.Helpers.Params.SmartTool;
using SmartTooling_API.Models.SmartTool;

namespace SmartTooling_API._Repositories.Repositories.SmartTool
{
    public class ModelOperationRepository : MainRepository<Model_Operation>, IModelOperationRepository
    {
        private readonly DataContext _context;
        public ModelOperationRepository(DataContext context, CBDataContext CBcontext, SHCDataContext SHCcontext,
                            SPCDataContext SPCcontext, TSHDataContext TSHcontext, IConfiguration configuration) :
                            base(context, CBcontext, SHCcontext, SPCcontext, TSHcontext, configuration)
        {
            _context = context;
        }

        public async Task<Model_Operation> GetByModelOperation(ModelOperationEditParam modelParam)
        {
            var modelOperation = await _context.Model_Operation.Where(x => x.factory_id == modelParam.factory_id
            && x.model_no == modelParam.model_no && x.stage_id == modelParam.stage_id && x.operation_id == modelParam.operation_id)
            .FirstOrDefaultAsync();
            return modelOperation;
        }
    }
}