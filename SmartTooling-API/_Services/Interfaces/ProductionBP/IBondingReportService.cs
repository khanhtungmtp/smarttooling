using System.Collections.Generic;
using System.Threading.Tasks;
using SmartTooling_API.DTO.ProductionBP;
using SmartTooling_API.Helpers.Params;
using SmartTooling_API.Helpers.Params.ProductionBP;

namespace SmartTooling_API._Services.Interfaces.ProductionBP
{
    public interface IBondingReportService
    {
        Task<List<KeyValuePair<string, string>>> GetAllFactory();
        Task<List<KeyValuePair<string, string>>> GetProcessType(string factory_id);
        Task<List<KeyValuePair<string, string>>> GetAutoTech(string factory_id);
        Task<PageListUtility<PBP_Bonding_Program_SettingDTO>> Search(PaginationParams pagination, BondingReportParam searchParam, bool isPaging);
    }
}