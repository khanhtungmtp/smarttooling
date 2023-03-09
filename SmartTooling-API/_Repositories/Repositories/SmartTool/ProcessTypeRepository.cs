using Microsoft.Extensions.Configuration;
using SmartTooling_API._Repositories.Interfaces.SmartTool;
using SmartTooling_API.Data;
using SmartTooling_API.Models.SmartTool;

namespace SmartTooling_API._Repositories.Repositories.SmartTool
{
    public class ProcessTypeRepository : MainRepository<Process_Type>, IProcessTypeRepository
    {
        private readonly DataContext _context;

        public ProcessTypeRepository(DataContext context, CBDataContext CBcontext, SHCDataContext SHCcontext,
                            SPCDataContext SPCcontext, TSHDataContext TSHcontext, IConfiguration configuration) :
                            base(context, CBcontext, SHCcontext, SPCcontext, TSHcontext, configuration)
        {
            _context = context;
        }
    }
}