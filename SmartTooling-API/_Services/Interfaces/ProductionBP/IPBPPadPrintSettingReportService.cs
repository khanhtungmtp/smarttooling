using System.Collections.Generic;
using System.Threading.Tasks;
using SmartTooling_API.DTO.ProductionBP;
using SmartTooling_API.Helpers.Params;
using SmartTooling_API.Helpers.Params.ProductionBP;

namespace SmartTooling_API._Services.Interfaces.ProductionBP
{
    public interface IPBPPadPrintSettingReportService
    {
        Task<List<KeyValuePair<string, string>>> GetAllFactory();

        Task<PageListUtility<PBP_Pad_Print_SettingDTO>> Search(PaginationParams pagination, PBPPadPrintSettingParams searchParam, bool isPaging = true);
    }
}