using Microsoft.Extensions.Configuration;
using SmartTooling_API._Repositories.Interfaces.BestLine;
using SmartTooling_API._Repositories.Repositories.SmartTool;
using SmartTooling_API.Data;
using SmartTooling_API.Models.BestLine;

namespace SmartTooling_API._Repositories.Repositories.BestLine
{
    public class BLAttachmentTypeRepository : BestLineRepository<BL_Attachment_Type>, IBLAttachmentTypeRepository
    {
        private readonly DataContext _context;
        public BLAttachmentTypeRepository(DataContext context, CBDataContext CBcontext, SHCDataContext SHCcontext, SPCDataContext SPCcontext, TSHDataContext TSHcontext, IConfiguration configuration) : base(context, CBcontext, SHCcontext, SPCcontext, TSHcontext, configuration)
        {
            _context = context;
        }
    }
}