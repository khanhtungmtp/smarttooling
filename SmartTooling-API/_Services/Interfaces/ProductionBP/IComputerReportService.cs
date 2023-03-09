using System.Collections.Generic;
using System.Threading.Tasks;
using SmartTooling_API.DTO.ProductionBP;
using SmartTooling_API.Helpers.Params;
using SmartTooling_API.Helpers.Params.ProductionBP;

namespace SmartTooling_API._Services.Interfaces.ProductionBP
{
    public interface IComputerReportService
    {
         Task<List<KeyValuePair<string, string>>> GetAllFactory();
         Task<PageListUtility<PBP_ComputerStitchingSettingDTO>> Search(PaginationParams pagination, ComputerReportParam searchParam,bool isPagging);
    }
}