using System.Threading.Tasks;
using SmartTooling_API.Data;
using SmartTooling_API.Models.SmartTool;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SmartTooling_API._Repositories.Interfaces.SmartTool;

namespace SmartTooling_API._Repositories.Repositories.SmartTool
{
    public class DefectReasonRepository : MainRepository<Defect_Reason>, IDefectReasonRepository
    {
        private readonly DataContext _context;
        public DefectReasonRepository(DataContext context, CBDataContext CBcontext, SHCDataContext SHCcontext,
                                SPCDataContext SPCcontext, TSHDataContext TSHcontext, IConfiguration configuration) :
                                base(context, CBcontext, SHCcontext, SPCcontext, TSHcontext, configuration)
        {
            _context = context;
        }

        public async Task<bool> CheckDefectReasonExists(string defectreasonID)
        {
            if (await _context.Defect_Reason.AnyAsync(x => x.defect_reason_id == defectreasonID))
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