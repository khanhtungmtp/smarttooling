using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SmartTooling_API._Repositories.Interfaces.SmartTool;
using SmartTooling_API.Data;
using SmartTooling_API.Models.SmartTool;

namespace SmartTooling_API._Repositories.Repositories.SmartTool
{
    public class ModelRepository : MainRepository<Model>, IModelRepository
    {
        private readonly DataContext _context;
        public ModelRepository(DataContext context, CBDataContext CBcontext, SHCDataContext SHCcontext,
                            SPCDataContext SPCcontext, TSHDataContext TSHcontext, IConfiguration configuration) :
                            base(context, CBcontext, SHCcontext, SPCcontext, TSHcontext, configuration)
        {
            _context = context;
        }

        public async Task<Model> GetByFactoryAndModelNo(string facID, string modelNo)
        {
            var model = await _context.Model.Where(x => x.factory_id == facID && x.model_no == modelNo).FirstOrDefaultAsync();
            return model;
        }
    }
}